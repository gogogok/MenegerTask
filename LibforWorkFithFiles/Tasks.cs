using System.Text;

namespace LibWorkWithFiles;

public enum Statuses
{
    ToDo,
    InProgress,
    Done
}

public enum Prioritys
{
    High,
    Medium,
    Low
}

public class Tasks
{
    /// <summary>
    /// ID задачи
    /// </summary>
    private int _id;

    /// <summary>
    /// Приоритет задачи
    /// </summary>
    private Prioritys _priority;

    /// <summary>
    /// Статус задачи
    /// </summary>
    private Statuses _status;

    /// <summary>
    /// Описание задачи
    /// </summary>
    private string? _description;
    
    /// <summary>
    /// ID задач, зависимых от этой задачи
    /// </summary>
    private List<int> _dependenciesIdFromThis = new List<int>();
    
    /// <summary>
    /// ID задач, от которых зависит эта задача
    /// </summary>
    private List<int> _dependenciesIdThisDep = new List<int>();
    
    /// <summary>
    /// Деделайн задачи
    /// </summary>
    private DateTime _deadLine = default;
    
    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    /// <summary>
    /// Аксесcор к _status
    /// </summary>
    public string Status
    {
        get
        {
            if (_status == Statuses.ToDo)
            {
                return "TODO";
            }

            if (_status == Statuses.InProgress)
            {
                return "IN_PROGRESS";
            }

            if (_status == Statuses.Done)
            {
                return "DONE";
            }

            return "";
        }
        set
        {
            //выбрасывается исключение, если такого статуса не существует
            if (value == "TODO")
            {
                _status = Statuses.ToDo;
            }
            else if (value == "IN_PROGRESS")
            {
                _status = Statuses.InProgress;
            }
            else if (value == "DONE")
            {
                _status = Statuses.Done;
            }
            else
            {
                throw new FormatException();
            }
        }
    }

    /// <summary>
    /// Аксесcор к _priority
    /// </summary>
    public string Priority
    {
        get
        {
            if (_priority == Prioritys.Low)
            {
                return "Низкий";
            }

            if (_priority == Prioritys.Medium)
            {
                return "Средний";
            }

            if (_priority == Prioritys.High)
            {
                return "Высокий";
            }

            return "";
        }
        set
        {
            //выбрасывается исключение, если такого приоритета не существует
            if (value == "Низкий")
            {
                _priority = Prioritys.Low;
            }
            else if (value == "Средний")
            {
                _priority = Prioritys.Medium;
            }
            else if (value == "Высокий")
            {
                _priority = Prioritys.High;
            }
            else
            {
                throw new FormatException();
            }
        }
    }

    /// <summary>
    /// Аксесcор к _description
    /// </summary>
    public string Desc
    {
        get => _description;
        set
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                _description = value;
            }
            else
            {
                throw new FormatException();
            }
        }
    }

    private DateTime _createdAt = DateTime.Now;
    public DateTime GetCreatedAt()
    {
        return _createdAt;
    }

    private DateTime _updatedAt;

    public DateTime GetUpdatedAt()
    {
        return _updatedAt;
    }

    public void SetUpdatedAt(DateTime dateTime)
    {
        _updatedAt = dateTime;
    }

    public void AddDependency(int dependencyId)
    {
       _dependenciesIdFromThis.Add(dependencyId);
    }

    /// <summary>
    /// Удаление задачи, зависящей от данной
    /// </summary>
    /// <param name="dependencyId">ID задачи, которая зависит от данной</param>
    public void DeleteDependencyFromThis(int dependencyId)
    {
        _dependenciesIdFromThis.Remove(dependencyId);
    }
    
    /// <summary>
    /// Удаление задачи, от которой зависит данная
    /// </summary>
    /// <param name="dependencyId">ID задачи, от которой зависит данная</param>
    public void DeleteDependencyThisFrom(int dependencyId)
    {
        _dependenciesIdThisDep.Remove(dependencyId);
    }
    /// <summary>
    /// ID задач, зависящих от этой задачи
    /// </summary>
    /// <returns>Список ID</returns>
    public List<int> GetDependency()
    {
        return _dependenciesIdFromThis;
    }
    /// <summary>
    /// ID задач, от которых зависит эта задача
    /// </summary>
    /// <returns>Список ID</returns>
    public List<int> GetDependencyThisFrom()
    {
        return _dependenciesIdThisDep;
    }

    /// <summary>
    /// Добавление задач, от которых зависит данная
    /// </summary>
    /// <param name="taskID">Задача, от которой зависит данная</param>
    public void SetDependenciesIdThisFrom(int taskID)
    {
        _dependenciesIdThisDep.Add(taskID);
    }

    /// <summary>
    /// Добавление дедлайна.
    /// </summary>
    /// <param name="deadLine">Дата, которую нужно установить в качестве дедлайнс</param>
    /// <exception cref="FormatException">Исключение, если дата дедлайна раньше сегодняшней</exception>
    public void SetDeadLine(DateTime deadLine)
    {
        if (deadLine > DateTime.Now)
        {
            _deadLine = deadLine;
        }
        else
        {
            throw new FormatException("Дата дедлайна раньше сегодняшней");
        }
    }
    public DateTime GetDeadLine()
    {
        return _deadLine;
    }
    public void DeleteDeadLine()
    {
        _deadLine = default;
    }

    /// <summary>
    /// Конструктор для создания задачи в коде
    /// </summary>
    /// <param name="id">ID задачи</param>
    /// <param name="status">Статус задачи</param>
    /// <param name="priority">Приоритет задачи</param>
    /// <param name="description">Описание задачи</param>
    public Tasks(int id, string status, string priority, string description, DateTime updatedAt)
    {
        ID = id;
        Priority = priority;
        Status = status;
        Desc = description;
        SetUpdatedAt(updatedAt);
    }
    

    /// <summary>
    /// Конструктор без параметров для корректной работы деселиризаторов
    /// </summary>
    public Tasks()
    {
        SetUpdatedAt(DateTime.Now);
    }

    /// <summary>
    /// Переопределение ToString для вывода в консоль
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
       StringBuilder str = new();
       str.AppendLine($"ID: {ID}");
       str.AppendLine($"Статус: {Status}");
       str.AppendLine($"Приоритет: {Priority}");
       str.AppendLine($"Описание: {Desc}");
       str.AppendLine($"Дата и время создания: {GetCreatedAt()}");
       if (GetCreatedAt().Second != GetUpdatedAt().Second)
       {
           str.AppendLine($"Дата последнего редактирования: {GetUpdatedAt()}");
       }
       if (GetDeadLine() != default)
       {
           str.AppendLine($"Дедлайн: {GetDeadLine()}");
       }
       str.AppendLine("--------------------");
       return str.ToString();
    }

    public int PriorytiImportance(string pr)
    {
        if(pr =="Высокий") return (int)Prioritys.High;
        if(pr == "Средний") return (int)Prioritys.Medium;
        if(pr == "Низкий") return (int)Prioritys.Low;
        return 4;
    }
}