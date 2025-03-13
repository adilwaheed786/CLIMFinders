using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class SearchService(IVehicleService vehicleService)  : ISearchService
    { 
        private readonly IVehicleService _vehicleService = vehicleService;

        public List<VehicleListDto> GetSearchResult(string VIN)
        {
            try
            {
                var search = _vehicleService.GetAllVehicles().Where(e=>e.VIN.Contains(VIN));
                return search.ToList();
            }
            catch {
                throw;
            }
        }         
    }
} 