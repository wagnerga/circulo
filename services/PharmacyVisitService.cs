using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using PharmacyVisited = Database.PharmacyVisited;

namespace Services
{
	public class PharmacyVisitService : IPharmacyVisitService
	{
		private readonly DbContextOptionsBuilder<CirculoContext> _context;
		private readonly ILogger _logger;

		public PharmacyVisitService(IConfiguration configuration, ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger("PharmacyVisitService");
			_context = new DbContextOptionsBuilder<CirculoContext>();
			_context.UseNpgsql(configuration["ConnectionString"]);
		}

		/// <summary>
		///	Returns a list of pharmacy names to visit
		/// </summary>
		/// <param name="x">number of pharmacy names to return</param>
		/// <returns>list of pharmacy names</returns>
		public async Task<List<string>> GetPharmaciesToVisit(int x)
		{
			await using var context = new CirculoContext(_context.Options);

			var pharmaciesVisited = await context.PharmacyVisited.ToListAsync();
			var pharmaciesNearby = await context.PharmacyNearby.ToListAsync();

			try
			{
				return GetPharmaciesToVisit(x, pharmaciesVisited, pharmaciesNearby);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return new List<string> { "Not enough data" };
		}

		public static List<string> GetPharmaciesToVisit(int x, IReadOnlyCollection<PharmacyVisited> pharmaciesVisited, IEnumerable<PharmacyNearby> pharmaciesNearby)
		{
			var pharmacyNames = new List<string>();
			var priorAuthVisits = pharmaciesVisited.Where(pharmacy => pharmacy.SupportsPriorAuth);
			var notPriorAuthVisits = pharmaciesVisited.Where(pharmacy => !pharmacy.SupportsPriorAuth);

			// first add prior auth visits in asc order, use distinct names
			pharmacyNames.AddRange(priorAuthVisits
				.Select(pharmacy => pharmacy.PharmacyName)
				.Distinct()
				.OrderBy(pharmacy => pharmacy));
			// next add nearby pharmacies (that aren't in list already) ordered by copay (less is better) then by distance (closer is better), use distinct names
			pharmacyNames.AddRange(pharmaciesNearby
				.Where(pharmacy => !pharmacyNames.Contains(pharmacy.PharmacyName))
				.OrderBy(pharmacy => !pharmacy.SupportsPriorAuth)
				.ThenBy(pharmacy => pharmacy.Copay)
				.ThenBy(pharmacy => pharmacy.Distance)
				.Select(pharmacy => pharmacy.PharmacyName)
				.Distinct());
			// next add visits with no prior auth, and aren't already in the list in asc order, use distinct names
			pharmacyNames.AddRange(notPriorAuthVisits
				.Where(pharmacy => !pharmacyNames.Contains(pharmacy.PharmacyName))
				.Select(pharmacy => pharmacy.PharmacyName)
				.Distinct()
				.OrderBy(pharmacy => pharmacy));

			// if not enough data return empty list
			if (pharmacyNames.Count < x)
				return new List<string> { "Not enough data" };

			// return x pharmacies
			return pharmacyNames.Take(x).ToList();
		}

		/// <summary>
		/// Inserts pharmacies visited and pharmacies nearby in to the database
		/// </summary>
		/// <param name="pharmaciesVisited"></param>
		/// <param name="pharmaciesNearby"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public async Task InsertPharmaciesToVisit(List<PharmacyVisited> pharmaciesVisited, List<PharmacyNearby> pharmaciesNearby)
		{
			await using var context = new CirculoContext(_context.Options);

			await context.Database.ExecuteSqlRawAsync("DELETE FROM \"PharmacyVisited\"");
			await context.Database.ExecuteSqlRawAsync("DELETE FROM \"PharmacyNearby\"");

			context.PharmacyVisited.AddRange(pharmaciesVisited);
			context.PharmacyNearby.AddRange(pharmaciesNearby);

			await context.SaveChangesAsync();
		}
	}
}