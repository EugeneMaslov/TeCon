using System;
using System.Collections.Generic;
using System.Text;

namespace TeCon
{
    public class Question
    {
        /// <summary>
        /// Списков вариантов ответов на вопрос
        /// </summary>
        public List<Varients> varients { get; private set; }

        /// <summary>
        /// Создание пользователем вопроса
        /// </summary>
        /// <param name="varients">Список созданных вариантов ответа</param>
        public Question(List<Varients> varients)
        {
            this.varients = varients;
        }
    }
}
