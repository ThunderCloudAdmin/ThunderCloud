using ThunderClient.WebApp.Services.Interfaces;

namespace ThunderClient.WebApp.Services;

/// <summary>
/// Provides functionality to manage file system watchers for specified folders.
/// </summary>
public class FileWatcherService : IFileWatcherService
{
    private readonly Dictionary<string, FileSystemWatcher> _watchers = new Dictionary<string, FileSystemWatcher>(StringComparer.OrdinalIgnoreCase); // Case-insensitive comparison

    /// <summary>
    /// Adds a new file system watcher for the specified folder.
    /// </summary>
    /// <param name="folderPath">The absolute path of the folder to monitor.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the folder is already being monitored, or if it is a child directory of an existing watcher.
    /// </exception>
    public void AddWatcher(string folderPath)
    {
        folderPath = Path.GetFullPath(folderPath); // Ensure the path is absolute

        // Check if the folder is a child or parent of an existing watcher
        if (HandleParentChildConflict(folderPath))
        {
            Console.WriteLine($"Watcher for {folderPath} has been added, and necessary adjustments were made.");
            return;
        }

        if (_watchers.ContainsKey(folderPath))
        {
            throw new InvalidOperationException($"Folder {folderPath} is already being monitored.");
        }

        CreateAndStartWatcher(folderPath);
    }    

    /// <summary>
    /// Removes the file system watcher for the specified folder.
    /// </summary>
    /// <param name="folderPath">The absolute path of the folder to stop monitoring.</param>
    public void RemoveWatcher(string folderPath)
    {
        folderPath = Path.GetFullPath(folderPath); // Ensure the path is absolute

        if (_watchers.TryGetValue(folderPath, out var watcher))
        {
            watcher.EnableRaisingEvents = false;

            // Unsubscribe and dispose
            watcher.Created -= OnChanged;
            watcher.Deleted -= OnChanged;
            watcher.Changed -= OnChanged;
            watcher.Renamed -= OnRenamed;
            watcher.Dispose();

            _watchers.Remove(folderPath);
        }
    }

    /// <summary>
    /// Checks if the new folder path is a parent or child directory of any existing watcher.
    /// </summary>
    /// <param name="newFolderPath">The absolute path of the new folder to check.</param>
    /// <returns>
    /// <c>true</c> if the new folder path is a parent directory of an existing watcher and thus requires removal of the existing watchers;
    /// otherwise, <c>false</c>.
    /// </returns>
    private bool HandleParentChildConflict(string newFolderPath)
    {
        var pathsToRemove = new List<string>();

        foreach (var existingFolderPath in _watchers.Keys)
        {
            if (IsParentDirectory(newFolderPath, existingFolderPath))
            {
                // New folder is a parent of an existing folder, remove the child and allow the parent to take over
                pathsToRemove.Add(existingFolderPath);
            }
            else if (IsParentDirectory(existingFolderPath, newFolderPath))
            {
                // New folder is a child of an existing folder, reject the new watcher
                throw new InvalidOperationException($"Cannot monitor {newFolderPath} as it is a child directory of an existing watcher.");
            }
        }

        // Remove the existing child watchers (if any)
        foreach (var pathToRemove in pathsToRemove)
        {
            RemoveWatcher(pathToRemove);
        }

        // Create and start the new parent watcher
        if (pathsToRemove.Count > 0)
        {
            CreateAndStartWatcher(newFolderPath);
            return true; // Indicate that the new parent watcher was added successfully
        }

        return false; // No conflicts, proceed normally
    }

    /// <summary>
    /// Creates and starts a new <see cref="FileSystemWatcher"/> for the specified folder path.
    /// </summary>
    /// <param name="folderPath">The absolute path of the folder to monitor.</param>
    private void CreateAndStartWatcher(string folderPath)
    {
        var watcher = new FileSystemWatcher(folderPath)
        {
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName
        };

        // Subscribe to events
        watcher.Created += OnChanged;
        watcher.Deleted += OnChanged;
        watcher.Changed += OnChanged;
        watcher.Renamed += OnRenamed;

        _watchers[folderPath] = watcher;
    }

    /// <summary>
    /// Determines whether the specified parent path is a parent directory of the specified child path.
    /// </summary>
    /// <param name="parentPath">The absolute path of the potential parent directory.</param>
    /// <param name="childPath">The absolute path of the potential child directory.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="childPath"/> is within <paramref name="parentPath"/>; otherwise, <c>false</c>.
    /// </returns>
    private static bool IsParentDirectory(string parentPath, string childPath)
    {
        // Normalize both paths to ensure correct comparison
        var parentFullPath = Path.GetFullPath(parentPath).TrimEnd(Path.DirectorySeparatorChar);
        var childFullPath = Path.GetFullPath(childPath).TrimEnd(Path.DirectorySeparatorChar);

        // Check if child path starts with the parent path
        return childFullPath.StartsWith(parentFullPath + Path.DirectorySeparatorChar);
    }

    /// <summary>
    /// Handles file system changes such as creation, deletion, or modification of files.
    /// </summary>
    /// <param name="sender">The <see cref="FileSystemWatcher"/> instance that triggered the event.</param>
    /// <param name="e">The <see cref="FileSystemEventArgs"/> containing the event data.</param>
    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        // Handle file created/changed/deleted event
        Console.WriteLine($"File {e.ChangeType}: {e.FullPath}");
    }

    /// <summary>
    /// Handles the renaming of files or directories.
    /// </summary>
    /// <param name="sender">The <see cref="FileSystemWatcher"/> instance that triggered the event.</param>
    /// <param name="e">The <see cref="RenamedEventArgs"/> containing the event data.</param>
    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        // Handle renamed event
        Console.WriteLine($"File renamed from {e.OldFullPath} to {e.FullPath}");
    }

    /// <summary>
    /// Disposes of all file system watchers and releases resources.
    /// </summary>
    public void Dispose()
    {
        foreach (var watcher in _watchers.Values)
        {
            watcher.Dispose();
             GC.SuppressFinalize(this);
        }

        _watchers.Clear();
    }
}