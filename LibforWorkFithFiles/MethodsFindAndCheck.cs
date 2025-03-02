namespace LibWorkWithFiles
{

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
                        if (tasks[i].Id == id)
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
                if (task.Id == id)
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
        public static bool IsCircled(int taskId, List<Tasks> tasks)
        {
            //стек вызовов для отслеживания циклов
            HashSet<int> checkedAlr = new();
            HashSet<int> inStack = new();
            return FindDep(taskId, checkedAlr, inStack, tasks);
        }

        /// <summary>
        /// Метод, ищущий зависимости
        /// </summary>
        /// <param name="taskId">Id задачи, которая проверяется на наличие циклов</param>
        /// <param name="tasks">Список задач</param>
        /// <param name="checkedAlr">Стек Id задач, которые уже проверены</param>
        /// <param name="inStack">Стек для задач, которые проверяются</param>
        /// <returns>True - зависимость циклична, false - в ином случае</returns>
        private static bool FindDep(int taskId, HashSet<int> checkedAlr, HashSet<int> inStack, List<Tasks> tasks)
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

            if (FindById(taskId, tasks) == null)
            {
                return false;
            }
            foreach (int dependencyId in FindById(taskId, tasks).GetDependency())
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
        /// Проверка на наличие зависимости, где предыдущая задача уже сделана
        /// </summary>
        /// <param name="tasksThisDep">Задача, от которой должно зависеть</param>
        public static void CheckDependencyNotCircled(Tasks tasksThisDep)
        {
            if (tasksThisDep.Status == "DONE")
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Метод для проверки существования имени проекта
        /// </summary>
        /// <param name="projects">Список проектов</param>
        /// <returns>Имя проекта, если такое есть</returns>
        public static string CheckProjectName(List<Project> projects)
        {
            string projectName;
            while (true)
            {
                try
                {
                    projectName = Console.ReadLine();
                    bool isName = false;
                    for (int i = 0; i < projects.Count; i++)
                    {
                        if (projects[i].Name == projectName)
                        {
                            isName = true;
                        }
                    }

                    if (!isName)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Название проекта не было введено. Повторите ввод");
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Проекта с таким названием не существует. Повторите ввод");
                }
            }

            return projectName;
        }

        /// <summary>
        /// Нахождение проекта по названию
        /// </summary>
        /// <param name="name">Название проекта</param>
        /// <param name="projects">Список проектов</param>
        /// <returns></returns>
        public static Project? FindByName(List<Project> projects, string name)
        {
            foreach (Project project in projects)
            {
                if (project.Name == name)
                {
                    return project;
                }
            }

            return null;
        }
    }
}