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
    /// <param name="projects">Список проектов</param>
    /// /// <param name="tasks">Список задач</param>
    public static void Change(ref string path, List<Project> projects,List<Tasks> tasks)
    {
        if (tasks.Count != 0)
        {
            Console.WriteLine("Введите ID задачи, которую желаете изменить");
            int id = MethodsFindAndCheck.CheckId(tasks);
            Tasks task = MethodsFindAndCheck.FindById(id, tasks);
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
                            task.Status = status;
                            task.Updated();
                            task.PercentComplete = 0;

                            break;
                        case '2':
                            status = "IN_PROGRESS";
                            task.Status = status;
                            task.Updated();
                            task.PercentComplete = 50;

                            break;
                        case '3':
                            //проверка, есть ли незаконченная задача, от которой зависит данная
                            foreach (int idTask in task.GetDependencyThisFrom())
                            {
                                Tasks tasksFromDep = MethodsFindAndCheck.FindById(idTask, tasks);
                                if (tasksFromDep.Status != "DONE")
                                {
                                    throw new ArgumentException();
                                }
                            }

                            status = "DONE";
                            task.Status = status;
                            task.Updated();
                            task.PercentComplete = 100;
                            break;
                    }
                } while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.D3);


                WriteToFile.WriteBackToFile(ref path, projects);
                Console.Clear();
                Console.WriteLine("Статус задачи изменён");
            }
            catch (ArgumentException)
            {
                Console.WriteLine(
                    "Нельзя закончить данную задачу, так как есть неоконченная задача, от которой зависит данная");
            }
        }
        else
        {
            Console.WriteLine("Задачи не были найдены");
        }
    }
}