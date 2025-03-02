namespace LibWorkWithFiles
{

    /// <summary>
    /// Класс для добавления процента 
    /// </summary>
    public static class AddPersentComplete
    {
        /// <summary>
        /// Метод для задания процента выполнения
        /// </summary>
        /// <param name="tasks">Список задач</param>
        /// <param name="projects">Список проектов</param>
        public static void AddPersent(List<Tasks> tasks,List<Project> projects)
        {
            int percent;
            Console.WriteLine("Введите ID задачи, которой хотите установить процент выполнения");
            int id = MethodsFindAndCheck.CheckId(tasks);
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите новый процент выполнения");
                    percent = int.Parse(Console.ReadLine());
                    if (percent < 0 || percent > 100)
                    {
                        throw new FormatException();
                    }

                    Tasks task = MethodsFindAndCheck.FindById(id, tasks);
                    task.PercentComplete = percent;
                    foreach (Project project in projects)
                    {
                        foreach (Tasks task1 in project)
                        {
                            if (task1.Id == task.Id)
                            {
                                task1.PercentComplete = percent;
                                break;
                            }
                        }
                    }
                    task.Updated();
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Данные не могут быть записаны, как процент выполнения. Повторите ввод.");
                }
            }

        }
    }
}