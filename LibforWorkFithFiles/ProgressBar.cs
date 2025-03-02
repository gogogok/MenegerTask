using System.Text;

namespace LibWorkWithFiles
{

    /// <summary>
    /// Класс, создающий progress bar
    /// </summary>
    public class ProgressBar
    {
        /// <summary>
        /// Метод, создающий строку Progress Bar
        /// </summary>
        /// <param name="perc">Процент выполнения</param>
        /// <returns>Строку прогресса</returns>
        public static string Progress(int perc)
        {
            int scale = perc / 4;
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= scale; i++)
            {
                sb.Append('█');
            }

            for (int i = scale + 1; i <= 25; i++)
            {
                sb.Append('░');
            }

            sb.Append($" {perc}%");
            return sb.ToString();
        }
    }
}