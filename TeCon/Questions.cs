using System;
using System.Collections.Generic;
using System.Text;

namespace TeCon
{
    public class Questions
    {
        /// <summary>
        /// Список созданных вопросов
        /// </summary>
        public List<Question> questions { get; private set; }

        /// <summary>
        /// Создание теста
        /// </summary>
        /// <param name="questions">Список созданных вопросов</param>
        public Questions(List<Question> questions)
        {
            this.questions = questions;
        }
    }
}
