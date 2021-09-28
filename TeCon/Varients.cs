using System;
using System.Collections.Generic;
using System.Text;

namespace TeCon
{
    public class Varients
    {
        /// <summary>
        /// Один вариант
        /// </summary>
        public string varient { get; set; }

        /// <summary>
        /// Правильный ли это ответ
        /// </summary>
        public bool isTrue { get; set; }


        /// <summary>
        /// Создание пользователем варианта
        /// </summary>
        /// <param name="varient">Созданный вариант</param>
        /// <param name="isTrue">Пользователь отметил этот вариант правильным</param>
        public Varients(string varient, bool isTrue)
        {
            this.varient = varient;
            this.isTrue = isTrue;
        }
    }
}
