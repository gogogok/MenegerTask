using MenuLib;
using Spectre.Console;

namespace LibWorkWithFiles;

public static class WorkWithProjects
{
    public static void ProjectsOptions(List<Project> projects)
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
                    RenameProject(projects);
                    break;
                case '3':
                    ChangeTaskLocation(projects);
                    break;
                case '4':
                    ShowStatistics(projects);
                    break;
            }
            
        } while (key.Key != ConsoleKey.D1 & key.Key != ConsoleKey.D2 & key.Key != ConsoleKey.D3 & key.Key != ConsoleKey.D4);
        
    }
    
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

    private static int PercentCompleteProject(Project project)
    {
        int perc = 0;
        foreach (Tasks task in project)
        {
            perc += task.PercentComplete;
        }
        perc /= CountTasksDone(project);
        return perc;
    }

    private static void RenameProject(List<Project> projects)
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
            project1.Name = newName;
            Console.WriteLine("Имя успешно изменено.");
        }
        else
        {
            Console.WriteLine("Ошибка! Проект с таким именем уже существует.");
        }
    }

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

    private static void ChangeTaskLocation(List<Project> projects)
    {
        List<Tasks> tasks = new List<Tasks>();
        foreach (Project project in projects)
        {
            foreach (Tasks task1 in project)
            {
                tasks.Add(task1);
            }
        }
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
    }

    private static void ShowStatistics(List<Project> projects)
    {
        Console.Clear();
        Panel panel = new Panel("  Статистика по проектам  ");
        panel.Header = new PanelHeader(" ");
        foreach (Project project in projects)
        {
            panel.Header = new PanelHeader($" Проект: {project.Name}");
            panel.Header = new PanelHeader($"   Количество задач в проекте: {project.TasksCount()}");
            panel.Header = new PanelHeader($"   Количество сделанных задач в проекте: {CountTasksDone(project)}");
            panel.Header = new PanelHeader($"   Процент сделанных задач в проекте: {PercentCompleteProject(project)}");
            panel.Header = new PanelHeader("");
        }
        panel.Header = new PanelHeader("------------------------------------------------------------");
        panel.Border = BoxBorder.Double;
        AnsiConsole.Write(panel);
    }
}
    
