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
		public static IEnumerable<TestCaseData> CorrectDateTestCases
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
				yield return new TestCaseData("Вывод сайта в Топ-10. Настроим плотный поток клиентов. Весь апрель скидки!", new DateTime(2022, 4, 30));
				yield return new TestCaseData("Беспрецедентный опыт в SEO и аналитике сайтов. Скидки до 20% до 25.04! Звоните!", new DateTime(2022, 4, 25));
			}
		}

		[TestCaseSource(nameof(CorrectDateTestCases))]
		public async Task DateParser_Should_Return_Correct_Date(string adText, DateTime expectedDate)
		{
			var result = await DateParser.GetDateTimeFromText(adText);

			result.Should().BeSameDateAs(expectedDate);
		}

		public static IEnumerable<TestCaseData> NullTestCases
		{
			get
			{
				yield return new TestCaseData("Добрый вечер");
				yield return new TestCaseData("Test sandbox banner 1 text");
				yield return new TestCaseData("Test sandbox banner 5 text");
				yield return new TestCaseData("25 апреля");
				yield return new TestCaseData("28.01");
				yield return new TestCaseData("Июнь");
			}
		}

		[TestCaseSource(nameof(NullTestCases))]
		public async Task DateParser_Should_Return_Null(string adText)
		{
			var result = await DateParser.GetDateTimeFromText(adText);

			result.Should().BeNull();
		}
	}
}