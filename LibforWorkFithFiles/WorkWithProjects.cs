using MenuLib;
using Spectre.Console;

namespace LibWorkWithFiles;

/// <summary>
/// Класс методов для работы с проектами
/// </summary>
public static class WorkWithProjects
{
    /// <summary>
    /// Выбор действия с проектами
    /// </summary>
    /// <param name="projects">Список проектов</param>
    /// <param name="tasks">Список задач</param>
    public static void ProjectsOptions(List<Project> projects,List<Tasks> tasks,ref string path)
    {
        Frame.PrintFrame(Frame.ForPrint(Texts.WorkWithProjects));
        ConsoleKeyInfo key = Console.ReadKey(true);
        do
        {
            switch (key.KeyChar)
            {
                case '1':
                    AddProject(projects);
                    break;
                case '2':
                    RenameProject(projects,ref path);
                    break;
                case '3':
                    ChangeTaskLocation(projects, tasks);
                    break;
                case '4':
                    DeleteProject(projects, tasks, ref path);
                    break;
                case '5':
                    ShowStatistics(projects);
                    break;
            }
        } while (key.Key != ConsoleKey.D1 & key.Key != ConsoleKey.D2 & key.Key != ConsoleKey.D3 & key.Key != ConsoleKey.D4 & key.Key != ConsoleKey.D5);
        
    }
    
    /// <summary>
    /// Метод, считающий сколько задач в проекте сделано
    /// </summary>
    /// <param name="project">Проект, в котором нужно посчитать задачи</param>
    /// <returns>Количество сделанных задач в проекте</returns>
    private static int CountTasksDone(Project project)
    {
        int countDone = 0;
        foreach (Tasks task in project)
        {
            if (task.Status == "DONE")
            {
                countDone++;
            }
        }
        return countDone;
    }

    /// <summary>
    /// Метод, удаляющий проект и задачи в нём
    /// </summary>
    /// <param name="projects">Список проектов</param>
    /// <param name="tasks">Список задач</param>
    /// <param name="path">Путь к файлу, в который будут записаны изменения</param>
    private static void DeleteProject(List<Project> projects, List<Tasks> tasks,ref string path)
    {
        Console.Clear();
        Console.WriteLine("Введите название проекта, который хотите удалить");
        string name = MethodsFindAndCheck.CheckProjectName(projects);
        Project? project = MethodsFindAndCheck.FindByName(projects, name);
        foreach (Tasks task in project)
        {
            tasks.Remove(task);
        }
        projects.Remove(project);
        Console.WriteLine("Проект успешно удалён.");
        WriteToFile.WriteBackToFile(ref path,projects);
    }
    
    /// <summary>
    /// Метод, считающий процент выполнения задач
    /// </summary>
    /// <param name="project">Список проектов</param>
    /// <returns>Процент выполнения проекта</returns>
    private static int PercentCompleteProject(Project project)
    {
        int perc = 0;
        int c = 0;
        foreach (Tasks task in project)
        {
            perc += task.PercentComplete;
            c++;
        }
        if (c != 0)
        {
            perc /= c;
        }
        return perc;
    }

    /// <summary>
    /// Метод, переименовывающий проект
    /// </summary>
    /// <param name="projects">Список проектов</param>
    /// <param name="path">Путь к файлу, куда будет перезаписана информация</param>
    private static void RenameProject(List<Project> projects,ref string path)
    {
        Console.Clear();
        Console.WriteLine("Введите проект, имя которого хотите изменить");
        string firstName = MethodsFindAndCheck.CheckProjectName(projects);
        Project? project1 = MethodsFindAndCheck.FindByName(projects,firstName);
        Console.WriteLine();
        Console.WriteLine("Введите новое имя");
        string newName = Console.ReadLine();
        if (MethodsFindAndCheck.FindByName(projects, newName) == null)
        {
            foreach (Tasks tasks in project1)
            {
                tasks.InProject = newName;
            }
            project1.Name = newName;
            Console.WriteLine("Имя успешно изменено.");
            WriteToFile.WriteBackToFile(ref path,projects);
        }
        else
        {
            Console.WriteLine("Ошибка! Проект с таким именем уже существует.");
        }
    }

    /// <summary>
    /// Метод, добавляющий новый проект
    /// </summary>
    /// <param name="projects">Список проектов</param>
    private static void AddProject(List<Project> projects)
    {
        Console.Clear();
        Console.WriteLine("Введите имя нового проекта");
        string name = Console.ReadLine();
        if (MethodsFindAndCheck.FindByName(projects, name) == null)
        {
            Project newProject = new Project(name);
            projects.Add(newProject);
        }
        else
        {
            Console.WriteLine("Ошибка! Проект с таким именем уже существует.");
        }
    }

    /// <summary>
    /// Метод, меняющий принадлежность задачи к проекту
    /// </summary>
    /// <param name="projects">Список проектов</param>
    /// <param name="tasks">Список задач</param>
    private static void ChangeTaskLocation(List<Project> projects, List<Tasks> tasks)
    {
        Console.Clear();
        Console.WriteLine("Введите Id задачи, которую желаете перенести в другой проект.");
        int id = MethodsFindAndCheck.CheckId(tasks);
        Tasks task = MethodsFindAndCheck.FindById(id, tasks);
        Console.WriteLine("Введите название проекта, в который хотите перенести задачу.");
        string name = MethodsFindAndCheck.CheckProjectName(projects);
        Project? project1 = MethodsFindAndCheck.FindByName(projects, name);
        foreach (Project project in projects)
        {
            if (project.IsInProject(task))
            {
                project.RemoveFromPr(task);
                break;
            }
        }
        project1?.AddTaskInProject(task);
        task.InProject = project1.Name;
    }

    /// <summary>
    /// Метод, выводящий статистику по проектам
    /// </summary>
    /// <param name="projects">Список проектов</param>
    private static void ShowStatistics(List<Project> projects)
    {
        Console.Clear();
        Table table = new Table();
        table.AddColumn("Проект").Centered();
        table.AddColumn(" Количество задач в проекте").Centered();
        table.AddColumn("  Количество сделанных задач в проекте").Centered();
        table.AddColumn(" Процент выполнения проекта").Centered();
        foreach (Project project in projects)
        {
            table.AddRow(project.Name, project.TasksCount().ToString(),CountTasksDone(project).ToString(),PercentCompleteProject(project).ToString());
        }
        AnsiConsole.Write(table);
        
    }
}
    
