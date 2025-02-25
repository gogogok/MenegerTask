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
        int id = MethodsFindAndCheck.CheckId(tasks);

        string status = String.Empty;
        ConsoleKeyInfo key;
        try
        {

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
                        Task task = MethodsFindAndCheck.FindById(id, tasks);
                        //проверка, есть ли незаконченная задача, от которой зависит данная
                        foreach (int idTask in task.GetDependencyThisFrom())
                        {
                            Task taskFromDep = MethodsFindAndCheck.FindById(idTask, tasks);
                            if (taskFromDep.Status != "DONE")
                            {
                                throw new ArgumentException();
                            }
                        }

                        status = "DONE";
                        break;
                }
            } while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.D3);


            for (int i = 0; i < tasks.Count; i++)
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
        catch (ArgumentException)
        {
            Console.WriteLine("Нельзя закончить данную задачу, так как есть неоконченная задача, от которой зависит данная");
        }
    }
}