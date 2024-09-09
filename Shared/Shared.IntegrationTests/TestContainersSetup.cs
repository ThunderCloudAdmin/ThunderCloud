public class TestContainersSetup : IAsyncLifetime
{
    private PostgreSqlContainer _postgresContainer;

    public string ConnectionString => _postgresContainer.GetConnectionString();

    public async Task InitializeAsync()
    {
        var builder = new ContainerBuilder<PostgreSqlContainer>()
            .ConfigureDatabaseConfiguration("TestDatabase", "postgres", "password")
            .Build();

        _postgresContainer = builder;

        await _postgresContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (_postgresContainer != null)
        {
            await _postgresContainer.StopAsync();
        }
    }
}
