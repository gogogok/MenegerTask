using System.Globalization;
using MenuLib;
using Spectre.Console;

namespace LibWorkWithFiles;

/// <summary>
/// Класс для вывода информации
/// </summary>
public static class ShowTasks
{

    public static void Show(List<Tasks> tasks)
    {
        ConsoleKeyInfo key1;
        do
        {
            Frame.PrintFrame(Frame.ForPrint(Texts.ChooseShow));
            key1 = Console.ReadKey(true);
            switch (key1.KeyChar)
            {
                case '1':
                    Console.Clear();
                    ShowToConsole(tasks);
                    break;
                case '2':
                    Table(tasks);
                    Console.Clear();
                    break;
            }

           
        } while (key1.Key != ConsoleKey.D1 & key1.Key != ConsoleKey.D2);
    }
    
    
    private static void ShowToConsole(List<Tasks> tasks)
    {
        foreach (Tasks task in tasks)
        {
            if (task.GetDeadLine() != default)
            {
                if (task.GetDeadLine() < DateTime.Now)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            Console.WriteLine(task);
            Console.ResetColor();
            
        }
    }

    private static void Table(List<Tasks> tasks)
    {
        Console.Clear();
        FiltrAndSortForTable.FilterSort(tasks);
       
    }
}