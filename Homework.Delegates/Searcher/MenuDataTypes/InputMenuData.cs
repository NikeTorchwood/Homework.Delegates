namespace Homework.Delegates.Searcher.MenuDataTypes;

public class InputMenuData(string title, string? input)
{
    public string Title { get; } = title;
    public string? Input { get; } = input;
}