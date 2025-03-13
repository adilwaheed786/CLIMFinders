using CLIMFinders.Application.DTOs;

namespace CLIMFinders.Application.Interfaces
{
    public interface ISearchService
    {
        List<VehicleListDto> GetSearchResult(string VIN);
    }
}
