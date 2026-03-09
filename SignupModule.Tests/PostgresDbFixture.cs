
using Testcontainers.PostgreSql;

namespace SignupModule.Tests;

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<PostgresDbFixture>
{
}
public class PostgresDbFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public string ConnectionString => _container.GetConnectionString();
    public PostgresDbFixture()
    {
        _container = new PostgreSqlBuilder("postgres:18-bookworm").Build();
    }
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
