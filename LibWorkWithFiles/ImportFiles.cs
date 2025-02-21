using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;

namespace LibWorkWithFiles;

public class ImportFiles
{
    public static  List<Task> GetPass()
    {
        Console.WriteLine("Введите путь до файла ");
        string? path = Console.ReadLine();
        if (File.Exists(path))
        {
            string extension = Path.GetExtension(path);
            switch (extension)
            {
                case ".csv":
                    List<Task> resultTasks = new List<Task>();
                    using (StreamReader reader = new StreamReader(path))
                    {
                        using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            IEnumerable<Task> tasks = csv.GetRecords<Task>();
                            foreach (Task task in tasks)
                            {
                                resultTasks.Add(task);
                                Console.WriteLine(task);
                            }
                        }
                    }
                    return resultTasks;
                    
                case ".json":
                    List<string> lines2 = File.ReadAllLines(path).ToList();
                    List<string> resultListJson = BetweenStaples(string.Join(" ", lines2));
                    foreach (string line in resultListJson)
                    {
                        Task task = JsonSerializer.Deserialize<Task>(line);
                        Console.WriteLine(task);
                    }

                    break;
                case ".txt":
                    string[] lines = File.ReadAllLines(path);
                    string pattern2 = @"\[(\d+)\] \[(\w+)\] \[(\w+)\] (.*)";
                    List<Task> resultTasks2 = new List<Task>();
                    foreach (string line in lines)
                    {
                        Match afterReg = Regex.Match(line, pattern2);
                        if (afterReg.Groups.Count == 5)
                        {
                            if (int.TryParse(afterReg.Groups[1].Value, out int result))
                            {
                                Task task = new Task(result, afterReg.Groups[3].ToString().Replace("[","").Replace("]",""), afterReg.Groups[2].ToString().Replace("[","").Replace("]",""), afterReg.Groups[4].ToString().Replace("[","").Replace("]",""));
                                resultTasks2.Add(task);
                                Console.WriteLine(task);
                            }
                            else
                            {
                                throw new FormatException();
                            }
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    }
                    return resultTasks2;
            }
        }
        return null;
    }
    
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