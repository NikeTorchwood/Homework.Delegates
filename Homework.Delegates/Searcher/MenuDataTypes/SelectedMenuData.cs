namespace Homework.Delegates.Searcher.MenuDataTypes;

public class SelectedMenuData
{
    public string Title { get; }
    public IReadOnlyCollection<string> Items { get; private set; }
    public SelectedMenuData(string title, List<string> items)
    {
        Title = title;
        items.Add("Exit");
        Items = items;
    }
}