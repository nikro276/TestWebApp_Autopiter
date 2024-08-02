using System.Text.Encodings.Web;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApp_Autopiter.Models;
using Microsoft.Extensions.Options;

namespace TestWebApp_Autopiter.Controllers
{
    [Controller]
    public class UtilsController : Controller
    {
        ApplicationContext db;
        JsonSerializerOptions? options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) };

        public UtilsController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<JsonResult> GetFilials()
        {
            return Json(await db.Filials.ToListAsync(), options);
        }

        [HttpGet]
        public async Task<JsonResult> GetEmployees()
        {
            return Json(await db.Employees.ToListAsync(), options);
        }

        [HttpGet]
        public async Task<JsonResult> GetPrintingDevices(int? connectionTypeId)
        {
            if (connectionTypeId != null)
                return Json(await db.PrintingDevices.Where(device => device.ConnectionTypeId == connectionTypeId).ToListAsync(), options);
            else
                return Json(await db.PrintingDevices.ToListAsync(), options);
        }
    }
}