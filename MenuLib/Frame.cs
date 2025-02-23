namespace MenuLib;

/// <summary>
/// Перечисление всех выводов в рамках
/// </summary>
public enum Texts
{
    ChooseFileExport,
    Description,
    ChoosePoint,
    ChoosePriority,
    ChooseStatus,
    ChooseShow
}

/// <summary>
/// Класс, создающий рамку 
/// </summary>
public static class Frame
{

    /// <summary>
    /// Метод, передающий информацию для рамок
    /// </summary>
    /// <param name="text">Нужный элемент перечисления</param>
    /// <returns>Массив строк для рамки</returns>
    public static string[] ForPrint(Texts text)
    {
        string[] str;
        switch (text)
        {
            case Texts.ChooseFileExport:
                str = ["Выберите формат файла, куда вы хотите записать информацию", " ", "1. Текстовый ", "2. CSV", "3. JSON"];
                return str;
            case Texts.Description:
                str = ["Приветствую, пользователь!", "", "Данное приложение является менеджером задач.", "Задачи можно предоставить в виде CSV, ", "JSON файла или текстовым файлом.","", "Данные о задачах стоит предоставить так:", "[ID задачи] [Статус] [Приоритет] [Описание задачи]","", "Список статусов:", "• TODO","• IN_PROGRESS ","• DONE", "","Список приоритетов: ", "• Низкий" , "• Средний ", "• Высокий ", "", "Нажми любую кнопку для продолжения",""];
                return str;
            case Texts.ChoosePoint:
                str  = ["Меню ", "Выберите пункт меню", " ", "1. Ввести данные через файл", "2. Просмотреть все задачи", "3. Добавить новую задачу ", "4. Изменить статус задачи", "5. Удалить задачу по ID",  "6. Выйти из программы", " "];
                return str;
            case Texts.ChoosePriority:
                str = ["Выберите приоритет", "", "1.Высокий ", "2. Средний", "3.Низкий"," "];
                return str;
            case Texts.ChooseStatus:
                str = ["Выберите статус", " ","1. TODO", "2.IN_PROGRESS", "3. DONE"," "];
                return str;
            case Texts.ChooseShow:
                str = ["Выберите, как вывести данные","","1. В консоль", "2. В виде таблицы "," "];
                return str;
        }
        return null;
    }


    /// <summary>
    /// Вывод рамки в консоль
    /// </summary>
    public static void PrintFrame(string[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            int lenOfStr = data[i].Length;
            int c = CountLength(data) + 4;
            if (i == 0)
            {
                int lenOfPole = (c - lenOfStr) / 2;
                Console.Write("\u2554");
                while (c != 0)
                {
                    Console.Write("\u2550");
                    c--;
                }

                Console.WriteLine("\u2557");

                string prob = "";
                while (lenOfPole != 0)
                {
                    prob += " ";
                    lenOfPole--;
                }

                Console.Write("\u2551");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"{prob}{data[i]}{prob}");
                Console.ResetColor();
                Console.WriteLine("\u2551");
            }
            else if (i == data.Length - 1)
            {
                Console.Write("\u255a");
                while (c != 0)
                {
                    Console.Write("\u2550");
                    c--;
                }

                Console.WriteLine("\u255d");
            }
            else
            {
                int lenOfPole = (c - lenOfStr) / 2;
                string prob = "";
                while (lenOfPole != 0)
                {
                    prob += " ";
                    lenOfPole--;
                }

                if (data[i] != "Выберите пункт меню")
                {
                    Console.WriteLine($"\u2551{prob}{data[i]}{prob}\u2551");
                }
                else //изменение цвета выбора пункта меню в рамке
                {
                    Console.Write("\u2551");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write($"{prob}{data[i]}{prob}");
                    Console.ResetColor();
                    Console.WriteLine("\u2551");
                }
            }
        }

    }

    /// <summary>
    /// Метод для подсчёта самой длинной строки
    /// </summary>
    /// <param name="data">Массив строк, из которых будет выбрана самая длинная</param>
    /// <returns>Максимальную длину строки</returns>
    private static int CountLength(string[] data)
    {
        int maxSize = 0;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].Length > maxSize)
            {
                maxSize = data[i].Length;
            }
        }
        return maxSize;
    }
}