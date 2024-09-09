public class PostgresTestContainer : IAsyncLifetime
{
    private readonly DockerClient _dockerClient;
    private string _containerId;
    private readonly string _connectionString;

    public PostgresTestContainer()
    {
        _dockerClient = new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
        _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=testdb";
    }

    public string ConnectionString => _connectionString;

    public async Task InitializeAsync()
    {
        var response = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
        {
            Image = "postgres:16",
            Env = new[] { "POSTGRES_PASSWORD=postgres", "POSTGRES_DB=testdb" },
            ExposedPorts = new Dictionary<string, EmptyStruct>
            {
                { "5432/tcp", default }
            },
            HostConfig = new HostConfig
            {
                PortBindings = new Dictionary<string, IList<PortBinding>>
                {
                    { "5432/tcp", new List<PortBinding> { PortBinding.CreateBoundPortBinding(5432) } }
                }
            }
        });

        _containerId = response.ID;
        await _dockerClient.Containers.StartContainerAsync(_containerId, new ContainerStartParameters());
    }

    public async Task DisposeAsync()
    {
        if (_containerId != null)
        {
            await _dockerClient.Containers.StopContainerAsync(_containerId, new ContainerStopParameters());
            await _dockerClient.Containers.RemoveContainerAsync(_containerId, new ContainerRemoveParameters { Force = true });
        }
    }
}
