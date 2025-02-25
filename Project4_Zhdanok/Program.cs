using LibWorkWithFiles;
using Task = LibWorkWithFiles.Task;

namespace  Project4_Zhdanok;
//Жданок Дарья
//БПИ248
//Вариант 2
//B-side
using MenuLib;

/// <summary>
/// Класс, отвечающий за основную работу программы
/// </summary>
internal class Program
{
    /// <summary>
    /// Точка входа в программу
    /// </summary>
    internal static void Main()
    {
        List<Task> tasks = new List<Task>();//список задач
        Frame.PrintFrame(Frame.ForPrint(Texts.Description));//меню приветствия
        ConsoleKeyInfo _ = Console.ReadKey(true);
        Console.Clear();
        string pathToFile = String.Empty;
        ConsoleKeyInfo menuKey;
        do
        {
            Frame.PrintFrame(Frame.ForPrint(Texts.ChoosePoint)); //меню
            menuKey = Console.ReadKey(true);
            Console.Clear();
            switch (menuKey.KeyChar)
            {
                //получение данных из файла
                case '1':
                    tasks = ImportFiles.GetPass(out pathToFile);
                    //Console.WriteLine(tasks.Count);
                    break;
                //вывод задач
                case '2':
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("Задачи не найдены");
                    }
                    else
                    {
                        ShowTasks.Show(tasks);
                    }
                    break;
                //добавление задачи
                case '3':
                    tasks.Add(AddTask.AddTasks(tasks, pathToFile));
                    WriteToFile.WriteBackToFile(ref pathToFile, tasks);
                    break;
                //изменение статуса задачи
                case '4':
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("Задачи не найдены");
                    }
                    else
                    {
                        ChangeStatusTask.Change(ref pathToFile, tasks);
                    }
                    break;
                //удаление задачи
                case '5':
                    if (tasks.Count== 0)
                    {
                        Console.WriteLine("Задачи не найдены");
                    }
                    else
                    {
                        DeleteTask.Delete(ref pathToFile, tasks);
                    }
                    break;
                //добавление зависимости
                case '6':
                    if (tasks.Count == 0)
                    {
                        Console.WriteLine("Задачи не найдены");
                    }
                    else
                    {
                        Dependence.ChooseDepAction(tasks);
                    }
                    break;
                //для выхода из программы
                case '7':
                    break;
                //при не выборе ни одного из пунктов
                default:
                    Console.WriteLine("Введите один из пунктов меню");
                    break;
            }
        } while (menuKey.Key != ConsoleKey.D7);
        
    }
}