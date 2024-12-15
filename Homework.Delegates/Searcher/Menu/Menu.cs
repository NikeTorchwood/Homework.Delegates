using Homework.Delegates.Searcher.MenuDataTypes;

namespace Homework.Delegates.Searcher.Menu;

public class Menu
{
    public int PrintMenu(SelectedMenuData selectedMenuData)
    {
        Console.WriteLine(selectedMenuData.Title);
        var cursorPos = Console.GetCursorPosition().Top;
        ConsoleKeyInfo ki;
        var markerPos = 0;
        do
        {
            Console.SetCursorPosition(default, cursorPos);
            PrintItems(selectedMenuData.Items.ToArray(), markerPos);
            ki = Console.ReadKey();
            switch (ki.Key)
            {
                case ConsoleKey.LeftArrow:
                case ConsoleKey.UpArrow:
                    if (markerPos > 0)
                        markerPos--;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.DownArrow:
                    if (markerPos < selectedMenuData.Items.Count - 1)
                        markerPos++;
                    break;
            }
        } while (ki.Key != ConsoleKey.Enter);

        return markerPos;
    }

    private void PrintItems(string[] items, int markerPos)
    {
        for (var i = 0; i < items.Length; i++)
        {
            var marker = i == markerPos ? '>' : ' ';
            Console.WriteLine($"{marker}{items[i]}");
        }
    }
}