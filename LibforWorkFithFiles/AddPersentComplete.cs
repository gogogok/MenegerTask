namespace LibWorkWithFiles;

public static class AddPersentComplete
{
    public static void AddPersent(List<Tasks> tasks)
    {
        int percent;
        Console.WriteLine("Введите ID задачи, которой хотите установить процент выполнения");
        int id = int.Parse(Console.ReadLine());
        while (true)
        {
            try
            {
                percent = int.Parse(Console.ReadLine());
                if (percent < 0 || percent > 100)
                {
                    throw new FormatException();
                }
                Tasks task = MethodsFindAndCheck.FindById(id, tasks);
                task.SetPercentComplete(percent);
                task.SetUpdatedAt(DateTime.Now);
            }
            catch (FormatException)
            {
                Console.WriteLine("Данные не могут быть записаны, как процент выполнения. Повторите ввод.");
            }
        }
       
    }
}