using CLIMFinders.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Application.Interfaces
{
    public interface IVehicleService
    {
        List<VehicleListDto> GetAllVehicles();
        List<SelectListItem> GetVehicleColors();
        List<SelectListItem> GetVehicleMakes();
        List<SelectListItem> GetVehicleModel(int Id);
        List<SelectListItem> StatusOptions();
        List<SelectListItem> PopulateYear();
        ResponseDto SaveVehicle(VehicleDto vehicle);
        ResponseDto SaveUserVehicle(VehicleDto vehicle);
        List<VehicleListDto> GetVehicles();
        VehicleDto GetVehicle(int Id);
        void DeleteVehicle(int Id);
    }
}
