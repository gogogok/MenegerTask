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
    /// <param name="path">Ссылка на файл, куда будет записана новая задача</param>
    /// <returns>Новую задачу</returns>
    public static Task AddTasks(List<Task> tasks,string path)
    {
        Console.WriteLine("Добавление задачи");
        Console.WriteLine("Введите название задачи:");
        bool notInList = false;
        do
        {
            string? desc = Console.ReadLine();
            if (tasks!= null) //проверка на то, есть ли уже задача с таким описанием
            {
                if (tasks.Count != 0)
                {
                    foreach (Task task in tasks)
                    {
                        if (task.Desc == desc)
                        {
                            notInList = true;
                            break;
                        }
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
                } while (key1.Key != ConsoleKey.D1 & key1.Key != ConsoleKey.D2 & key1.Key != ConsoleKey.D3);
                
                return new Task(UniqueId(tasks), "TODO", priority,desc); //возвращение новой задачи
            }
            else
            {
                Console.WriteLine("Задача с таким названием уже существует.");
            }
        } while (notInList);
        return null;
    }

    /// <summary>
    /// Присваивание новой задаче уникального ID
    /// </summary>
    /// <param name="tasks">Список задач</param>
    /// <returns>Уникальный ID</returns>
    private static int UniqueId(List<Task> tasks)
    {
        int ind1 = 0;
        if (tasks.Count != 0) 
        {
            foreach (Task task in tasks)
            {
                ind1++;
                int ind2 = task.ID;
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
        return ind1+=1;
    }
}