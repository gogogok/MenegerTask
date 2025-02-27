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
    /// <param name="tasks">Список задач</param>
    public static void ChooseDeadLineAction(List<Tasks> tasks, ref bool alrBotInUse)
    {
        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Frame.PrintFrame(Frame.ForPrint(Texts.ChooseDeadLineAct));
            key = Console.ReadKey(true);
            switch (key.KeyChar)
            {
                case '1':
                    AddDeadLine(tasks,ref alrBotInUse);
                    break;
                case '2':
                    DeleteDeadLines(tasks);
                    break;
            }
        }while(key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2);
    }
    
    /// <summary>
    /// Метод для добавления нового дедлайна
    /// </summary>
    /// <param name="tasks">Список задач</param>
    private static void AddDeadLine(List<Tasks> tasks, ref bool alreadyBotInUse)
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
                if (DateTime.TryParseExact(date, patternDate, null, DateTimeStyles.None, out DateTime parsedDate))
                {
                    Tasks task = MethodsFindAndCheck.FindById(id, tasks);
                    task.SetDeadLine(parsedDate);
                    task.SetUpdatedAt(DateTime.Now); //запись изменения задачи
                    if (!alreadyBotInUse)
                    {
                        _ = TelegramBot.SendMessageAsync();
                        alreadyBotInUse = true;
                    }
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
            task.SetUpdatedAt(DateTime.Now);
            Console.WriteLine("Дедлайн успешно удалён");
        }
    }

    /// <summary>
    /// Метод, фоново проверяющий дедлайны и отсылающий о них сообщения в бот
    /// </summary>
    /// <param name="tasks">Список задач</param>
    /// <param name="token">Токен для прекращения работы</param>
    public static async Task CheckDeadLinesAsync(List<Tasks> tasks, CancellationToken token)
    {
        while (!token.IsCancellationRequested) // Проверяем флаг отмены
        {
            if (tasks.Count != 0)
            {
                foreach (Tasks task in tasks)
                {
                    if (task.GetDeadLine() != default)
                    {
                        if (DateTime.Now.Minute == task.GetDeadLine().Minute)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"Дедлайн задачи {task.ID} просрочен.");
                            await TelegramBot.NotificationAcync($"Дедлайн задачи \n\n {task.ID}. {task.Desc}\n\n просрочен.");
                        }
                        else if (DateTime.Now.Minute - task.GetDeadLine().Minute == 1 )
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"До конца дедлайна задачи {task.ID} меньше 1 минуты, ОЧЕНЬ поторопитесь!");
                            await TelegramBot.NotificationAcync($"До конца дедлайна задачи \n\n {task.ID}. {task.Desc}\n\n меньше 1 минуты, ОЧЕНЬ поторопитесь!");
                        }
                        else if (DateTime.Now.Minute - task.GetDeadLine().Minute == 5 )
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"До конца дедлайна задачи {task.ID} меньше 5 минут, поторопитесь!");
                            await TelegramBot.NotificationAcync($"До конца дедлайна задачи \n\n {task.ID}. {task.Desc}\n\n меньше 5 минут, поторопитесь!");
                        }
                        else if (DateTime.Now.Minute - task.GetDeadLine().Minute == 10 )
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"До конца дедлайна задачи {task.ID} меньше 10 минут, поторопитесь!");
                            await TelegramBot.NotificationAcync($"До конца дедлайна задачи \n\n {task.ID}. {task.Desc}\n\n меньше 10 минут, поторопитесь!");
                        }
                        else if (DateTime.Now.Minute - task.GetDeadLine().Minute == 59 )
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"До конца дедлайна задачи {task.ID} меньше одного часа!");
                            await TelegramBot.NotificationAcync($"До конца дедлайна задачи \n\n {task.ID}. {task.Desc}\n\n меньше одного часа!");
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