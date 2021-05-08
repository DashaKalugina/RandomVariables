using RandomVariablesLibrary.Distributions.Base;
using RandomVariablesLibrary.Distributions.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using RandomVariables.WebApplication.Models;

namespace RandomVariables.WebApplication
{
    public static class FormulaRecognitionService
    {
        private static List<string> _standardDistributions = DistributionNames.FullDistrNamesByShortNames.Keys.ToList();

		public static Distribution GetResultFormula(string formula)
        {
			formula = formula.Replace('.', ',');
            var formulaPPN = ConvertFormulaToPPN(formula);
			return CalculateFormula(formulaPPN);
        }

        private static string ConvertFormulaToPPN(string formulaString)
        {
			if (formulaString.Length == 0)
				throw new Exception("Formula is invalid");

			var indexOfDistr = IndexOfStandartDistribution(formulaString, 0);
			//if ((!IsDigit(formulaString[0]) || !IsNegativeNumber(formulaString, 0)) && indexOfDistr == -1 && formulaString[0] != '(')
			//	throw new Exception("Syntax error");

			if (!IsDigit(formulaString[0]) && !IsNegativeNumber(formulaString, 0) && indexOfDistr == -1 && formulaString[0] != '(')
				throw new Exception("Syntax error");

			var formulaSymbolIndex = 0;
			var operationStack = new Stack<char>();
			var resultString = string.Empty;
			var diffOpenAndCloseBrackets = 0;
			var wasOperation = false;
			while (formulaSymbolIndex != formulaString.Length)
			{
				string formulaElement = GetFormulaElement(formulaString, formulaSymbolIndex);
				if (formulaElement.Length > 1 || IsDigit(formulaElement[0]))
				{
					resultString += formulaElement + " ";
					wasOperation = false;
					formulaSymbolIndex += formulaElement.Length;
					continue;
				}
				else
				{
					char operation = formulaElement[0];
					switch (operation)
					{
						case '(':
							operationStack.Push(operation);
							diffOpenAndCloseBrackets++;
							wasOperation = false;
							break;
						case '*':
						case '/':
						case '+':
						case '-':
							if (formulaSymbolIndex == formulaString.Length)
                            {
								throw new Exception("Syntax error");
							}

							if (wasOperation == false)
							{
								wasOperation = true;
								if (operationStack.Count() == 0)
								{
									operationStack.Push(operation);
									break;
								}
								while (!(operationStack.Count() == 0) && GetOperationPriority(operation) <= GetOperationPriority(operationStack.Peek()))
								{
									resultString += operationStack.Pop() + ' ';
								}
								if ((!(operationStack.Count() == 0) && GetOperationPriority(operation) > GetOperationPriority(operationStack.Peek())) || (operationStack.Count() == 0))
								{
									operationStack.Push(operation);
								}
								break;
							}
							else
                            {
								throw new Exception("Syntax error");
							}

						case ')':
							if (wasOperation == true)
                            {
								throw new Exception("Syntax error");
							}
							else
							{
								while (!(operationStack.Count() == 0) && (operation = operationStack.Peek()) != '(' && diffOpenAndCloseBrackets > 0)
								{
									resultString += operation + ' ';
									operationStack.Pop();
								}
								if (operationStack.Count() != 0 && operationStack.Peek() == '(')
									operationStack.Pop();
							}
							diffOpenAndCloseBrackets--;
							break;
						default:
							{
								throw new Exception("Error: invalid symbol in the string");
							}
					}
				}
				formulaSymbolIndex++;
			}

			while (!(operationStack.Count() == 0))
			{
				resultString += operationStack.Pop();
			}
			if (diffOpenAndCloseBrackets > 1)
            {
				throw new Exception("Error: wrong number of brackets");
			}

			return resultString;
		}

		private static bool IsDigit(char s) => (s >= '0' && s <= '9');

		private static bool IsNegativeNumber(string formula, int numberIndex)
        {
			return formula[numberIndex] == '-' 
				&& numberIndex + 1 != formula.Length 
				&& IsDigit(formula[numberIndex + 1]);
		}

		private static bool IsSeporatorNotWholePart(char symbol) => symbol == '.' || symbol == ',';

		private static int IndexOfStandartDistribution(string formula, int formulaIndex)
        {
			var result = _standardDistributions.Select((distrName, index) => (distrName, index))
				.Select(x => (x.distrName, x.index, formula.IndexOf(x.distrName, formulaIndex, StringComparison.InvariantCultureIgnoreCase) == formulaIndex))
				.FirstOrDefault(x => x.Item3);

			return string.IsNullOrEmpty(result.distrName) ? -1 : result.index;
		}

		private static string GetFormulaElement(string formula, int startIndex)
        {
			var formulaElement = string.Empty;
			// Если у числа стоит минус, т.е. число отрицательное, то сохраним его.
			if (IsNegativeNumber(formula, startIndex))
			{
				formulaElement += "-";
				startIndex++;
			}
			var symbol = formula[startIndex];
			// Заполняем символами, если это число.
			while (IsDigit(symbol) || IsSeporatorNotWholePart(symbol))
			{
				formulaElement += symbol;
				startIndex++;
				if (startIndex == formula.Length)
                {
					break;
                }
				symbol = formula[startIndex];
			}

			// Если это не число, то проверим, может это распределение.
			if(formulaElement.Length == 0)
            {
				var indexOfDistr = IndexOfStandartDistribution(formula, startIndex);
                if (indexOfDistr != -1)
                {
					formulaElement = _standardDistributions[indexOfDistr];
					startIndex += formulaElement.Length - 1;
					while (formula[startIndex] != ')')
					{
						startIndex++;
						formulaElement += formula[startIndex];
					}
				}
			}

			// Если это не число и не распределение, то это должна быть операция.
			if (formulaElement.Length == 0)
			{
				formulaElement += symbol;
			}

			return formulaElement;
		}

