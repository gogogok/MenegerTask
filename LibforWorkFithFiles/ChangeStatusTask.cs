using MenuLib;

namespace LibWorkWithFiles;

/// <summary>
/// Класс для изменения статуса задачи
/// </summary>
public static class ChangeStatusTask
{
    /// <summary>
    /// Метод для изменения статуса задачи
    /// </summary>
    /// <param name="path">Путь к файлу, где лежат задачи</param>
    /// <param name="tasks">Список задач</param>
    public static void Change(ref string path, List<Task> tasks)
    {
        Console.WriteLine("Введите ID задачи, которую желаете изменить");
        int id;
        while (true)
        {
            try
            {
                id = int.Parse(Console.ReadLine());
                bool isId = false;
                for(int i = 0; i < tasks.Count; i++)
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

        string status = String.Empty;
        ConsoleKeyInfo key;
        do
        {
            //получение статуса от пользователя
            Console.Clear();
            Frame.PrintFrame(Frame.ForPrint(Texts.ChooseStatus));
            key = Console.ReadKey(true);
            switch (key.KeyChar)
            {
                case '1':
                    status = "TODO";
                    break;
                case '2':
                    status = "IN_PROGRESS";
                    break;
                case '3':
                    status = "DONE";
                    break;
            }
        } while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.D3);
        
        for(int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].ID == id)
            {
                tasks[i].Status = status;
                tasks[i].SetUpdatedAt(DateTime.Now);
            }
        }
        WriteToFile.WriteBackToFile(ref path, tasks);
        Console.Clear();
        Console.WriteLine("Статус задачи изменён");
    }
}