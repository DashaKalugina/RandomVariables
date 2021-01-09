using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomVariables
{
    /// <summary>
    /// Базовый класс распределения СВ
    /// </summary>
    public abstract class DistributionBase
    {
        /// <summary>
        /// Функция плотности распределения СВ
        /// </summary>
        public virtual Func<double, double> ProbabilityFunction { get; set; }

        /// <summary>
        /// Функция распределения СВ
        /// </summary>
        public virtual Func<double, double> DistributionFunction { get; set; }

        /// <summary>
        /// Генератор значений СВ
        /// </summary>
        //public abstract IRandomNumberGenerator RandomNumberGenerator { get; }

        public abstract double GetNewRandomValue();

        /// <summary>
        /// Генерирует заданное число значений СВ
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public double[] GenerateRandomVariableValues(int N)
        {
            var randomVariableValues = new List<double>(N);
            for (int i = 0; i < N; i++)
            {
                //Thread.Sleep(10);
                randomVariableValues.Add(GetNewRandomValue());
            }
            randomVariableValues.Sort();
            return randomVariableValues.ToArray();
        }

        //public double[] GenerateRandomVariableValues(int N)
        //{
        //    var randomVariableValues = new double[N];
        //    for (int i=0; i<N; i++)
        //    {
        //        //Thread.Sleep(10);
        //        randomVariableValues[i] = RandomNumberGenerator.Next();
        //    }
        //    return randomVariableValues;
        //}
    }
}
