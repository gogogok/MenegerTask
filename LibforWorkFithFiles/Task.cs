using System.Text;

namespace LibWorkWithFiles;

public enum Statuses
{
    ToDo,
    InProgress,
    Done
}

public enum Priority
{
    High,
    Medium,
    Low
}

public class Task
{
    /// <summary>
    /// ID задачи
    /// </summary>
    private int _id;

    /// <summary>
    /// Приоритет задачи
    /// </summary>
    private Priority _priority;

    /// <summary>
    /// Статус задачи
    /// </summary>
    private Statuses _status;

    /// <summary>
    /// Описание задачи
    /// </summary>
    private string? _description;
    
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
            if (_priority == LibWorkWithFiles.Priority.Low)
            {
                return "Низкий";
            }

            if (_priority == LibWorkWithFiles.Priority.Medium)
            {
                return "Средний";
            }

            if (_priority == LibWorkWithFiles.Priority.High)
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
                _priority = LibWorkWithFiles.Priority.Low;
            }
            else if (value == "Средний")
            {
                _priority = LibWorkWithFiles.Priority.Medium;
            }
            else if (value == "Высокий")
            {
                _priority = LibWorkWithFiles.Priority.High;
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
    

    /// <summary>
    /// Конструктор для создания задачи в коде
    /// </summary>
    /// <param name="id">ID задачи</param>
    /// <param name="status">Статус задчи</param>
    /// <param name="priority">Приоритет задачи</param>
    /// <param name="description">Описание задачи</param>
    public Task(int id, string status, string priority, string description, DateTime updatedAt)
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
    public Task()
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

       str.AppendLine("--------------------");
       return str.ToString();
    }
}