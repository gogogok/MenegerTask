using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;

namespace LibWorkWithFiles;

/// <summary>
/// Класс, отвечающий за обработку файла
/// </summary>
public static  class ImportFiles
{
    /// <summary>
    /// Метод, обрабатывающий файл
    /// </summary>
    /// <param name="path">Ссылка на файл, который нужно обработать</param>
    /// <returns>Список задач, полученный из этого файла </returns>
    public static  List<Tasks> GetPass(out string path)
    {
        Console.WriteLine("Введите путь до файла ");
        bool notExist = false;
        do
        {
            path = Console.ReadLine();
            if (File.Exists(path))
            {
                notExist = false;
                string extension = Path.GetExtension(path);
                switch (extension) //определение расширения файла
                {
                    case ".csv":
                        List<Tasks> resultTasks = new List<Tasks>();
                            using (StreamReader reader = new StreamReader(path))
                            {
                                using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                                {
                                    IEnumerable<Tasks> tasks = csv.GetRecords<Tasks>();
                                    foreach (Tasks task in tasks)
                                    {
                                        resultTasks.Add(task);
                                        //Console.WriteLine(task);
                                    }
                                }
                            }
                            return resultTasks;
                        
                        
                       // catch (CsvHelperException)
                        //{
                        Console.WriteLine("CSV файл не корректен. Попробуйте другой файл.");
                        notExist = true;
                        //}
                        break;

                    case ".json":
                        List<string> lines2 = File.ReadAllLines(path).ToList();
                        List<string> resultListJson = BetweenStaples(string.Join(" ", lines2));
                        List<Tasks> resultTasks3 = new List<Tasks>();
                        try
                        {
                            foreach (string line in resultListJson)
                            {
                                Tasks tasks = JsonSerializer.Deserialize<Tasks>(line);
                                resultTasks3.Add(tasks);
                                //Console.WriteLine(task);
                            }

                            return resultTasks3;
                        }
                        catch (System.Text.Json.JsonException)
                        {
                            Console.WriteLine("JSON файл не корректен. Попробуйте другой файл.");
                            notExist = true;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("JSON файл не корректен. Попробуйте другой файл.");
                            notExist = true;
                        }
                        break;
                    case ".txt":
                        string[] lines = File.ReadAllLines(path);
                        string pattern2 = @"\[(\d+)\] \[(\w+)\] \[(\w+)\] (.*)"; //паттерн для нахождения задач 
                        List<Tasks> resultTasks2 = new List<Tasks>();
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
                                        afterReg.Groups[4].ToString().Replace("[", "").Replace("]", ""),DateTime.Now);
                                    resultTasks2.Add(tasks);
                                    //Console.WriteLine(task);
                                }
                                else
                                {
                                    throw new IndexOutOfRangeException();
                                }
                            }

                            return resultTasks2;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Неверная структура файла, попробуйте снова.");
                            notExist = true;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Неверная структура файла, попробуйте снова.");
                            notExist = true;
                        }
                        break;
                }
            }
            else
            {
                Console.WriteLine("Такого файла не существует, введи корректный путь до файла.");
                notExist = true;
            }
        }while(notExist);
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