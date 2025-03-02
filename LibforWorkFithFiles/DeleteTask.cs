namespace LibWorkWithFiles
{

    /// <summary>
    ///  Класс, для удаления задач
    /// </summary>
    public static class DeleteTask
    {
        /// <summary>
        /// Метод, удаляющий задачу из списка
        /// </summary>
        /// <param name="path">Путь к файлу, где лежат задачи</param>
        /// <param name="projects">Список проектов</param>
        ///  /// <param name="tasks">Список задач</param>
        public static void Delete(ref string path, List<Project> projects, List<Tasks> tasks)
        {
            if (tasks.Count != 0)
            {
                Console.WriteLine("Введите ID задачи, которую желаете удалить");
                int id = MethodsFindAndCheck.CheckId(tasks);
                //нахождение задачи по ID и её удаление
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].Id == id)
                    {
                        tasks.RemoveAt(i);
                    }
                }

                foreach (Project project in projects)
                {
                    foreach (Tasks task in project)
                    {
                        if (task.Id == id)
                        {
                            project.RemoveFromPr(task);
                            break;
                        }
                    }
                }
                WriteToFile.WriteBackToFile(ref path, projects);
            }
            else
            {
                Console.WriteLine("Задачи не были найдены");
            }
        }
    }
}