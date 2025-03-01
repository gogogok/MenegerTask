using System.Globalization;
using MenuLib;

namespace LibWorkWithFiles;

/// <summary>
/// Класс, отвечающий за работу дедлайнов
/// </summary>
public static class DeadLines
{
    /// <summary>
    /// Меню для выбора действия с дедлайном
    /// </summary>
    /// /// <param name="tasks">Список задач</param>
    public static void ChooseDeadLineAction(List<Tasks> tasks)
    {
        ConsoleKeyInfo key;
        if (tasks.Count > 0)
        {
            do
            {
                Console.Clear();
                Frame.PrintFrame(Frame.ForPrint(Texts.ChooseDeadLineAct));
                key = Console.ReadKey(true);
                switch (key.KeyChar)
                {
                    case '1':
                        AddDeadLine(tasks);
                        break;
                    case '2':
                        DeleteDeadLines(tasks);
                        break;
                }
            } while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2);
        }
        else
        {
            Console.WriteLine("Задач не найдено");
        }
    }
    
    /// <summary>
    /// Метод для добавления нового дедлайна
    /// </summary>
    /// <param name="tasks">Список задач</param>
    private static void AddDeadLine(List<Tasks> tasks)
    {
        Console.WriteLine("Введите ID задачи, для которой желаете установить дедлайн");
        int id = MethodsFindAndCheck.CheckId(tasks);
        string patternDate = "dd-MM-yy HH:mm";
        while(true)
        {
            Console.WriteLine($"Введите дату в формате \"{patternDate}\"");
            try
            {
                Console.CursorVisible = true;
                string date = Console.ReadLine();
                if (DateTime.TryParseExact(date, patternDate, null, DateTimeStyles.None, out DateTime _))
                {
                    Tasks task = MethodsFindAndCheck.FindById(id, tasks);
                    task.DeadLine = date;
                    task.Updated(); //запись изменения задачи
                    break;
                }
                throw new FormatException(); //исключение, если не тот формат даты или дата дедлайна уже просрочена
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат даты или дедлайн уже прошёл");
            }
        }
    }
    
    /// <summary>
    /// Метод для удаления дедлайна
    /// </summary>
    /// <param name="tasks">Список задач</param>
    private static void DeleteDeadLines(List<Tasks> tasks)
    {
        Console.WriteLine("Введите ID задачи, для которой желаете удалить дедлайн");
        int id = MethodsFindAndCheck.CheckId(tasks);
        Tasks task = MethodsFindAndCheck.FindById(id, tasks);
        Console.Clear();
        if (task.GetDeadLine() == default)
        {
            Console.WriteLine("Дедлайна для этой задачи не существовало");
        }
        else
        {
            task.DeleteDeadLine();
            task.Updated();
            Console.WriteLine("Дедлайн успешно удалён");
        }
    }

    /// <summary>
    /// Метод, фоново проверяющий дедлайны и отсылающий о них сообщения в бот
    /// </summary>
    /// <param name="projects">Список проектов</param>
    /// <param name="token">Токен для прекращения работы</param>
    public static async Task CheckDeadLinesAsync(List<Project> projects, CancellationToken token)
    {
        List<Tasks> tasks = new List<Tasks>();
        foreach (Project project in projects)
        {
            foreach (Tasks task1 in project)
            {
                tasks.Add(task1);
            }
        }
        while (!token.IsCancellationRequested) // Проверяем флаг отмены
        {
            if (tasks.Count != 0)
            {
                foreach (Tasks task in tasks)
                {
                    int time = (task.GetDeadLine() - DateTime.Now.AddSeconds(-DateTime.Now.Second)).Minutes;
                    if (task.GetDeadLine() != default)
                    {
                        if (DateTime.Now > task.GetDeadLine() & DateTime.Now.Minute - task.GetDeadLine().Minute < 2)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"Дедлайн задачи {task.Id} проекта {task.InProject} просрочен.");
                            await TelegramBot.NotificationAcync($"Дедлайн задачи \n\n {task.Id}. {task.Desc} проекта {task.InProject}\n\n просрочен.");
                        }
                        else if (DateTime.Now.Year == task.GetDeadLine().Year & DateTime.Now.Month == task.GetDeadLine().Month & DateTime.Now.Day == task.GetDeadLine().Day & task.GetDeadLine().Hour - DateTime.Now.Hour == 1 & time  == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"До конца дедлайна задачи {task.Id}  проекта {task.InProject} меньше одного часа!");
                            await TelegramBot.NotificationAcync($"До конца дедлайна задачи \n\n {task.Id}. {task.Desc}  проекта {task.InProject}\n\n меньше одного часа!");
                        }
                        else if (DateTime.Now.Year == task.GetDeadLine().Year & DateTime.Now.Month == task.GetDeadLine().Month & DateTime.Now.Day == task.GetDeadLine().Day & ((DateTime.Now.Hour == task.GetDeadLine().Hour  & time == 0) |(DateTime.Now.Hour - task.GetDeadLine().Hour == 1 & time <0)))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"До конца дедлайна задачи {task.Id}  проекта {task.InProject} меньше 1 минуты, ОЧЕНЬ поторопитесь!");
                            await TelegramBot.NotificationAcync($"До конца дедлайна задачи \n\n {task.Id}. {task.Desc} проекта {task.InProject}\n\n меньше 1 минуты, ОЧЕНЬ поторопитесь!");
                        }
                        else if (DateTime.Now.Year == task.GetDeadLine().Year & DateTime.Now.Month == task.GetDeadLine().Month & DateTime.Now.Day == task.GetDeadLine().Day & ((DateTime.Now.Hour == task.GetDeadLine().Hour  & time == 4) |(DateTime.Now.Hour - task.GetDeadLine().Hour == 1 & time <0)))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"До конца дедлайна задачи {task.Id} проекта {task.InProject} меньше 5 минут, поторопитесь!");
                            await TelegramBot.NotificationAcync($"До конца дедлайна задачи \n\n {task.Id}. {task.Desc} проекта {task.InProject}\n\n меньше 5 минут, поторопитесь!");
                        }
                        else if (DateTime.Now.Year == task.GetDeadLine().Year & DateTime.Now.Month == task.GetDeadLine().Month & DateTime.Now.Day == task.GetDeadLine().Day & ((DateTime.Now.Hour == task.GetDeadLine().Hour  & time == 9) |(DateTime.Now.Hour - task.GetDeadLine().Hour == 1 & time <0)))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"До конца дедлайна задачи {task.Id} проекта {task.InProject} меньше 10 минут, поторопитесь!");
                            await TelegramBot.NotificationAcync($"До конца дедлайна задачи \n\n {task.Id}. {task.Desc} проекта {task.InProject}\n\n меньше 10 минут, поторопитесь!");
                        }
                    }
                }
            }
            Console.ResetColor();
            try
            {
                //задержка после каждой проверки на минуту и токен, который даёт понять, продолжать работу или остановить
                await Task.Delay(60000, token); 
            }
            catch (TaskCanceledException)
            {
                //выход из цикла, если работа должна быть прекращена
                break; 
            }
        }
    }
}