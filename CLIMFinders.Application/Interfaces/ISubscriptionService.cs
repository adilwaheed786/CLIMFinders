using CLIMFinders.Application.DTOs;

namespace CLIMFinders.Application.Interfaces
{
    public interface ISubscribeService
    {
        List<SubscriptionPlansDto> GetSubscriptionPlans();
       // GenericResponse AddUpdateSubscription(SubscriptionDto requestDto);
    }
}
