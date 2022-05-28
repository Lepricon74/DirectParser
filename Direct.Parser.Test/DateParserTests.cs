using System;
using FluentAssertions;
using NUnit.Framework;

namespace Direct.Parser.Test
{
	[TestFixture]
	public class DateParserTests
	{
		[Test]
		public void Test1()
		{
			var testStr = "Огнетушители в наличии. Оптом и в розницу. Доставка в сжатые сроки. Скидки до 28.01";

			var result = DateParser.GetDateTimeFromText(testStr);

			result.Should().Be(new DateTime(2022, 1, 28));
		}

		[Test]
		public void Test2()
		{
			var testStr = "Ищете металлопрокат? – Скидки весь март!";

			var result = DateParser.GetDateTimeFromText(testStr);

			result.Should().BeSameDateAs(new DateTime(2022, 3, 31));
		}

		[Test]
		public void Test3()
		{
			var testStr = "Большой ассортимент. Скидки до 31 марта";

			var result = DateParser.GetDateTimeFromText(testStr);

			result.Should().BeSameDateAs(new DateTime(2022, 3, 31));
		}

		[Test]
		public void Test4()
		{
			var testStr = "Команда экспертов. Найдём новые точки роста. Весь апрель скидки! Звоните";

			var result = DateParser.GetDateTimeFromText(testStr);

			result.Should().BeSameDateAs(new DateTime(2022, 4, 30));
		}

		[Test]
		public void Test5()
		{
			var testStr = "Контекстная реклама в Промо Эксперт! – Акция до 25 апреля";

			var result = DateParser.GetDateTimeFromText(testStr);

			result.Should().BeSameDateAs(new DateTime(2022, 4, 25));
		}

		[Test]
		public void Test6()
		{
			var testStr = "Контекстная реклама в Промо Эксперт! – Акция с 25 до 30 апреля";

			var result = DateParser.GetDateTimeFromText(testStr);

			result.Should().BeSameDateAs(new DateTime(2022, 4, 30));
		}

		[Test]
		public void Test7()
		{
			var testStr = "Контекстная реклама в Промо Эксперт! – Акция с 25.04 до 30.04";

			var result = DateParser.GetDateTimeFromText(testStr);

			result.Should().BeSameDateAs(new DateTime(2022, 4, 30));
		}

		[Test]
		public void Test8()
		{
			var testStr = "Контекстная реклама в Промо Эксперт! – Акция с 25 апреля до 10 мая";

			var result = DateParser.GetDateTimeFromText(testStr);

			result.Should().BeSameDateAs(new DateTime(2022, 5, 10));
		}
	}
}