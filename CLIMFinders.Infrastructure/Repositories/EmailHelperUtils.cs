using CLIMFinders.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;

namespace CLIMFinders.Infrastructure.Repositories
{
    public class EmailHelperUtils(IWebHostEnvironment env): IEmailHelperUtils
    {
        private readonly IWebHostEnvironment _env = env;

        public string FillEmailContents(
            object dataToFill,
            string fileName,
            string Name
        )
        {

            var templateContent = File.ReadAllText(
                Path.Combine(_env.ContentRootPath, "wwwroot/EmailTemplates/" + fileName + ".html")
            );
             
            templateContent = templateContent.Replace("@Name", Name); 
             
            foreach (PropertyInfo prop in dataToFill.GetType().GetProperties())
            {
                var propName = "@" + Convert.ToString(prop.Name);
                var propValue = Convert.ToString(prop.GetValue(dataToFill, null)!);
                templateContent = templateContent.Replace(propName, propValue);
            }

            templateContent = templateContent.Replace("True", "Yes");
            templateContent = templateContent.Replace("False", "No");

            return templateContent;
        }
    }
}
