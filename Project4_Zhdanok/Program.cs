using LibWorkWithFiles;

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
    static async Task Main()
    {
        bool alreadyBotInUse = false;
        Console.CursorVisible = false;
        List<Tasks> tasks = new List<Tasks>(); // список задач
        Frame.PrintFrame(Frame.ForPrint(Texts.Description)); // меню приветствия
        Console.ReadKey(true);
        Console.Clear();
        string pathToFile = string.Empty;
        ConsoleKeyInfo menuKey;
        Console.CursorVisible = false;
        _ = TelegramBot.SendMessageAsync();
            do
            {
                Frame.PrintFrame(Frame.ForPrint(Texts.ChoosePoint)); // меню
                CancellationTokenSource cts = new CancellationTokenSource();
                Task deadlineCheckTask = Task.Run(() => DeadLines.CheckDeadLinesAsync(tasks, cts.Token));
                menuKey = Console.ReadKey(true);
                Console.Clear();
                switch (menuKey.KeyChar)
                {
                    //Ввод данных
                    case '1':
                        do
                        {
                            pathToFile = ImportFilesAsync.GetPass();
                            tasks = await ImportFilesAsync.FileHandler(pathToFile);
                        } while (tasks == null);
                        break;
                    
                    //Вывод задач
                    case '2':
                        if (tasks.Count == 0)
                            Console.WriteLine("Задачи не найдены");
                        else
                            ShowTasks.Show(tasks);
                        break;
                    
                    //Добавление задач
                    case '3':
                        tasks.Add(AddTask.AddTasks(tasks));
                        WriteToFile.WriteBackToFile(ref pathToFile, tasks);
                        break;
                    
                    //Изменение статуса
                    case '4':
                        if (tasks.Count == 0)
                            Console.WriteLine("Задачи не найдены");
                        else
                            ChangeStatusTask.Change(ref pathToFile, tasks);
                        break;
                    //Удаление задачи
                    case '5':
                        if (tasks.Count == 0)
                            Console.WriteLine("Задачи не найдены");
                        else
                            DeleteTask.Delete(ref pathToFile, tasks);
                        break;
                    
                    //управление зависимостями
                    case '6':
                        if (tasks.Count == 0)
                            Console.WriteLine("Задачи не найдены");
                        else
                            Dependence.ChooseDepAction(tasks);
                        break;
                    
                    //управление дедлайнами
                    case '7':
                        if (tasks.Count == 0)
                            Console.WriteLine("Задачи не найдены");
                        else
                        {
                            DeadLines.ChooseDeadLineAction(tasks);
                            WriteToFile.WriteBackToFile(ref pathToFile, tasks);
                        }
                        break;
                    
                    //Добавление процента выполнения
                    case '8':
                        if (tasks.Count == 0)
                            Console.WriteLine("Задачи не найдены");
                        else
                        {
                            AddPersentComplete.AddPersent(tasks);
                            WriteToFile.WriteBackToFile(ref pathToFile, tasks);
                        }
                        break;

                    case '9': // Завершение программы
                        cts.Cancel(); // Останавливаем фоновую задачу
                        break;

                    default:
                        Console.WriteLine("Введите один из пунктов меню");
                        break;
                }
                cts.Cancel();
            } while (menuKey.Key != ConsoleKey.D9);
        
    }
}