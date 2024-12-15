namespace Homework.Delegates.Searcher;

public class FileSearcher
{
    private bool _cancelSearch;
    public event EventHandler<FileArgs> FileFound;
    public event EventHandler SearchStarted;
    public event EventHandler SearchIsStopped;
    public async Task Search(string filename, string driverPath, CancellationToken cancellationToken)
    {
        var driver = new DirectoryInfo(driverPath);

        _cancelSearch = false;
        await OnSearchStarted();

        // Асинхронный поиск в каталоге
        await SearchInDirectory(filename, driver, cancellationToken);
        OnSearchIsStopped();
    }

    private async Task SearchInDirectory(string filename, DirectoryInfo di, CancellationToken cancellationToken)
    {
        if (_cancelSearch || cancellationToken.IsCancellationRequested)
            return;

        try
        {
            await SearchInFiles(filename, di, cancellationToken);

            foreach (var directory in di.GetDirectories())
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await SearchInDirectory(filename, directory, cancellationToken);
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Нет доступа к директории. Сообщение: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка доступа к директории. Сообщение: {ex.Message}");
        }
    }

    private async Task SearchInFiles(string filename, DirectoryInfo di, CancellationToken cancellationToken)
    {
        var files = di.GetFiles();
        foreach (var fileInfo in files)
        {

            if (cancellationToken.IsCancellationRequested)
                return;

            var src = fileInfo.Name.Split('.')[0].Trim().ToLower();
            if (src != filename.Trim().ToLower()) continue;

            OnFileFound(fileInfo);
        }
    }

    protected virtual void OnFileFound(FileInfo fileInfo)
    {
        FileFound?.Invoke(this, new FileArgs(fileInfo.FullName));
    }

    public void CancelSearch() => _cancelSearch = true;

    protected virtual async Task OnSearchStarted()
    {
        SearchStarted?.Invoke(this, EventArgs.Empty);
    }
    protected virtual void OnSearchIsStopped()
    {
        SearchIsStopped?.Invoke(this, EventArgs.Empty);
    }
}