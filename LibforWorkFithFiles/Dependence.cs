using System.Text;
using MenuLib;
using Spectre.Console;

namespace LibWorkWithFiles;

/// <summary>
/// Класс, отвечающий за работу зависимостей.
/// </summary>
public static class Dependence
{
    /// <summary>
    /// Выбор пункта меню зависимостей
    /// </summary>
    /// <param name="tasks">Список задач</param>
    public static void ChooseDepAction(List<Task> tasks)
    {
        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Frame.PrintFrame(Frame.ForPrint(Texts.ChooseDependAct)); //меню выбора
            key = Console.ReadKey(true);
            switch(key.KeyChar)
            {
                case '1':
                    NewDependence(tasks);
                    break;
                case '2':
                    DeleteDependency(tasks);
                    break;
                case '3':
                    TableDependency(tasks);
                    break;
            }
            
        }while(key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.D3);
    }

    /// <summary>
    /// Метод, выводящий таблицу в зависимостями
    /// </summary>
    /// <param name="tasks">Список задач.</param>
    private static void TableDependency(List<Task> tasks)
    {
        Console.Clear();
        var table = new Table();
        table.AddColumn(new TableColumn("Задача").Centered());
        table.AddColumn(new TableColumn("Задачи, зависящие от этой").Centered());
        foreach (Task task in tasks)
        {
            if (task.GetDependency().Count != 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (int dep in task.GetDependency())
                {
                    sb.AppendLine($"{dep}. {MethodsFindAndCheck.FindById(dep,tasks).Desc}");
                }
                table.AddRow($"{task.ID}. {task.Desc}", $"{sb}");
            }
        }
        table.Border(TableBorder.Rounded);
        AnsiConsole.Write(table);
    }
    
    /// <summary>
    /// Метод для создания новой зависимости
    /// </summary>
    /// <param name="tasks">Список задач</param>
    private static void NewDependence(List<Task> tasks)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Введите ID задачи, которая должна ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("ЗАВИСЕТЬ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(" от другой.");
        Console.ResetColor();
        
        //ID первой задачи
        int idFirst= MethodsFindAndCheck.CheckId(tasks);
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Введите ID задачи, от которой  ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("ЗАВИСИТ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(" первая.");
        Console.ResetColor();
        
        //ID второй задачи
        int idSecond = MethodsFindAndCheck.CheckId(tasks);
        try
        {
            Task task2 = MethodsFindAndCheck.FindById(idSecond, tasks);
            Task task1 = MethodsFindAndCheck.FindById(idFirst, tasks);
            MethodsFindAndCheck.CheckDependency(task1, task2); //проверка корректности добавления зависимости
            task2.AddDependency(idFirst);
            task1.SetDependenciesIdThisFrom(idSecond);
            Console.Clear();
            Console.WriteLine($"Добавлена зависимость: {idFirst} от {idSecond} ");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Зависимость уже существует или задача 2 окончена. Процесс прерван.");
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Вы попытались создать циклическую зависимость. Процесс прерван.");
        }

    }

    private static void DeleteDependency(List<Task> tasks)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Введите ID задачи, ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("У КОТОРОЙ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(" должна быть удалена зависимость.");
        Console.ResetColor();
        
        //ID задачи, у которой должна быть удалена зависимость
        int idFirst= MethodsFindAndCheck.CheckId(tasks);
        Task task1 = MethodsFindAndCheck.FindById(idFirst, tasks);
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("Введите ID задачи, которую нужно  ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("ОСВОБОДИТЬ ОТ ЗАВИСИМОСТИ");
        Console.ResetColor();
        
        //ID задачи, которую нужно освободить от зависимости
        int idSecond = MethodsFindAndCheck.CheckId(tasks);
        Task task2 = MethodsFindAndCheck.FindById(idSecond, tasks);

        bool isInDep = false;
        foreach (int dep1 in task2.GetDependencyThisFrom())
        {
            if (dep1 == idFirst)
            {
                isInDep = true;
            }
        }

        if (isInDep)
        {
            task1.DeleteDependencyFromThis(idSecond);
            task2.DeleteDependencyThisFrom(idFirst);
            Console.WriteLine("Зависимость удалена");
        }
        else
        {
            Console.WriteLine("Данной зависимости не существует");
        }
        
    }
    
}