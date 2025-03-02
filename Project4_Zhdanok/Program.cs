using LibWorkWithFiles;

namespace Project4_Zhdanok
{
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
        private static async Task Main()
        {
            bool alreadyBotInUse = false;
            Console.CursorVisible = false;
            List<Tasks> tasks = new List<Tasks>();
            List<Project> projects = new List<Project>(); //список проектов
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
                Task deadlineCheckTask = Task.Run(() => DeadLines.CheckDeadLinesAsync(projects, cts.Token));
                menuKey = Console.ReadKey(true);
                Console.Clear();
                switch (menuKey.KeyChar)
                {
                    //Ввод данных
                    case '1':
                        do
                        {
                            pathToFile = ImportFilesAsync.GetPass();
                            projects = await ImportFilesAsync.FileHandler(pathToFile);
                        } while (projects == null);

                        foreach (Project project in projects)
                        {
                            foreach (Tasks task in project)
                            {
                                tasks.Add(task);
                            }
                        }

                        break;

                    //Вывод задач
                    case '2':
                        if (projects.Count == 0)
                        {
                            Console.WriteLine("Проекты не найдены");
                        }
                        else
                        {
                            ShowTasks.Show(projects, tasks);
                        }

                        break;

                    //Добавление задач
                    case '3':
                        Tasks taskNew = AddTask.AddTasks(projects, tasks);
                        tasks.Add(taskNew);
                        WriteToFile.WriteBackToFile(ref pathToFile, projects);
                        break;

                    //Изменение статуса
                    case '4':
                        if (projects.Count == 0)
                        {
                            Console.WriteLine("Проекты не найдены");
                        }
                        else
                        {
                            ChangeStatusTask.Change(ref pathToFile, projects, tasks);
                        }

                        break;
                    //Удаление задачи
                    case '5':
                        if (projects.Count == 0)
                        {
                            Console.WriteLine("Проекты не найдены");
                        }
                        else
                        {
                            DeleteTask.Delete(ref pathToFile, projects, tasks);
                        }

                        break;

                    //управление зависимостями
                    case '6':
                        if (projects.Count == 0)
                        {
                            Console.WriteLine("Проекты не найдены");
                        }
                        else
                        {
                            Dependence.ChooseDepAction(projects, tasks);
                            WriteToFile.WriteBackToFile(ref pathToFile, projects);
                        }

                        break;

                    //управление дедлайнами
                    case '7':
                        if (projects.Count == 0)
                        {
                            Console.WriteLine("Проекты не найдены");
                        }
                        else
                        {
                            DeadLines.ChooseDeadLineAction(tasks);
                            WriteToFile.WriteBackToFile(ref pathToFile, projects);
                        }

                        break;

                    //Добавление процента выполнения
                    case '8':
                        if (projects.Count == 0)
                        {
                            Console.WriteLine("Задачи не найдены");
                        }
                        else
                        {
                            AddPersentComplete.AddPersent(tasks);
                            WriteToFile.WriteBackToFile(ref pathToFile, projects);
                        }

                        break;

                    //работа с проектами
                    case '9':
                        WorkWithProjects.ProjectsOptions(projects, tasks, ref pathToFile);
                        break;

                    case 'F': // Завершение программы
                        cts.Cancel(); // Останавливаем фоновую задачу
                        break;

                    default:
                        Console.WriteLine("Введите один из пунктов меню");
                        break;
                }

                cts.Cancel();
            } while (menuKey.Key != ConsoleKey.F);

        }
    }
}