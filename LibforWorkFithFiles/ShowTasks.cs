using System.Globalization;
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
                    Console.Clear();
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
        FiltrSort(tasks);
       
    }

    private static void FiltrSort(List<Task> tasks)
    {
       bool exitFlag = true;
       List<Action<List<Task>>> actions = new List<Action<List<Task>>>();
       List<string> parametrs = new List<string>();
       do
       {
            Console.Clear();
            AnsiConsole.Markup("[bold red]Фильтрация и сортировка[/]");
            Console.WriteLine();
            if (actions.Count > 0)
            {
                AnsiConsole.Markup("\n[green]Выбранные фильтры и метод сортировки:[/]\n");
                for (int i = 0; i < actions.Count; i++)
                {
                    AnsiConsole.Markup($"[blue]{i + 1}[/]: {parametrs[i]}\n");
                }
            }
            else
            {
                AnsiConsole.Markup("\n[grey]Ничего не выбрано.[/]\n");
            }

            Console.WriteLine();
            
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Выберите действие:[/]")
                    .AddChoices("Добавить фильтр", "Отсортировать", "Удалить критерий", "Показать задачи", "Вернуться в меню "));
            switch (choice)
            {
                case "Добавить фильтр":
                    string filterType = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[green]Выберите фильтр[/]:")
                            .AddChoices("Статус","Приоритет"));

                    if (filterType == "Статус")
                    {
                        string status = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Выберите статус[/]:")
                                .AddChoices("TODO", "IN_PROGRESS", "DONE"));
                        actions.Add(list => list.RemoveAll(t => t.Status != status));
                        parametrs.Add($"Фильтрация: Критерий - Статус {status}");
                    }
                    else if (filterType == "Приоритет")
                    {
                        string priority = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Выберите приоритет[/]:")
                                .AddChoices("Высокий", "Средний", "Низкий"));

                        actions.Add(list => list.RemoveAll(t => t.Priority != priority));
                        parametrs.Add($"Фильтрация: Критерий - Приоритет {priority}");
                    }
                    break;
                case "Отсортировать": 
                    string sortField = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[green]Выберите критерий сортировки[/]:")
                            .AddChoices("ID", "Статус", "Приоритет", "Описание"));

                    if (sortField == "ID")
                    {
                        actions.Add(list => 
                        {
                            List<Task> sortedList = list.OrderBy(task => task.ID).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add("Сортировка: Критерий - ID");
                    }
                    else if (sortField == "Статус")
                    {
                        actions.Add(list => 
                        {
                            List<Task> sortedList = list.OrderBy(task => task.Status).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add("Сортировка: Критерий - Статус");
                    }
                    else if (sortField == "Приоритет")
                    {
                        actions.Add(list => 
                        {
                            List<Task> sortedList = list.OrderBy(task => task.Priority).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add("Сортировка: Критерий - Приоритет");
                    }
                    else if (sortField == "Описание")
                    {
                        actions.Add(list => 
                        {
                            List<Task> sortedList = list.OrderBy(task => task.Desc).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add("Сортировка: Критерий - Описание");
                    }
                    break;
                
                case "Удалить критерий":
                    if (actions.Count > 0)
                    {
                        int actionToRemove = AnsiConsole.Prompt(
                            new SelectionPrompt<int>()
                                .Title("Выберите действие для удаления:")
                                .AddChoices(Enumerable.Range(1, actions.Count).ToList()));

                        actions.RemoveAt(actionToRemove - 1);
                        parametrs.RemoveAt(actionToRemove - 1);
                    }
                    else
                    {
                        AnsiConsole.Markup("[red]Нет критериев для удаления[/]");
                        Console.ReadKey();
                    }
                    break;
                
                case "Показать задачи":
                    List<Task> filteredTasks = new List<Task>(tasks); // Копия исходного списка
                    foreach (Action<List<Task>> action in actions)
                    {
                        action(filteredTasks);
                    }

                    // Вывести таблицу
                    Table table = new Table();
                    table.AddColumn("ID").Centered();
                    table.AddColumn("Статус").Centered();
                    table.AddColumn("Приоритет").Centered();
                    table.AddColumn("Описание").Centered();
                    table.AddColumn("Время создания").Centered();
                    table.AddColumn("Время изменения").Centered();

                    foreach (Task task in filteredTasks)
                    {
                        if (task.GetCreatedAt().Second != task.GetUpdatedAt().Second)
                        {
                            table.AddRow(task.ID.ToString(), task.Status, task.Priority, task.Desc,
                                task.GetCreatedAt().ToString(CultureInfo.InvariantCulture),
                                task.GetUpdatedAt().ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            table.AddRow(task.ID.ToString(), task.Status, task.Priority, task.Desc,
                                task.GetCreatedAt().ToString(CultureInfo.InvariantCulture),
                                "Задача не изменялась");
                        }
                    }

                    table.Border(TableBorder.Rounded);
                    AnsiConsole.Write(table);
                    Console.WriteLine("Нажмите любую кнопку, чтобы снова вернуться в меню выбора");
                    Console.ReadKey();
                    break;
                
                case "Вернуться в меню ":
                    exitFlag = false;
                    break;
            }
       }while(exitFlag);
    }
}

