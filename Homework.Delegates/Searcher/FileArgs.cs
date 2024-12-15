namespace Homework.Delegates.Searcher;

public class FileArgs(string fileName) : EventArgs
{
    public string FileName { get; } = fileName;
}