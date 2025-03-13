using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Application.Interfaces
{
    public interface IStaticSelectOptionService
    {
        public List<SelectListItem> RoleOptions();
        List<SelectListItem> SubRoleOptions();
        public List<SelectListItem> StatusOptions();
        List<SelectListItem> PopulateYear();
    }
}
