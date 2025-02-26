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
    public static int CheckId(List<Tasks> tasks)
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
    public static Tasks FindById(int id, List<Tasks> tasks)
    {
        foreach (Tasks task in tasks)
        {
            if (task.ID == id)
            {
                return task;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Метод, проверяющий, создаётся ли циклическая зависимость
    /// </summary>
    /// <param name="taskId">Id задачи, которая проверяется на наличие циклов</param>
    /// <param name="tasks">Список задач</param>
    /// <returns>True - зависимость циклична, false - в ином случае</returns>
    public static bool IsCircled(int taskId,List<Tasks> tasks)
    {
        //стек вызовов для отслеживания циклов
        HashSet<int> checkedAlr = new();
        HashSet<int> inStack = new(); 
        return FindDep(taskId, checkedAlr, inStack,tasks);
    }
    
    /// <summary>
    /// Метод, ищущий зависимости
    /// </summary>
    /// <param name="taskId">Id задачи, которая проверяется на наличие циклов</param>
    /// <param name="tasks">Список задач</param>
    /// <param name="checkedAlr">Стек Id задач, которые уже проверены</param>
    /// <param name="inStack">Стек для задач, которые проверяются</param>
    /// <returns>True - зависимость циклична, false - в ином случае</returns>
    private  static bool FindDep(int taskId, HashSet<int> checkedAlr, HashSet<int> inStack,List<Tasks> tasks)
    {
        if (inStack.Contains(taskId))
        {
            return true;
        } //найден цикл

        if (checkedAlr.Contains(taskId))
        {
            return false; //уже проверено, цикла нет
        }

        checkedAlr.Add(taskId);
        inStack.Add(taskId);

        foreach (int dependencyId in FindById(taskId,tasks).GetDependencyThisFrom())
        {
            if (FindDep(dependencyId, checkedAlr, inStack, tasks))
            {
                return true;
            }
        }
        inStack.Remove(taskId); //выход из рекурсии
        return false;
    }
    
    /// <summary>
    /// Проверка на наличие противоречащих зависимостей, но не отслеживающий цикличность
    /// </summary>
    /// <param name="tasksDepFrom>Задача, которая должна зависеть от другой</param>
    /// <param name="tasksThisDep>Задача, от которой должно зависеть</param>
    public static void CheckDependencyNotCircled(Tasks tasksDepFrom, Tasks tasksThisDep)
    {
        if (tasksThisDep.GetDependency().Count!= 0)
        {
            for (int i = 0; i < tasksThisDep.GetDependency().Count; i++)
            {
                if (tasksThisDep.GetDependency()[i] == tasksDepFrom.ID)
                {
                    throw new ArgumentException();
                }

                if (tasksThisDep.ID == tasksDepFrom.ID)
                {
                    throw new ArgumentException();
                }
            }
        }

        if (tasksThisDep.Status == "DONE")
        {
            throw new ArgumentException();
        }
    }
}