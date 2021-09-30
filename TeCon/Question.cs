using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace TeCon
{
    public class Question
    {
        public string quest { get; set; }
        /// <summary>
        /// Списков вариантов ответов на вопрос
        /// </summary>
        public VarientsViewModel varients { get; set; }

        /// <summary>
        /// Создание пользователем вопроса
        /// </summary>
        /// <param name="varients">Список созданных вариантов ответа</param>
        public Question(VarientsViewModel varients)
        {
            this.varients = varients;
        }
    }
}