		private static int GetOperationPriority(char c)
		{
			switch (c)
			{
				case '(': return 1;
				case '+': case '-': return 2;
				case '*': case '/': return 3;
				default: return 0;
			}
		}

		private static Distribution CalculateFormula(string formulaPPN)
        {
			var distributionsStack = new Stack<(Distribution distribution, double? number)>();
			for (var i = 0; i < formulaPPN.Length; i++)
			{
				if (formulaPPN[i] == ' ')
					continue;
				var isNegativeNumber = IsNegativeNumber(formulaPPN, i);
				var indexOfDistr = IndexOfStandartDistribution(formulaPPN, i);
				if (indexOfDistr != -1)
				{
					var distrName = _standardDistributions[indexOfDistr];
                    var startIndex = i;
                    while (formulaPPN[i] != ')')
                    {
                        i++;
                    }
					var distrStartIndex = startIndex + distrName.Length + 1;
					var distrParameters = formulaPPN.Substring(distrStartIndex, i - distrStartIndex);
                    var distribution = GetDistributionByName(distrName, distrParameters);
					distributionsStack.Push((distribution, null));
				}
				else if (IsDigit(formulaPPN[i]) || isNegativeNumber)
				{
                    if (isNegativeNumber)
                    {
						i++;
                    }

					int startIndexDigit = i;

					while (i != formulaPPN.Length && (IsDigit(formulaPPN[i]) || IsSeporatorNotWholePart(formulaPPN[i])))
					{
						i++;
					}

					var number = double.Parse(formulaPPN.Substring(startIndexDigit, i - startIndexDigit));
					number = isNegativeNumber ? -number : number;
					distributionsStack.Push((null, number));
					i--;
				} 
				else
				{
					char operation = formulaPPN[i];
					if (distributionsStack.Count() < 2)
					{
						throw new Exception("Формула имеет некорректный формат");
					}
					
					var secondElement = distributionsStack.Pop();
					var firstElement = distributionsStack.Pop();
					var operationResult = GetOperationResult(firstElement, secondElement, operation);
					distributionsStack.Push((operationResult, null));
				}
			}

			if (distributionsStack.Count() != 1)
            {
				throw new Exception("Формула имеет некорректный формат");
			}

			return distributionsStack.Pop().distribution;
		}

		private static Distribution GetDistributionByName(string distrName, string distrParams)
        {
			var parameters = !distrParams.Contains(';')
				? new double[] { double.Parse(distrParams) }
				: distrParams.Split(';').Select(x => double.Parse(x)).ToArray();

			if (!DistributionNames.FullDistrNamesByShortNames.TryGetValue(distrName, out var fullDistrName))
            {
				throw new Exception($"Не найдено распределение с именем {distrName}");
			}

			return fullDistrName switch
			{
				nameof(UniformDistribution) => new UniformDistribution(parameters[0], parameters[1]),
				nameof(NormalDistribution) => new NormalDistribution(parameters[0], parameters[1]),
				nameof(ExponentialDistribution) => new ExponentialDistribution(parameters[0]),
				nameof(ChiSquareDistribution) => new ChiSquareDistribution(parameters[0]),
				nameof(GammaDistribution) => new GammaDistribution(parameters[0], parameters[1]),
				nameof(CauchyDistribution) => new CauchyDistribution(parameters[0], parameters[1]),
				nameof(FDistribution) => new FDistribution(parameters[0], parameters[1]),
				nameof(WeibullDistribution) => new WeibullDistribution(parameters[0], parameters[1]),
				_ => null
			};
        }

		private static Distribution GetOperationResult((Distribution distribution, double? number) firstElement,
			(Distribution distribution, double? number) secondElement, char operation)
        {
			var isFirstDistribution = firstElement.distribution != null;
			var isSecondDistribution = secondElement.distribution != null;
            switch (operation)
            {
				case '+':
                    {
						if(isFirstDistribution && isSecondDistribution)
                        {
							return firstElement.distribution + secondElement.distribution;
                        }

						if(!isFirstDistribution)
                        {
							return firstElement.number.Value + secondElement.distribution;
						}

						return firstElement.distribution + secondElement.number.Value;
					}
				case '-':
					{
						if (isFirstDistribution && isSecondDistribution)
						{
							return firstElement.distribution - secondElement.distribution;
						}

						if (!isFirstDistribution)
						{
							return firstElement.number.Value - secondElement.distribution;
						}

						return firstElement.distribution - secondElement.number.Value;
					}
				case '*':
					{
						if (isFirstDistribution && isSecondDistribution)
						{
							return firstElement.distribution * secondElement.distribution;
						}

						if (!isFirstDistribution)
						{
							return firstElement.number.Value * secondElement.distribution;
						}

						return firstElement.distribution * secondElement.number.Value;
					}
				case '/':
					{
						if (isFirstDistribution && isSecondDistribution)
						{
							return firstElement.distribution / secondElement.distribution;
						}

						return firstElement.distribution / secondElement.number.Value;
					}

				default:
					throw new Exception("Некорректная операция.");
			}
        }
	}
}
