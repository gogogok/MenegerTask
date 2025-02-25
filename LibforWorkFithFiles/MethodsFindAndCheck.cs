namespace LibWorkWithFiles;

/// <summary>
/// Класс для проверки корректности действий и нахождения задач
/// </summary>
public static class MethodsFindAndCheck
{
    /// <summary>
    /// Метод для получения ID предмета
    /// </summary>
    /// <param name="tasks">Список задач</param>
    /// <returns>ID задачи</returns>
    public static int CheckId(List<Task> tasks)
    {
        int id;
        while (true)
        {
            try
            {
                id = int.Parse(Console.ReadLine());
                bool isId = false;
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].ID == id)
                    {
                        isId = true;
                    }
                }

                if (!isId)
                {
                    throw new IndexOutOfRangeException();
                }
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Число не было введено. Повторите ввод");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Задачи с таким ID не существует. Повторите ввод");
            }
        }
        return id;
    }

    /// <summary>
    /// Нахождение предмета по ID
    /// </summary>
    /// <param name="id">ID предмета</param>
    /// <param name="tasks">Список задач</param>
    /// <returns></returns>
    public static Task FindById(int id, List<Task> tasks)
    {
        foreach (Task task in tasks)
        {
            if (task.ID == id)
            {
                return task;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Проверка на наличие противоречащих зависимостей
    /// </summary>
    /// <param name="taskDepFrom">Задача, которая должна зависеть от другой</param>
    /// <param name="taskThisDep">Задача, от которой должно зависеть</param>
    public static void CheckDependency(Task taskDepFrom, Task taskThisDep)
    {
        if (taskThisDep.GetDependency().Count!= 0)
        {
            for (int i = 0; i < taskThisDep.GetDependency().Count; i++)
            {
                if (taskThisDep.GetDependency()[i] == taskDepFrom.ID)
                {
                    throw new ArgumentException();
                }

                if (taskThisDep.ID == taskDepFrom.ID)
                {
                    throw new ArgumentException();
                }
                
            }
        }

        if (taskThisDep.GetDependencyThisFrom().Count != 0)
        {
            for (int i = 0; i < taskThisDep.GetDependencyThisFrom().Count; i++)
            {
                if (taskThisDep.GetDependencyThisFrom()[i] == taskDepFrom.ID)
                {
                    throw new TimeoutException();
                }
            }
        }

        if (taskThisDep.Status == "DONE")
        {
            throw new ArgumentException();
        }
    }
}