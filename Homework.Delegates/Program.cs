using Homework.Delegates.FirstExercise;
using Homework.Delegates.Searcher;
using Homework.Delegates.Searcher.Menu;
using Homework.Delegates.Searcher.MenuDataTypes;

namespace Homework.Delegates;

public class Program
{
    private static async Task Main()
    {
        // Пример 1: Поиск максимального элемента
        var numbers = new List<Person>
            {
                new Person { Name = "Alice", Age = 25 },
                new Person { Name = "Bob", Age = 30 },
                new Person { Name = "Charlie", Age = 22 }
            };

        var oldest = numbers.GetMax(p => p.Age);
        Console.WriteLine($"Самый старший: {oldest.Name}, возраст: {oldest.Age}");

        //Пример 2: Поиск файлов

        var menu = new Menu();
        var drivers = DriveInfo.GetDrives();
        var mainMenuData = new SelectedMenuData(
            "Список дисков",
            drivers.Select(driveInfo => driveInfo.Name).ToList());

        var selectedItem = menu.PrintMenu(mainMenuData);
        if (selectedItem == mainMenuData.Items.Count - 1) return;

        var fileNameMenu = new InputMenuData("Напишите имя файла:",
            Console.ReadLine());

        var searcher = new FileSearcher();
        var cancellationTokenSource = new CancellationTokenSource();

        // Обработчик события нахождения файла
        searcher.FileFound += (sender, args) =>
        {
            Console.WriteLine($"Файл найден: {args.FileName}");
        };
        //Обработчик события при завершении поиска
        searcher.SearchIsStopped += (sender, args) => Console.WriteLine("Поиск завершен");
        // Обработчик начала поиска
        searcher.SearchStarted += async (sender, args) =>
        {
            var senderSearcher = (FileSearcher)sender;
            Console.WriteLine("Для остановки поиска нажми \"S\"");
            var inputTask = Task.Run(() =>
            {
                ConsoleKeyInfo ki;
                do
                {
                    ki = Console.ReadKey(true);
                } while (ki.Key != ConsoleKey.S);

                senderSearcher.CancelSearch();
                cancellationTokenSource.Cancel();
                Console.WriteLine("Поиск принудительно остановлен");
            }, cancellationTokenSource.Token);
            
            await inputTask;
        };

        // Запуск поиска
        await searcher.Search(fileNameMenu.Input, drivers[selectedItem].Name, cancellationTokenSource.Token);
    }
}