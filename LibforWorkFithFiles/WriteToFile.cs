using System.Globalization;
using System.Text;
using System.Text.Json;
using CsvHelper;

namespace LibWorkWithFiles
{

    /// <summary>
    /// Класс, для записи данных в файл
    /// </summary>
    public static class WriteToFile
    {
        /// <summary>
        /// Метод, для записи данных в файл
        /// </summary>
        /// <param name="path">Ссылка на файл для записи</param>
        /// <param name="projects">Данные проектов для записи</param>
        public static void WriteBackToFile(ref string path, List<Project> projects)
        {
            Console.WriteLine("Вы уверены, что хотите перезаписать файл?\n 1 - Да \n 2 - Нет");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            bool shure = true;
            do
            {
                switch (keyInfo.KeyChar)
                {
                    case '1':
                        shure = true;
                        break;
                    case '2':
                        shure = false;
                        break;
                        
                }
            }while (keyInfo.Key != ConsoleKey.D1 && keyInfo.Key != ConsoleKey.D2);

            if (shure)
            {
                List<Tasks> tasks = new List<Tasks>();
                foreach (Project project in projects)
                {
                    foreach (Tasks task in project)
                    {
                        tasks.Add(task);
                    }
                }

                IOrderedEnumerable<Tasks> res;
                res = from task in tasks orderby task.Id select task;
                tasks = res.ToList(); //сортировка по ID для лучшего вида файла
                if (!File.Exists(path))
                {
                    ConsoleKeyInfo key;
                    Console.WriteLine("Файла для записи задач не существует. Если хотите добавить свой, нажмите 1");
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
                        } while (!File.Exists(path));
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
            for (int i = 0; i < tasks.Count; i++)
            {
                str.AppendLine("\t{");
                str.AppendLine($"\t\t\"Id\": {tasks[i].Id},");
                str.AppendLine($"\t\t\"Status\": \"{tasks[i].Status}\",");
                str.AppendLine($"\t\t\"Priority\": \"{tasks[i].Priority}\",");
                str.AppendLine($"\t\t\"Desc\": \"{tasks[i].Desc}\",");
                str.AppendLine($"\t\t\"CreatedAt\": \"{tasks[i].GetCreatedAt():dd-MM-yy HH:mm}\",");
                if (tasks[i].GetCreatedAt().Second != tasks[i].GetUpdatedAt().Second)
                {
                    str.AppendLine($"\t\t\"Updated\": \"{tasks[i].GetUpdatedAt():dd-MM-yy HH:mm}\",");
                }
                if (tasks[i].GetDeadLine() != default)
                {
                    str.AppendLine($"\t\t\"DeadLine\": \"{tasks[i].GetDeadLine():dd-MM-yy HH:mm}\",");
                }

                str.AppendLine($"\t\t\"PercentComplete\": {tasks[i].PercentComplete},");
                if (tasks[i].GetDependency().Count != 0)
                {
                    str.AppendLine($"\t\t\"DependencyFromThis\": \"{tasks[i].DependencyFromThis}\"");
                }
                str.AppendLine($"\t\t\"InProject\": \"{tasks[i].InProject}\"");
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
            StringBuilder str = new StringBuilder();
            str.Append("Id,");
            str.Append("Status,");
            str.Append("Priority,");
            str.Append("Desc,");
            str.Append("CreatedAt,");
            str.Append("Updated,");
            str.Append("InProject,");
            str.Append("DependencyFromThis,");
            str.Append("DeadLine");
            lines.Add(str.ToString());
            foreach (Tasks task in tasks)
            {
                str.Clear();
                str.Append($"{task.Id},");
                str.Append($"{task.Status},");
                str.Append($"{task.Priority},");
                if (task.Desc.Contains(',')) //если есть запятая, добавляем кавычки
                {
                    str.Append($"\"{task.Desc}\",");
                }
                else
                {
                    str.Append($"{task.Desc},");
                }

                str.Append($"{task.GetCreatedAt():dd-MM-yy HH:mm},");
                str.Append($"{task.GetUpdatedAt():dd-MM-yy HH:mm},");
                str.Append($"{task.InProject},");
                if (task.GetDependency().Count != 0)
                {
                    str.Append($"\"{task.DependencyFromThis}\",");
                }
                else
                {
                    str.Append("-,");
                }

                if (task.GetDeadLine() != default)
                {
                    str.Append($"{task.GetDeadLine():dd-MM-yy HH:mm}");
                }
                else
                {
                    str.Append("-,");
                }

                lines.Add(str.ToString());
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
                lines.Add($"[{task.Id}] [{task.Status}] [{task.Priority}] {task.Desc}");
            }

            File.WriteAllLines(path, lines);
        }
    }
}