using CLIMFinders.Application.Enums;
using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class StaticSelectOptionService : IStaticSelectOptionService
    {
        public List<SelectListItem> StatusOptions()
        {
            var options = new List<SelectListItem>
            {
                new() { Value = "1", Text = "Impounded" },
                new() { Value = "2", Text = "Towed" },
                new() { Value = "3", Text = "Released" }
            };
            return options;
        }
        public List<SelectListItem> RoleOptions() 
        {
            var options = new List<SelectListItem>
            {
                new() { Text = RoleEnum.Users.ToString(), Value = ((int)RoleEnum.Users).ToString() },
                new() { Text = RoleEnum.Business.ToString(), Value = ((int)RoleEnum.Business).ToString() },
            };
            return options;
        }
        public List<SelectListItem> SubRoleOptions() 
        {
            var options = new List<SelectListItem>
            {
                new() { Text = SubRoleEnum.Tow.ToString(), Value = ((int)SubRoleEnum.Tow).ToString() },
                new() { Text = SubRoleEnum.Impound.ToString(), Value = ((int)SubRoleEnum.Impound).ToString() }
            };
            return options;
        }
        public List<SelectListItem> PopulateYear()
        {
            int startYear = 1900;
            int currentYear = DateTime.Now.Year;

            var years = new List<SelectListItem>();

            for (int year = currentYear; year >= startYear; year--)
            {
                years.Add(new SelectListItem { Value = year.ToString(), Text = year.ToString() });
            }

            return years;
        }
    }
}
