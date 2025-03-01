using System.Globalization;
using Spectre.Console;

namespace LibWorkWithFiles;

public static class FiltrAndSortForTable
{
    public static void FilterSort(List<Tasks> tasks)
    {
       bool exitFlag = true;
       List<Action<List<Tasks>>> actions = new List<Action<List<Tasks>>>();
       List<string> parametrs = new List<string>();
       do
       {
            Console.Clear();
            AnsiConsole.Markup("[bold cyan]Фильтрация и сортировка[/]");
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
                            .AddChoices("Статус","Приоритет","Дата создания","Дата изменения"));

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
                    else if (filterType == "Дата создания")
                    {
                        string dateCreate = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Задача существует: [/]:")
                                .AddChoices("1-7 дней", "8-14 дней", "Более 14"));
                        if (dateCreate == "1-7 дней")
                        {
                            actions.Add(list => list.RemoveAll(t => DateTime.Now.Day - t.GetCreatedAt().Day > 7));
                            parametrs.Add($"Фильтрация: Критерий -  дата создания {dateCreate}");
                        }
                        else if (dateCreate == "8-14 дней")
                        {
                            actions.Add(list => list.RemoveAll(t => DateTime.Now.Day - t.GetCreatedAt().Day <=7 || DateTime.Now.Day - t.GetCreatedAt().Day > 14));
                            parametrs.Add($"Фильтрация: Критерий -  дата создания {dateCreate}");
                        }
                        else if (dateCreate == "Более 14")
                        {
                            actions.Add(list => list.RemoveAll(t => DateTime.Now.Day - t.GetCreatedAt().Day <= 14));
                            parametrs.Add($"Фильтрация: Критерий -  дата создания {dateCreate}");
                        }
                        
                    }
                    else if (filterType == "Дата изменения")
                    {
                        string dateUpdate = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Задача была изменена: [/]:")
                                .AddChoices("1-7 дней", "8-14 дней", "Более 14"));
                        if (dateUpdate == "1-7 дней")
                        {
                            actions.Add(list => list.RemoveAll(t => DateTime.Now.Day - t.GetUpdatedAt().Day > 7));
                            parametrs.Add($"Фильтрация: Критерий -  дата изменения {dateUpdate}");
                        }
                        else if (dateUpdate == "8-14 дней")
                        {
                            actions.Add(list => list.RemoveAll(t => DateTime.Now.Day - t.GetUpdatedAt().Day <=7 || DateTime.Now.Day - t.GetCreatedAt().Day > 14));
                            parametrs.Add($"Фильтрация: Критерий -  дата изменения {dateUpdate}");
                        }
                        else if (dateUpdate == "Более 14")
                        {
                            actions.Add(list => list.RemoveAll(t => DateTime.Now.Day - t.GetUpdatedAt().Day <= 14));
                            parametrs.Add($"Фильтрация: Критерий -  дата изменения {dateUpdate}");
                        }
                        
                    }
                    break;
                case "Отсортировать": 
                    string sortField = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[green]Выберите критерий сортировки[/]:")
                            .AddChoices("ID", "Статус (TODO - IN_PROGRESS - DONE)","Статус ( DONE - IN_PROGRESS - TODO)", "Приоритет (Высокий - Средний  -Низкий)","Приоритет (Низкий - Средний -Высокий)", "Описание","Дата создания (сначала старые)","Дата изменения  (сначала старые)","Дата создания (сначала новые)","Дата изменения  (сначала новые)"));

                    if (sortField == "ID")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderBy(task => task.ID).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Статус ( DONE - IN_PROGRESS - TODO)")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderBy(task => task.Status).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Статус (TODO - IN_PROGRESS - DONE)")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderByDescending(task => task.Status).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Приоритет (Высокий - Средний  -Низкий)")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderBy(task => task.PriorytiImportance(task.Priority)).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Приоритет (Низкий - Средний -Высокий)")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderByDescending(task => task.PriorytiImportance(task.Priority)).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Описание")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderBy(task => task.Desc).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Дата создания (сначала старые)")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderBy(task => task.GetCreatedAt()).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Дата изменения  (сначала старые)")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderBy(task => task.GetUpdatedAt()).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Дата создания (сначала новые)")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderByDescending(task => task.GetCreatedAt()).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
                    }
                    else if (sortField == "Дата изменения  (сначала новые)")
                    {
                        actions.Add(list => 
                        {
                            List<Tasks> sortedList = list.OrderByDescending(task => task.GetUpdatedAt()).ToList();
                            list.Clear();
                            list.AddRange(sortedList);
                        });
                        parametrs.Add($"Сортировка: Критерий - {sortField}");
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
                    List<Tasks> filteredTasks = new List<Tasks>(tasks); // Копия исходного списка
                    foreach (Action<List<Tasks>> action in actions)
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
                    table.AddColumn("Дедлайн").Centered();
                    table.AddColumn("Шкала прогресса").Centered();
                    
                    
                    foreach (Tasks task in filteredTasks)
                    {
                        string bar = ProgressBar.Progress(task.PercentComplete);
                        if (task.GetCreatedAt().Second != task.GetUpdatedAt().Second & task.GetDeadLine()== default)
                        {
                            table.AddRow(task.ID.ToString(), task.Status, task.Priority, task.Desc,
                                $"{task.GetCreatedAt():dd-MM-yy HH:mm}",
                                $"{task.GetUpdatedAt():dd-MM-yy HH:mm}","Нет дедлайна",bar);
                        }
                        else if (task.GetCreatedAt().Second != task.GetUpdatedAt().Second & task.GetDeadLine()!= default)
                        {
                            if (task.GetDeadLine() < DateTime.Now)
                            {
                                table.AddRow($"[red]{task.ID.ToString()}[/]", $"[red]{task.Status}[/]", $"[red]{task.Priority}[/]", $"[red]{task.Desc}[/]",
                                    $"[red]{task.GetCreatedAt():dd-MM-yy HH:mm}[/]",
                                    $"[red]{task.GetUpdatedAt():dd-MM-yy HH:mm}[/]",
                                    $"[red]{task.GetDeadLine():dd-MM-yy HH:mm}[/]",bar);
                            }
                            else
                            {
                                table.AddRow($"[green]{task.ID.ToString()}[/]", $"[green]{task.Status}[/]", $"[green]{task.Priority}[/]", $"[green]{task.Desc}[/]",
                                    $"[green]{task.GetCreatedAt():dd-MM-yy HH:mm}[/]",
                                    $"[green]{task.GetUpdatedAt():dd-MM-yy HH:mm}[/]",
                                    $"[green]{task.GetDeadLine():dd-MM-yy HH:mm}[/]",bar);
                            }
                        }
                        else if (task.GetCreatedAt().Second == task.GetUpdatedAt().Second &
                                 task.GetDeadLine() == default)
                        {
                            table.AddRow(task.ID.ToString(), task.Status, task.Priority, task.Desc,
                                $"{task.GetCreatedAt():dd-MM-yy HH:mm}",
                                "Задача не изменялась","Нет дедлайна",bar);
                        }
                        else if (task.GetCreatedAt().Second == task.GetUpdatedAt().Second &
                                 task.GetDeadLine() != default)
                        {
                            if (task.GetDeadLine() < DateTime.Now)
                            {
                                table.AddRow($"[red]{task.ID.ToString()}[/]", $"[red]{task.Status}[/]", $"[red]{task.Priority}[/]", $"[red]{task.Desc}[/]",
                                    $"[red]{task.GetCreatedAt():dd-MM-yy HH:mm}[/]",
                                    "[red]Задача не изменялась[/]", $"[red]{task.GetDeadLine():dd-MM-yy HH:mm}[/]", bar);
                            }
                            else
                            {
                                table.AddRow($"[green]{task.ID.ToString()}[/]", $"[green]{task.Status}[/]", $"[green]{task.Priority}[/]", $"[green]{task.Desc}[/]",
                                    $"[green]{task.GetCreatedAt():dd-MM-yy HH:mm}[/]",
                                    "[green]Задача не изменялась[/]", $"[green]{task.GetDeadLine():dd-MM-yy HH:mm}[/]",bar);
                            }
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