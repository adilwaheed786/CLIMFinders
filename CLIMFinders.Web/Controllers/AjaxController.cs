using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CLIMFinders.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AjaxController(IVehicleService context) : ControllerBase
    {
        private readonly IVehicleService context = context;

        [HttpGet("GetVehicleMakes")]
        public IActionResult GetVehicleMakes()
        {
            var models = context.GetVehicleMakes();
            return Ok(models);
        }
        [HttpGet("GetVehicleModel/{makeId}")]
        public IActionResult GetModelsByMake(int makeId)
        {
            var models = context.GetVehicleModel(makeId);
            return Ok(models);
        }
    }
}