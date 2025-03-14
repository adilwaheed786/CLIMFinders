using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class SearchService(IVehicleService vehicleService) : ISearchService
    {
        private readonly IVehicleService _vehicleService = vehicleService;

        public List<VehicleListDto> GetSearchResult(string search)
        {
            try
            {
                var result = _vehicleService.GetAllVehicles()
                        .Where(e => e.VIN.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        e.Make.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        e.Model.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        e.Color.Contains(search, StringComparison.OrdinalIgnoreCase));

                return result.ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}