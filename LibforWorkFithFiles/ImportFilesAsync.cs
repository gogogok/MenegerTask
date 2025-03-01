using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;

namespace LibWorkWithFiles;

/// <summary>
/// Класс, отвечающий за обработку файла
/// </summary>
public static  class ImportFilesAsync
{

    /// <summary>
    /// Метод, получающий правильный путь к файлу
    /// </summary>
    /// <returns></returns>
    public static string GetPass()
    {
        Console.WriteLine("Введите путь до файла ");
        string path = string.Empty;
        bool notExist = true;
        do
        {
            path = Console.ReadLine();
            if (File.Exists(path))
            {
                notExist = false;
                
            }
            else
            {
                Console.WriteLine("Такого файла не существует, введи корректный путь до файла.");
                notExist = true;
            }
        }while(notExist);
        return path;
    }

    /// <summary>
    /// Метод, обрабатывающий файл
    /// </summary>
    /// <param name="path">Ссылка на файл, который нужно обработать</param>
    /// <returns>Список задач, полученный из этого файла </returns>
    public static async Task<List<Project>> FileHandler(string path)
    {
        string extension = Path.GetExtension(path);
        switch (extension) //определение расширения файла
        {
            case ".csv":
                List<Tasks> resultTasks = new List<Tasks>();
                List<Project> projects = new List<Project>();
                try
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                       
                        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            MissingFieldFound = null,  //конфигуратор для обработки отсутствия столбцов
                            IgnoreBlankLines = true  //конфигуратор для обработки пустых строк
                            
                        };
                        using (CsvReader csv = new CsvReader(reader, config))
                        {
                            //асинхронная операция чтения файла
                            IEnumerable<Tasks> tasks = await Task.Run(() => csv.GetRecords<Tasks>().ToList());
                            foreach (Tasks task in tasks)
                            {
                                resultTasks.Add(task);
                            }

                            foreach (Tasks task in resultTasks)
                            {
                                if (task.Status == "DONE")
                                {
                                    task.PercentComplete = 100;
                                }
                                else if (task.Status == "IN_PROGRESS" & task.PercentComplete == 0)
                                {
                                    task.PercentComplete = 50;
                                }
                                else if (task.Status == "TODO")
                                {
                                    task.PercentComplete = 0;
                                }
                                string prName = task.InProject;
                                Project? pr = MethodsFindAndCheck.FindByName(projects,prName);
                                if (pr == null)
                                {
                                    Project pr2 = new Project(prName);
                                    pr2.AddTaskInProject(task);
                                    projects.Add(pr2);
                                }
                                else
                                {
                                    pr.AddTaskInProject(task);
                                }
                            }
                            return projects;
                        }
                    }
                }
                catch (CsvHelperException)
                {
                    Console.WriteLine("Неверный формат CSV файла. (Если файл корректен, проверьте заменены ли пустые строки \"-\", проверьте также, не истекли ли дедлайны)");
                    return null;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Данные CSV файла некорректны");
                    return null;
                }
            
            case ".json":
                //асинхронная операция чтения файла
                string[] lines2 = await File.ReadAllLinesAsync(path);
                List<string> resultListJson = BetweenStaples(string.Join(" ", lines2));
                List<Tasks> resultTasks3 = new List<Tasks>();
                List<Project> projects3 = new List<Project>();
                try
                {
                    foreach (string line in resultListJson)
                    {
                        Tasks tasks = JsonSerializer.Deserialize<Tasks>(line);
                        resultTasks3.Add(tasks);
                    }

                    foreach (Tasks task in resultTasks3)
                    {
                        
                        if (task.Status == "DONE")
                        {
                           task.PercentComplete = 100;
                        }
                        else if (task.Status == "IN_PROGRESS" & task.PercentComplete == 0)
                        {
                            task.PercentComplete =50;
                        }
                        else if (task.Status == "TODO")
                        {
                            task.PercentComplete = 0;
                        }
                        string prName = task.InProject;
                        Project pr = new Project(prName);
                        if (!projects3.Contains(pr))
                        {
                            projects3.Add(pr);
                            pr.AddTaskInProject(task);
                        }
                        else
                        {
                            Project? project = MethodsFindAndCheck.FindByName(projects3,prName);
                            project.AddTaskInProject(task);
                        }
                    }
                    return projects3;
                    
                }
                catch (JsonException)
                {
                    Console.WriteLine("JSON файл не корректен. Попробуйте другой файл.");
                    return null;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Данные файла некорректны, повторите попытку");
                    return null;
                }
                break;
            case ".txt":
                //асинхронная операция чтения файла
                string[] lines = await File.ReadAllLinesAsync(path);
                string pattern2 = @"\[(\d+)\] \[(\w+)\] \[(\w+)\] (.*)"; //паттерн для нахождения задач 
                List<Tasks> resultTasks2 = new List<Tasks>();
                List<Project> resultProjects = new List<Project>();
                try
                {
                    foreach (string line in lines)
                    {
                        Match afterReg = Regex.Match(line, pattern2);
                        if (afterReg.Groups.Count == 5)
                        {
                            int result = int.Parse(afterReg.Groups[1].Value);
                            Tasks tasks = new Tasks(result,
                                afterReg.Groups[2].ToString().Replace("[", "").Replace("]", ""),
                                afterReg.Groups[3].ToString().Replace("[", "").Replace("]", ""),
                                afterReg.Groups[4].ToString().Replace("[", "").Replace("]", ""));
                            resultTasks2.Add(tasks);
                            //Console.WriteLine(task);
                        }
                        else
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }

                    Project pr = new Project("Задачи без проекта");
                    foreach (Tasks task in resultTasks2)
                    {
                        pr.AddTaskInProject(task);
                        task.InProject = pr.Name;
                    }
                    resultProjects.Add(pr);
                    return resultProjects;
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Неверная структура файла, попробуйте снова.");
                    return null;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Неверная структура файла, попробуйте снова.");
                    return null;
                }
            default:
                Console.WriteLine("Неверное расширение файла. Повторите ввод");
                Console.WriteLine();
                break;
        }
        return null;
    }
    
    /// <summary>
    /// Метод, находящий значения между фигурными скобками
    /// </summary>
    /// <param name="text">Строка, в которой нужно найти все значения между '{' и '}'</param>
    /// <returns> Список строк, находящихся между '{' и '}' </returns>
    public static List<string> BetweenStaples(string text)
    {
        List<string> results = new List<string>();
        int index = 0;
        int start = 0;

        for (int i = 1; i < text.Length - 1; i++)
        {
            if (text[i] == '{') //начало скобки
            {
                if (index == 0)
                {
                    start = i;
                }

                index++;
            }
            else if (text[i] == '}')
            {
                if (index > 0)
                {
                    index -= 1;
                    if (index == 0 && start != 0) //доходим до последней скобки и добавляем к результату информацию между ними
                    {
                        int end = i;
                        results.Add(text[start..(end + 1)]);
                        start = 0; // сбрасываем начало
                        index = 0;
                    }
                }
            }
        }

        return results;
    }
}