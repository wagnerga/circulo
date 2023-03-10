using Database;

namespace Services.Interfaces
{
	public interface IPharmacyVisitService
	{
		Task<List<string>> GetPharmaciesToVisit(int x);
		Task InsertPharmaciesToVisit(List<PharmacyVisited> pharmaciesVisited, List<PharmacyNearby> pharmaciesNearby);
	}
}