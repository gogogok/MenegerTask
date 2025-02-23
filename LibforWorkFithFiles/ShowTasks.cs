using MenuLib;
using Spectre.Console;

namespace LibWorkWithFiles;

public static class ShowTasks
{

    public static void Show(List<Task> tasks)
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
                    break;
            }
        } while (key1.Key != ConsoleKey.D1 & key1.Key != ConsoleKey.D2);
    }
    
    
    private static void ShowToConsole(List<Task> tasks)
    {
        foreach (Task task in tasks)
        {
            Console.WriteLine(task);
        }
    }

    private static void Table(List<Task> tasks)
    {
        Console.Clear();
        Table table = new Table();
        table.AddColumn(new TableColumn("ID").Centered());
        table.AddColumn(new TableColumn("Статус").Centered());
        table.AddColumn(new TableColumn("Приоритет").Centered());
        table.AddColumn(new TableColumn("Описание").Centered());
        foreach (Task task in tasks)
        {
            table.AddRow(new Markup($"[blue]{task.ID}[/]"), new Markup($"{task.Status}"), new Markup($"{task.Priority}"),new Markup($"{task.Desc}"));
        }
        table.Border(TableBorder.Rounded);
        AnsiConsole.Write(table);
    }
    
    
}

