using AutoMapper;
using CLIMFinders.Application.DTOs;
using CLIMFinders.Domain.Entities;
using CLIMFinders.Domain.Entities;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace CLIMFinders.Web.ServiceExtension
{
    public class GenericMappingProfile : Profile
    {
        public GenericMappingProfile()
        {
            CreateMap<User, LoginResponseDto>()
                 .ForMember(dest => dest.BusinessId, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.Id : (int?)null))
                .ReverseMap();

            CreateMap<Vehicles, VehicleDto>().ReverseMap();
            CreateMap<VehiclesNotFound, VehicleDto>().ReverseMap();
            //CreateMap<Payments, PaymentRequestDto>().ReverseMap();

            CreateMap<Vehicles, VehicleListDto>()
            .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.VehicleMake.Name))
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.VehicleModel.Name))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.VehicleColor.Name)) 
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Users != null ? src.Users.FullName : null))
            .ReverseMap();
             
            // Map from User and Businesses to BusinessDto
            CreateMap<User, AddressDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.SubRoleId, opt => opt.MapFrom(src => src.SubRoleId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.Id : 0))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.ContactPerson : null))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.Phone : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.Address : null))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.City : null))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.State : null))
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.ZipCode : null))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Businesses != null ? src.Businesses.Description : null)).ReverseMap();

            CreateMap<BusinessCreditDto, AddressDto>().ReverseMap();
            CreateMap<UserAddress, AddressDto>().ReverseMap();
            CreateMap<PlanServices, PlanServicesDto>().ReverseMap();
            CreateMap<SubscriptionPlans, SubscriptionPlansDto>().ReverseMap();

        }
    }
}
