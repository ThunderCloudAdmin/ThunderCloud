namespace ThunderClient.WebApp.Services.Interfaces;

public interface IFileWatcherService : IDisposable
{
    public void AddWatcher(string folderPath);

    public void RemoveWatcher(string folderPath);
}
