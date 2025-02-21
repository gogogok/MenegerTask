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
    private int _id;
    private Priority _priority;
    private Statuses _status;
    private string? _description;

    public int ID
    {
        get
        {
            return _id;
        }
        set
        {
            try
            {
               _id = value;
            }
            catch(FormatException)
            {
                
            }
        }
    }
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
        }
    }
    
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
        }
    }

    public string Desc
    {
        get => _description;
        set
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                _description = value;
            }
        }
    }

    public Task(int id, string priority, string status, string description)
    {
        ID = id;
        Priority = priority;
        Status = status;
        Desc = description;
    }

    public Task()
    {
        
    }
    
    public override string ToString() => $"[{ID}] [{Priority}] [{Status}] {Desc}";
}