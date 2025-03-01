using System.Globalization;
using System.Text;
using CsvHelper.Configuration.Attributes;

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
    
    

    private int _percentComplete;

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

    /// <summary>
    /// Время создания задачи
    /// </summary>
    private DateTime _createdAt = DateTime.Now;



    /// <summary>
    /// Дата обновления
    /// </summary>
    private DateTime _updatedAt;


    [Optional]
    public int PercentComplete
    {
        get => _percentComplete;
        set
        {
            if (value < 0 || value > 100)
            {
                throw new FormatException();
            }
            else
            {
                _percentComplete = value;
            }
        }
    }

    public DateTime GetDeadLine()
    {
        return _deadLine;
    }
    
    [Optional]
    public string DeadLine
    {
        set
        {
            if (value != "-"| string.IsNullOrWhiteSpace(value))
            {
                if (DateTime.TryParseExact(value, "dd-MM-yy HH:mm", null, DateTimeStyles.None, out DateTime parsedDate))
                {
                    if (DateTime.Now <= parsedDate)
                    {
                        _deadLine = parsedDate;
                    }
                    else
                    {
                        throw new FormatException("Дата дедлайна раньше сегодняшней");
                    }
                }
                else
                {
                    throw new FormatException("Неверный формат данных");
                }
            }
            else
            {
                _deadLine = default;
            }
        }
    }

    public DateTime GetUpdatedAt()
    {
        return _updatedAt;
    }
    
    [Optional]
    public string Updated
    {
        set
        {
            if (value != "-" | string.IsNullOrWhiteSpace(value))
            {
                _ = DateTime.TryParseExact(value, "dd-MM-yy HH:mm", null, DateTimeStyles.None, out DateTime parsedDate);
                if (parsedDate != default)
                {
                    if (_createdAt <= parsedDate)
                    {
                        _updatedAt = parsedDate;
                    }
                    else
                    {
                        _updatedAt = DateTime.Now;
                    }
                }
                else
                {
                    _updatedAt = DateTime.Now;
                }
            }
            else
            {
                _updatedAt = DateTime.Now;
            }
        }
    }


    /// <summary>
    /// Метод, добавляющий задачи, зависимые от этой
    /// </summary>
    /// <param name="dependencyId"></param>
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
    
    public void DeleteDeadLine()
    {
        _deadLine = default;
    }
    
    /// <summary>
    /// Доступ к дате создания
    /// </summary>
    /// <returns>Дату создания</returns>
    public DateTime GetCreatedAt()
    {
     return _createdAt;
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
        _updatedAt = updatedAt;
        if (Status == "DONE")
        {
            _percentComplete = 100;
        }
        else if (Status == "IN_PROGRESS")
        {
            _percentComplete =50;
        }
        else if (Status == "TODO")
        {
            _percentComplete = 0;
        }
    }
    

    /// <summary>
    /// Конструктор без параметров для корректной работы деселиризаторов
    /// </summary>
    public Tasks()
    {
        _updatedAt = DateTime.Now;
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
       str.AppendLine($"Дата и время создания: {_createdAt}");
       if (_createdAt.Second != _updatedAt.Second)
       {
           str.AppendLine($"Дата последнего редактирования: {_updatedAt}");
       }
       if (_deadLine != default)
       {
           str.AppendLine($"Дедлайн: {_deadLine.ToString("dd-MM-yy HH:mm")}");
           if(DateTime.Now > _deadLine)
           {
               if ((DateTime.Today.Year == GetDeadLine().Year &  DateTime.Today.Month == GetDeadLine().Month &  DateTime.Today.Day == GetDeadLine().Day &  DateTime.Today.Hour == GetDeadLine().Hour &  DateTime.Today.Minute - GetDeadLine().Minute < 59))
               {
                   str.AppendLine("До конца дедлайна меньше часа!");
               }
           }
       }
       str.AppendLine(ProgressBar.Progress(_percentComplete));
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

