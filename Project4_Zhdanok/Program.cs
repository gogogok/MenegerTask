using LibWorkWithFiles;
using Task = LibWorkWithFiles.Task;

namespace  Project4_Zhdanok;
using MenuLib;

internal class Program
{
    public static void Main()
    {
        List<Task> tasks = new();
        Frame.PrintFrame(Frame.ForPrint(Texts.Description));
        ConsoleKeyInfo _ = Console.ReadKey(true);
        Console.Clear();
        ConsoleKeyInfo menuKey;
        do
        {
            Frame.PrintFrame(Frame.ForPrint(Texts.ChoosePoint));
            menuKey = Console.ReadKey(true);
            Console.Clear();
            switch (menuKey.KeyChar)
            {
                case '1':
                    tasks = ImportFiles.GetPass();
                    break;
                case '2':
                    if (tasks.Count < 0)
                    {
                        Console.WriteLine("Задачи не найдены");
                    }
                    else
                    {
                        
                    }
                    break;
                case '3':
                    
                    break;
                case '4':
                    if (tasks.Count < 0)
                    {
                        Console.WriteLine("Задачи не найдены");
                    }
                    else
                    {
                        
                    }
                    break;
                case '5':
                    if (tasks.Count < 0)
                    {
                        Console.WriteLine("Задачи не найдены");
                    }
                    else
                    {
                        
                    }
                    break;
                case '6':
                    break;
                default:
                    Console.WriteLine("Введите один из пунктов меню");
                    break;
            }
            
        } while (menuKey.Key != ConsoleKey.D6);
        
    }
}