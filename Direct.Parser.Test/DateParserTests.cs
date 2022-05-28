using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Direct.Parser.Test
{
	[TestFixture]
	public class DateParserTests
	{
		public static IEnumerable<TestCaseData> TestCases
		{
			get
			{
				yield return new TestCaseData("Огнетушители в наличии. Оптом и в розницу. Доставка в сжатые сроки. Скидки до 28.01", new DateTime(2022, 1, 28));
				yield return new TestCaseData("Ищете металлопрокат? – Скидки весь март!", new DateTime(2022, 3, 31));
				yield return new TestCaseData("Большой ассортимент. Скидки до 31 марта", new DateTime(2022, 3, 31));
				yield return new TestCaseData("Команда экспертов. Найдём новые точки роста. Весь апрель скидки! Звоните", new DateTime(2022, 4, 30));
				yield return new TestCaseData("Контекстная реклама в Промо Эксперт! – Акция до 25 апреля", new DateTime(2022, 4, 25));
				yield return new TestCaseData("Контекстная реклама в Промо Эксперт! – Акция с 25 до 30 апреля", new DateTime(2022, 4, 30));
				yield return new TestCaseData("Контекстная реклама в Промо Эксперт! – Акция с 25.04 до 30.04", new DateTime(2022, 4, 30));
				yield return new TestCaseData("Контекстная реклама в Промо Эксперт! – Акция с 25 апреля до 10 мая", new DateTime(2022, 5, 10));
			}
		}

		[TestCaseSource(nameof(TestCases))]
		public async Task DateParser_Should(string adText, DateTime expectedDate)
		{
			var result = await DateParser.GetDateTimeFromText(adText);

			result.Should().BeSameDateAs(expectedDate);
		}
	}
}