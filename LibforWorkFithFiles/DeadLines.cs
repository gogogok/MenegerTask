using System.Globalization;
using MenuLib;

namespace LibWorkWithFiles;

public static class DeadLines
{
    public static void ChooseDeadLineAction(List<Tasks> tasks)
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
                    AddDeadLine(tasks);
                    break;
                case '2':
                    DeleteDeadLines(tasks);
                    break;
            }
        }while(key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2);
    }
    
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
                string date = Console.ReadLine();
                if (DateTime.TryParseExact(date, patternDate, null, DateTimeStyles.None, out DateTime parsedDate))
                {
                    Tasks task = MethodsFindAndCheck.FindById(id, tasks);
                    task.SetDeadLine(parsedDate);
                    task.SetUpdatedAt(DateTime.Now);
                    break;
                }
                throw new FormatException();
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат даты или дедлайн уже прошёл");
            }
        }
    }
    
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
}