using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;

namespace LibWorkWithFiles;

/// <summary>
/// Класс, для записи данных в файл
/// </summary>
public static class WriteToFile
{
    /// <summary>
    /// Метод, для записи данных в файл
    /// </summary>
    /// <param name="path">Ссылка на файл для записи</param>
    /// <param name="tasks">Данные задач для записи</param>
    public static void WriteBackToFile(ref string path,List<Tasks> tasks)
    {
        IOrderedEnumerable<Tasks> res;
        res = from task in tasks orderby task.ID select task;
        tasks = res.ToList(); //сортировка по ID для лучшего вида файла
        if (!File.Exists(path))
        {
            ConsoleKeyInfo key;
            Console.WriteLine("Файла для записи задач не существует. Если хотите его создать, нажмите 1");
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.D1)
            {
                Console.WriteLine("Введите путь к файлу: ");
                do
                {
                    string path1 = Console.ReadLine();
                    if (File.Exists(path1))
                    {
                        //в зависимости от формата, свой метод записи
                        if (Path.GetExtension(path1) == ".json")
                        {
                            WriteJson(path1, tasks);
                            path = path1;
                        }
                        else if (Path.GetExtension(path1) == ".txt")
                        {
                            WriteTxt(path1, tasks);
                            path = path1;
                        }
                        else if (Path.GetExtension(path1) == ".csv")
                        {
                            WriteCsv(path1, tasks);
                            path = path1;
                        }
                        else
                        {
                            Console.WriteLine("Данный формат файла не поддерживается.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Файла не существует. Попробуйте снова");
                    }
                }while(!File.Exists(path));
            }
        }
        else 
        {
            //в зависимости от формата, свой метод записи
            string ext = Path.GetExtension(path);
            switch (ext)
            {
              case ".json":
                  WriteJson(path, tasks);
                  break;
              case ".csv":
                  WriteCsv(path, tasks);
                  break;
              case ".txt":
                  WriteTxt(path, tasks);
                  break;
            }
        }
    }

    /// <summary>
    /// Запись для JSON файла
    /// </summary>
    /// <param name="path">Ссылка для записи</param>
    /// <param name="tasks">Данные задач для записи</param>
    private static void WriteJson(string path, List<Tasks> tasks)
    {
        StringBuilder str = new StringBuilder();
        str.AppendLine("[");
        for(int i = 0; i < tasks.Count; i++)
        {
            str.AppendLine("\t{");
            str.AppendLine($"\t\t\"ID\": {tasks[i].ID},");
            str.AppendLine($"\t\t\"Status\": \"{tasks[i].Status}\",");
            str.AppendLine($"\t\t\"Priority\": \"{tasks[i].Priority}\",");
            str.AppendLine($"\t\t\"Desc\": \"{tasks[i].Desc}\"");
            if (i < tasks.Count - 1)
            {
                str.AppendLine("\t},");
            }
            else
            {
                str.AppendLine("\t}");
            }

        }
        str.AppendLine("]");
        string res = str.ToString();
        File.WriteAllText(path, res);
    }

    /// <summary>
    /// Запись для CSV файла
    /// </summary>
    /// <param name="path">Ссылка для записи</param>
    /// <param name="tasks">Данные задач для записи</param>
    private static void WriteCsv(string path, List<Tasks> tasks)
    {
        List<string> lines = new List<string>();
        lines.Add("ID,Status,Priority,Desc");
        foreach (Tasks task in tasks)
        {
            if (task.Desc.Contains(',')) //если есть запятая, добавляем кавычки
            {
                lines.Add($"{task.ID},{task.Status},{task.Priority},\"{task.Desc}\"");
            }
            else
            {
                lines.Add($"{task.ID},{task.Status},{task.Priority},{task.Desc}");
            }
        }
        File.WriteAllLines(path, lines);
    }

    /// <summary>
    /// Запись для текстового файла
    /// </summary>
    /// <param name="path">Ссылка для записи</param>
    /// <param name="tasks">Данные задач для записи</param>
    private static void WriteTxt(string path, List<Tasks> tasks)
    {
        List<string> lines = new List<string>();
        foreach (Tasks task in tasks)
        {
            lines.Add($"[{task.ID}] [{task.Status}] [{task.Priority}] {task.Desc}");
        }
        File.WriteAllLines(path, lines);
    }
    
}