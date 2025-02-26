namespace LibWorkWithFiles;

/// <summary>
///  Класс, для удаления задач
/// </summary>
public static class DeleteTask
{
    /// <summary>
    /// Метод, удаляющий задачу из списка
    /// </summary>
    /// <param name="path">Путь к файлу, где лежат задачи</param>
    /// <param name="tasks">Список задач</param>
    public static void Delete(ref string path, List<Tasks> tasks)
    {
        Console.WriteLine("Введите ID задачи, которую желаете удалить");
        int id = MethodsFindAndCheck.CheckId(tasks);
        //нахождение задачи по ID и её удаление
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].ID == id)
            {
                tasks.RemoveAt(i);
            }
        }
        WriteToFile.WriteBackToFile(ref path, tasks);
    }
}