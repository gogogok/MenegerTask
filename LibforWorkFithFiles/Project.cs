using System.Collections;

namespace LibWorkWithFiles;

/// <summary>
/// Класс, описывающий проекты пользователя
/// </summary>
public class Project : IEnumerable
{
    private string _name;

    public string Name
    {
        get => _name;
        set => _name = value;
    }
    
    private List<Tasks> _tasksInProject = new List<Tasks>();

    public void AddTaskInProject(Tasks task)
    {
        _tasksInProject.Add(task);
    }

    public bool IsInProject(Tasks task)
    {
        if (_tasksInProject.Contains(task)) return true;
        return false;
    }

    public void RemoveFromPr(Tasks task)
    {
        _tasksInProject.Remove(task);
    }
    
    public int TasksCount() => _tasksInProject.Count;
    public IEnumerator GetEnumerator()
    {
        foreach (Tasks task in _tasksInProject)
        {
            yield return task;
        }
    }

    public Project(string name)
    {
        _name = name;
    }
    
}