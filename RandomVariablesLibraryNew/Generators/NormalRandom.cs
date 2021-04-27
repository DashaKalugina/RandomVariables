using System;

namespace RandomVariablesLibraryNew.Generators
{
    public class NormalRandom : Random
    {
        private double prevSample = double.NaN;

        /// <summary>
        /// Используется метод Marsaglia polar для генерации нормально распределенной СВ
        /// </summary>
        /// <returns></returns>
        protected override double Sample()
        {
            // Если есть предыдущее значение, то возвращаем его
            if (!double.IsNaN(prevSample))
            {
                double result = prevSample;
                prevSample = double.NaN;
                return result;
            }

            // Если нет, то вычисляем следующие два значения
            double u, v, sumOfSquares;
            do
            {
                u = 2 * base.Sample() - 1;
                v = 2 * base.Sample() - 1; // [-1, 1)
                sumOfSquares = u * u + v * v;
            }
            while (u <= -1 || v <= -1 || sumOfSquares >= 1 || sumOfSquares == 0);

            var radius = Math.Sqrt(-2.0 * Math.Log(sumOfSquares) / sumOfSquares);

            prevSample = radius * v;
            return radius * u;
        }
    }
}