using AutoMapper;
using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class VehicleService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, IStaticSelectOptionService staticSelectOptionService) : IVehicleService
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;
        private readonly IUserService _userService = userService;
        private readonly IStaticSelectOptionService _staticSelectOptionService = staticSelectOptionService;
         
        public List<VehicleListDto> GetVehicles()
        {
            try
            {
                var repository = unitOfWork.GetRepository<Vehicles>();
                var response = GetAllVehicles()
                    .Where(e=>e.UserId == _userService.GetUserId());
                var lstVehicles = mapper.Map<List<VehicleListDto>>(response);
                lstVehicles.ForEach(v =>
                {
                    v.BoundStatus = StatusOptions().FirstOrDefault(e => e.Value == v.Status.ToString()).Text;
                });

                return lstVehicles;
            }
            catch
            {
                throw;
            }
        }
        public List<VehicleListDto> GetAllVehicles()
        {
            try
            {
                var repository = unitOfWork.GetRepository<Vehicles>();
                var response = repository.GetAllInclude(v => v.VehicleMake, v => v.VehicleModel, v => v.VehicleColor, v => v.Users, v=>v.Users.Businesses);
                var lstVehicles = mapper.Map<List<VehicleListDto>>(response);
                lstVehicles.ForEach(v =>
                {
                    v.BoundStatus = StatusOptions().FirstOrDefault(e => e.Value == v.Status.ToString()).Text;                    
                });

                return lstVehicles;
            }
            catch
            {
                throw;
            }
        }
        public List<SelectListItem> GetVehicleColors()
        {
            try
            {
                var repository = unitOfWork.GetRepository<VehicleColor>();
                var response = repository.GetAll();
                return DropdownHelper.GetDropdownList(response, e => e.Id, e => e.Name);
            }
            catch
            {
                throw;
            }
        }
        public List<SelectListItem> GetVehicleMakes()
        {
            try
            {
                var repository = unitOfWork.GetRepository<VehicleMake>();
                var response = repository.GetAll();
                return DropdownHelper.GetDropdownList(response, e => e.Id, e => e.Name);
            }
            catch
            {
                throw;
            }
        }
        public List<SelectListItem> GetVehicleModel(int Id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<VehicleModel>();
                var response = repository.GetAll().Where(e => Id == 0 || e.MakeId == Id);
                return DropdownHelper.GetDropdownList(response, e => e.Id, e => e.Name);
            }
            catch
            {
                throw;
            }
        }
        public List<SelectListItem> StatusOptions()
        { 
            return _staticSelectOptionService.StatusOptions();
        }
        public List<SelectListItem> PopulateYear()
        { 
            return _staticSelectOptionService.PopulateYear();
        }
        public ResponseDto SaveVehicle(VehicleDto vehicle)
        {
            ResponseDto response = new();
            try
            {
                response = AddOrUpdateVehicle(vehicle);
            }
            catch
            {
                response.Status = "An unexpected error occurred";
                throw;
            }
            return response;
        }
        public ResponseDto SaveUserVehicle(VehicleDto vehicle)
        {
            ResponseDto response = new();
            try
            {
                response = AddOrUpdateUserVehicle(vehicle);
            }
            catch
            {
                response.Status = "An unexpected error occurred";
                throw;
            }
            return response;
        }
        private ResponseDto AddOrUpdateVehicle(VehicleDto model)
        {
            ResponseDto response = new();
            var repository = unitOfWork.GetRepository<Vehicles>();
            if (IsVehicleExists(model.VIN, model.Id))
            {
                response.Id = -1;
                response.Name = model.VIN;
                response.Status = "Vehicle already exists";
            }
            else
            {
                if (model.Id == 0)
                {
                    var mappedObj = mapper.Map<Vehicles>(model);
                    mappedObj.UserId = _userService.GetUserId();
                    mappedObj.AddedById = mappedObj.ModifiedById = _userService.GetUserId();
                    var entity = repository.Insert(mappedObj);
                    response.Id = entity.Id;
                    response.Name = entity.VIN;
                    response.Status = "Vehicle detail added successfully";
                }
                else
                {
                    var detail = repository.GetById(model.Id);
                    detail.UserId = _userService.GetUserId();
                    detail.Status = model.Status;
                    detail.VIN = model.VIN;
                    detail.ColorId = model.ColorId;
                    detail.MakeId = model.MakeId;
                    detail.ModelId = model.ModelId;
                    detail.Note = model.Note;
                    detail.PickedOn = model.PickedOn;
                    detail.Year = model.Year;
                    detail.ContactInformation= model.ContactInformation;    
                    detail.ImpoundFees = model.ImpoundFees;
                    detail.LocationDetails = model.LocationDetails; 
                    detail.ReasonImpoundent = model.ReasonImpoundent;   
                    detail.ModifiedById = _userService.GetUserId();
                    repository.Update(detail);
                    response.Id = detail.Id;
                    response.Name = detail.VIN;
                    response.Status = "Vehicle detail updated successfully";
                }
                repository.Save();
            }
            return response;
        }
        private ResponseDto AddOrUpdateUserVehicle(VehicleDto model)
        {
            ResponseDto response = new();
            var repository = unitOfWork.GetRepository<VehiclesNotFound>();
            if (IsUserVehicleExists(model.VIN, model.Id))
            {
                response.Id = -1;
                response.Name = model.VIN;
                response.Status = "Vehicle already exists";
            }
            else
            {
                if (model.Id == 0)
                {
                    var mappedObj = mapper.Map<VehiclesNotFound>(model);
                    mappedObj.UserId = _userService.GetUserId();
                    mappedObj.AddedById = mappedObj.ModifiedById = _userService.GetUserId();
                    var entity = repository.Insert(mappedObj);
                    response.Id = entity.Id;
                    response.Name = entity.VIN;
                    response.Status = "Vehicle detail added successfully";
                }
                else
                {
                    var detail = repository.GetById(model.Id);
                    detail.UserId = _userService.GetUserId();
                    detail.Status = model.Status;
                    detail.VIN = model.VIN;
                    detail.ColorId = model.ColorId;
                    detail.MakeId = model.MakeId;
                    detail.ModelId = model.ModelId;
                    detail.Note = model.Note;
                    detail.PickedOn = model.PickedOn;
                    detail.Year = model.Year;
                    detail.ContactInformation = model.ContactInformation;
                    detail.ImpoundFees = model.ImpoundFees;
                    detail.LocationDetails = model.LocationDetails;
                    detail.ReasonImpoundent = model.ReasonImpoundent;
                    detail.ModifiedById = _userService.GetUserId();
                    repository.Update(detail);
                    response.Id = detail.Id;
                    response.Name = detail.VIN;
                    response.Status = "Vehicle detail updated successfully";
                }
                repository.Save();
            }
            return response;
        }

        public VehicleDto GetVehicle(int Id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<Vehicles>();
                var response = repository.GetById(Id);
                var vehicle = mapper.Map<VehicleDto>(response);
                return vehicle;
            }
            catch
            {
                throw;
            }
        }
        public void DeleteVehicle(int Id)
        {
            var repository = unitOfWork.GetRepository<Vehicles>();
            repository.Delete(Id);
            repository.Save(); 
        }
        bool IsVehicleExists(string vIN, int Id = 0)
        {
            var repository = unitOfWork.GetRepository<Vehicles>();
            return repository != null && repository.GetAll().Any(x => x.VIN == vIN && x.Id != Id);
        }
        bool IsUserVehicleExists(string vIN, int Id = 0)
        {
            var repository = unitOfWork.GetRepository<VehiclesNotFound>();
            return repository != null && repository.GetAll().Any(x => x.VIN == vIN && x.Id != Id);
        }
    }
}
