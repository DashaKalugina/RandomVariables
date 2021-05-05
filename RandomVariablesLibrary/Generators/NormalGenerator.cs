namespace RandomVariablesLibrary.Generators
{
    /// <summary>
    /// Генератор нормально распределенной СВ
    /// </summary>
    public static class NormalGenerator
    {
        private static readonly NormalRandom _random = new NormalRandom();

        public static double Next(double mu, double sigma)
        {
            return sigma * _random.NextDouble() + mu;
        }
    }
}
