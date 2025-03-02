using System.Collections;

namespace LibWorkWithFiles
{

    /// <summary>
    /// Класс, описывающий проекты пользователя
    /// </summary>
    public class Project : IEnumerable
    {
        /// <summary>
        /// Название проекта
        /// </summary>
        private string _name;

        /// <summary>
        /// Аксессор к названию
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Список задач в проекте
        /// </summary>
        private List<Tasks> _tasksInProject = new List<Tasks>();

        /// <summary>
        /// Аксессор к списку задач
        /// </summary>
        public List<Tasks> Tasks => _tasksInProject;

        /// <summary>
        /// Метод добавления задач в проект
        /// </summary>
        /// <param name="task">Задача, которую нужно добавить</param>
        public void AddTaskInProject(Tasks task)
        {
            _tasksInProject.Add(task);
        }

        /// <summary>
        /// Проверка, есть ли задача в проекте
        /// </summary>
        /// <param name="task">Задача, которую нужно проверить</param>
        /// <returns> True - есть, false - нет</returns>
        public bool IsInProject(Tasks task)
        {
            if (_tasksInProject.Contains(task))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Метод, убирающий задачу из проекта
        /// </summary>
        /// <param name="task">Задача, которую нужно убрать</param>
        public void RemoveFromPr(Tasks task)
        {
            _tasksInProject.Remove(task);
        }

        /// <summary>
        /// Метод, считающий количество задач в проекте
        /// </summary>
        /// <returns></returns>
        public int TasksCount()
        {
            return _tasksInProject.Count;
        }

        /// <summary>
        /// Для перечисления задач в проекте
        /// </summary>
        /// <returns>IEnumerator, перечисляющий задачи в проекте</returns>
        public IEnumerator GetEnumerator()
        {
            foreach (Tasks task in _tasksInProject)
            {
                yield return task;
            }
        }

        /// <summary>
        /// Конструктор для проекта
        /// </summary>
        /// <param name="name">Имя проекта</param>
        public Project(string name)
        {
            _name = name;
        }

    }
}