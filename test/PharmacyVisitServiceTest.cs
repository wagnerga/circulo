using Database;
using Services;

namespace Test
{
	public class PharmacyVisitServiceTest
	{
		[SetUp]
		public void Setup()
		{
		}

		[TestCase(1, new[] { "Costco" })]
		[TestCase(2, new[] { "Costco", "CVS Pharmacy" })]
		[TestCase(4, new[] { "Costco", "CVS Pharmacy", "Kroger", "RMB" })]
		[TestCase(5, new[] { "Costco", "CVS Pharmacy", "Kroger", "RMB", "Giant Eagle" })]
		[TestCase(6, new[] { "Costco", "CVS Pharmacy", "Kroger", "RMB", "Giant Eagle", "Walmart" })]
		[TestCase(7, new[] { "Not enough data" })]
		public void Test1(int x, string[] expected)
		{
			var pharmaciesNearby = new List<PharmacyNearby>
			{
				new()
				{
					PharmacyName = "Giant Eagle",
					Copay = 2,
					Distance = 5,
					SupportsPriorAuth = false
				},
				new()
				{
					PharmacyName = "Kroger",
					Copay = 3,
					Distance = 4,
					SupportsPriorAuth = true
				},
				new()
				{
					PharmacyName = "Kroger",
					Copay = 4,
					Distance = 5,
					SupportsPriorAuth = false
				},
				new()
				{
					PharmacyName = "RMB",
					Copay = 5,
					Distance = 3,
					SupportsPriorAuth = true
				}
			};
			var pharmaciesVisited = new List<PharmacyVisited>
			{
				new()
				{
					PharmacyName = "CVS Pharmacy",
					SupportsPriorAuth = true
				},
				new()
				{
					PharmacyName = "Walmart",
					SupportsPriorAuth = false
				},
				new()
				{
					PharmacyName = "CVS Pharmacy",
					SupportsPriorAuth = false
				},
				new()
				{
					PharmacyName = "Costco",
					SupportsPriorAuth = true
				}
			};

			var actual = PharmacyVisitService.GetPharmaciesToVisit(x, pharmaciesVisited, pharmaciesNearby);

			Assert.AreEqual(expected.ToList(), actual);
		}
	}
}