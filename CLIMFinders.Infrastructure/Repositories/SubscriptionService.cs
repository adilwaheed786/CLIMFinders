using AutoMapper;
using CLIMFinders.Application.DTOs;
using CLIMFinders.Application.Interfaces;
using CLIMFinders.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class SubscribeService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) : ISubscribeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork; 
        private readonly IMapper _mapper = mapper;
        private readonly IUserService _userService = userService;

        //public GenericResponse AddUpdateSubscription(SubscriptionDto requestDto)
        //{
        //    try
        //    {
        //        var repository = _unitOfWork.GetRepository<Subscriptions>();
        //        if (requestDto.Id == 0)
        //        {
        //            var mapped = _mapper.Map<Subscriptions>(requestDto);
        //            var entity = repository.Insert(mapped);
        //            entity.UserId = entity.AddedById = entity.ModifiedById = _userService.GetUserId();
        //            entity.AddedOn = entity.ModifiedOn = DateTime.Now;
        //            repository.Save();
        //            GenericResponse genericResponse = new(entity.Id, entity.UserId, "Subscription Confirmed untill" + entity.EndDate.ToString("MMM dd yyyy HH:mm"), entity, true);
        //            return genericResponse;

        //        }
        //        else
        //        {
        //            var payment = repository.GetById(requestDto.Id);
        //            var mapped = _mapper.Map(requestDto, payment);
        //            mapped.UserId = mapped.ModifiedById = _userService.GetUserId();
        //            mapped.ModifiedOn = DateTime.Now;
        //            repository.Update(mapped);
        //            repository.Save();
        //            GenericResponse genericResponse = new(mapped.Id, mapped.UserId, "Subscription Renews untill " + mapped.EndDate.ToString("MMM dd yyyy HH:mm"), mapped, true);
        //            return genericResponse;
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public List<SubscriptionPlansDto> GetSubscriptionPlans()
        {
            var replan = _unitOfWork.GetRepository<SubscriptionPlans>();
            var enPlan = replan.GetAll();
            var mappedPlan = _mapper.Map<List<SubscriptionPlansDto>>(enPlan);

            var repS = _unitOfWork.GetRepository<PlanServices>();
            var enPs = repS.GetAll();

            foreach (var pl in mappedPlan)
            {
                var services = enPs.Where(e => e.PlanId == pl.Id); 
                pl.PlanServicesDto = services != null ? _mapper.Map<List<PlanServicesDto>>(services) : [];
            }
            return mappedPlan;
        }
    }
}