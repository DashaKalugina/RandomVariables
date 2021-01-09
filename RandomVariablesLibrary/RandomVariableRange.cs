using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    /// <summary>
    /// Класс, задающий параметры изменения значений СВ
    /// </summary>
    public class RandomVariableRange
    {
        /// <summary>
        /// Начальное значение
        /// </summary>
        public double From { get; set; }

        /// <summary>
        /// Конечное значение
        /// </summary>
        public double To { get; set; }

        /// <summary>
        /// Шаг изменения
        /// </summary>
        public double H { get; set; }
    }
}
