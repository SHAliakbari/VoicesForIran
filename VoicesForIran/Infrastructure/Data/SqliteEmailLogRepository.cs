using Microsoft.Data.Sqlite;
using VoicesForIran.Core.Interfaces;
using VoicesForIran.Core.Models;

namespace VoicesForIran.Infrastructure.Data;

/// <summary>
/// SQLite repository for logging email generation
/// Privacy: Only stores MP name, riding name, and timestamp - NO user data
/// </summary>
public sealed class SqliteEmailLogRepository : IEmailLogRepository, IAsyncDisposable
{
    private readonly string _connectionString;
    private readonly ILogger<SqliteEmailLogRepository> _logger;
    private SqliteConnection? _connection;

    public SqliteEmailLogRepository(IConfiguration configuration, ILogger<SqliteEmailLogRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("EmailLogDb")
            ?? "Data Source=email_log.db";
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await EnsureConnectionAsync(cancellationToken);

        const string createTableSql = """
            CREATE TABLE IF NOT EXISTS EmailGenerationLogs (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                MpName TEXT NOT NULL,
                RidingName TEXT NOT NULL,
                GeneratedAtUtc TEXT NOT NULL
            );

            CREATE INDEX IF NOT EXISTS IX_EmailGenerationLogs_RidingName 
            ON EmailGenerationLogs(RidingName);

            CREATE INDEX IF NOT EXISTS IX_EmailGenerationLogs_GeneratedAtUtc 
            ON EmailGenerationLogs(GeneratedAtUtc);
            """;

        await using var command = _connection!.CreateCommand();
        command.CommandText = createTableSql;
        await command.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogInformation("Email log database initialized");
    }

    public async Task LogEmailGenerationAsync(string mpName, string ridingName, CancellationToken cancellationToken = default)
    {
        await EnsureConnectionAsync(cancellationToken);

        const string insertSql = """
            INSERT INTO EmailGenerationLogs (MpName, RidingName, GeneratedAtUtc)
            VALUES (@MpName, @RidingName, @GeneratedAtUtc)
            """;

        await using var command = _connection!.CreateCommand();
        command.CommandText = insertSql;
        command.Parameters.AddWithValue("@MpName", mpName);
        command.Parameters.AddWithValue("@RidingName", ridingName);
        command.Parameters.AddWithValue("@GeneratedAtUtc", DateTime.UtcNow.ToString("O"));

        await command.ExecuteNonQueryAsync(cancellationToken);

        _logger.LogDebug("Logged email generation for MP: {MpName}, Riding: {RidingName}", mpName, ridingName);
    }

    public async Task<ImpactStats> GetImpactStatsAsync(CancellationToken cancellationToken = default)
    {
        await EnsureConnectionAsync(cancellationToken);

        // Get total count
        await using var countCommand = _connection!.CreateCommand();
        countCommand.CommandText = "SELECT COUNT(*) FROM EmailGenerationLogs";
        var totalCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync(cancellationToken));

        // Get unique ridings count
        await using var uniqueCommand = _connection.CreateCommand();
        uniqueCommand.CommandText = "SELECT COUNT(DISTINCT RidingName) FROM EmailGenerationLogs";
        var uniqueRidings = Convert.ToInt32(await uniqueCommand.ExecuteScalarAsync(cancellationToken));

        // Get top ridings
        const string topRidingsSql = """
            SELECT RidingName, MpName, COUNT(*) as EmailCount
            FROM EmailGenerationLogs
            GROUP BY RidingName, MpName
            ORDER BY EmailCount DESC
            LIMIT 10
            """;

        await using var topCommand = _connection.CreateCommand();
        topCommand.CommandText = topRidingsSql;

        var topRidings = new List<RidingStats>();
        await using var reader = await topCommand.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            topRidings.Add(new RidingStats
            {
                RidingName = reader.GetString(0),
                MpName = reader.GetString(1),
                EmailCount = reader.GetInt32(2)
            });
        }

        return new ImpactStats
        {
            TotalEmailsGenerated = totalCount,
            UniqueRidings = uniqueRidings,
            TopRidings = topRidings
        };
    }

    public async Task<IReadOnlyList<EmailGenerationLog>> GetRecentLogsAsync(int count = 10, CancellationToken cancellationToken = default)
    {
        await EnsureConnectionAsync(cancellationToken);

        var sql = $"""
            SELECT Id, MpName, RidingName, GeneratedAtUtc
            FROM EmailGenerationLogs
            ORDER BY GeneratedAtUtc DESC
            LIMIT {count}
            """;

        await using var command = _connection!.CreateCommand();
        command.CommandText = sql;

        var logs = new List<EmailGenerationLog>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            logs.Add(new EmailGenerationLog
            {
                Id = reader.GetInt32(0),
                MpName = reader.GetString(1),
                RidingName = reader.GetString(2),
                GeneratedAtUtc = DateTime.Parse(reader.GetString(3))
            });
        }

        return logs;
    }

    private async Task EnsureConnectionAsync(CancellationToken cancellationToken)
    {
        if (_connection is null)
        {
            _connection = new SqliteConnection(_connectionString);
            await _connection.OpenAsync(cancellationToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.DisposeAsync();
            _connection = null;
        }
    }
}
