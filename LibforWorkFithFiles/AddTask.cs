using MenuLib;

namespace LibWorkWithFiles;

/// <summary>
/// Класс, отвечающий за добавление задач
/// </summary>
public static class AddTask
{
    /// <summary>
    /// Метод, добавляющий задачи
    /// </summary>
    /// <param name="tasks">Список, к которому будет добавлена задача</param>
    /// <param name="projects">Список проектов, в котором лежит проект, к которому будет добавлена задача</param>
    /// <returns>Новую задачу</returns>
    public static void AddTasks(List<Project> projects, List<Tasks> tasks)
    {
        Console.WriteLine("Добавление задачи");
        Console.WriteLine("Введите название задачи:");
        bool notInList = false;
        do
        {
            string? desc = Console.ReadLine();
            foreach (Project project in projects)
            {
                foreach (Tasks task in project)
                {
                    if (task.Desc == desc)
                    {
                        notInList = true;
                        break; 
                    }
                }
            }
            

            if (!notInList)
            {
                string priority = String.Empty;
                ConsoleKeyInfo key1;
                {
                    //получения от пользователя приоритета задачи
                    Frame.PrintFrame(Frame.ForPrint(Texts.ChoosePriority)); //меню для выбора приоритета
                    key1 = Console.ReadKey(true);
                    switch (key1.KeyChar)
                    {
                        case '1':
                            priority = "Высокий";
                            break;
                        case '2':
                            priority = "Средний";
                            break;
                        case '3':
                            priority = "Низкий";
                            break;
                    }
                } while (key1.Key != ConsoleKey.D1 && key1.Key != ConsoleKey.D2 && key1.Key != ConsoleKey.D3);
                
                Console.WriteLine("Введите название проекта, к которому хотите присоединить задачу");
                string name = MethodsFindAndCheck.CheckProjectName(projects);
                Project? project = MethodsFindAndCheck.FindByName(projects, name);
                tasks = tasks.OrderBy(t => t.Id).ToList();
                Tasks task = new Tasks(UniqueId(tasks), "TODO", priority, desc);
                tasks.Add(task);
                project.AddTaskInProject(task);
            }
            else
            {
                Console.WriteLine("Задача с таким названием уже существует.");
            }
        } while (notInList);
    }

    /// <summary>
    /// Присваивание новой задаче уникального ID
    /// </summary>
    /// <param name="tasks">Список задач</param>
    /// <returns>Уникальный ID</returns>
    private static int UniqueId(List<Tasks> tasks)
    {
        int ind1 = 0;
        if (tasks.Count != 0) 
        {
            foreach (Tasks task in tasks)
            {
                ind1++;
                int ind2 = task.Id;
                if (ind1 != ind2) //если ID пропущен, возвращаем этот ID
                {
                    return ind1;
                }
            }
        }
        else //если список был пустой, присваиваем ID 1
        {
            return 1;
        }
        return ind1 + 1;
    }
}