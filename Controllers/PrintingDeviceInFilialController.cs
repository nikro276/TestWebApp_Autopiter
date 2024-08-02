using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TestWebApp_Autopiter.Models;

namespace TestWebApp_Autopiter.Controllers
{
    [Controller]
    public class PrintingDeviceInFilialController : Controller
    {
        ApplicationContext db;
        IMemoryCache cache;
        JsonSerializerOptions? options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) };

        public PrintingDeviceInFilialController(ApplicationContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
        }

        [HttpGet]
        public async Task<JsonResult> GetInstallations(int? filialId)
        {
            if (filialId != null)
                return Json(await db.PrintingDevicesInFilials.Where(installation => installation.FilialId == filialId).ToListAsync(), options);
            else
                return Json(await db.PrintingDevices.ToListAsync(), options);
        }

        [HttpGet]
        public async Task<JsonResult> GetInstallation(int? installationId)
        {
            if (installationId == null)
                throw new HttpRequestException("Не указан Id инсталляции", null, System.Net.HttpStatusCode.BadRequest);
            cache.TryGetValue("PrintingDeviceInFilial" + installationId, out PrintingDeviceInFilial? installation);
            if (installation == null)
            {
                installation = await db.PrintingDevicesInFilials.FirstOrDefaultAsync(x => x.Id == installationId);
                if (installation != null)
                    cache.Set("PrintingDeviceInFilial" + installationId, installation, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(105)));
            }
            return Json(installation, options);
        }

        [HttpDelete]
        public async Task DeleteInstallation(int? installationId)
        {
            PrintingDeviceInFilial? installation;
            if (installationId == null || (installation = await db.PrintingDevicesInFilials.FirstOrDefaultAsync(install => install.Id == installationId)) == null)
                throw new HttpRequestException("Инсталляция с указанным Id не существует", null, System.Net.HttpStatusCode.BadRequest);
            db.PrintingDevicesInFilials.Remove(installation);
            await db.SaveChangesAsync();
            cache.Remove("PrintingDeviceInFilial" + installationId);
        }

        [HttpPost]
        public async Task<int> Add(PrintingDeviceInFilial installation)
        {
            CheckAddFormData(installation);
            if (!Request.Form.ContainsKey("NumberInFilial"))
            {
                var numbersInFilial = (from install in db.PrintingDevicesInFilials
                                             where install.FilialId == installation.FilialId
                                             select install.NumberInFilial).ToList();
                if (numbersInFilial.Count() > 0)
                    installation.NumberInFilial = numbersInFilial.Max() + 1;
                else
                    installation.NumberInFilial = 1;
            }
            db.PrintingDevicesInFilials.Add(installation);
            await db.SaveChangesAsync();
            var installationId = installation.Id.Value;
            cache.Set("PrintingDeviceInFilial" + installationId, installation, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(105)));
            Response.StatusCode = 201;
            return installationId;
        }

        private void CheckAddFormData(PrintingDeviceInFilial installation)
        {
            if (Request.Form.ContainsKey("Id") && !db.PrintingDevicesInFilials.Any(install => install.Id == installation.Id))
                throw new HttpRequestException("Инсталляция с указанным Id уже существует", null, System.Net.HttpStatusCode.BadRequest);
            if (!Request.Form.ContainsKey("Name"))
                throw new HttpRequestException("Не указано название инсталляции", null, System.Net.HttpStatusCode.BadRequest);
            if (!Request.Form.ContainsKey("FilialId"))
                throw new HttpRequestException("Не указан филиал", null, System.Net.HttpStatusCode.BadRequest);
            if (!db.Filials.Any(filial => filial.Id == installation.FilialId)) 
                throw new HttpRequestException("Нет филиала с таким Id", null, System.Net.HttpStatusCode.BadRequest);
            if (!Request.Form.ContainsKey("PrintingDeviceId"))
                throw new HttpRequestException("Не указано устройство печати", null, System.Net.HttpStatusCode.BadRequest);
            if (!db.PrintingDevices.Any(printingDevice => printingDevice.Id == installation.PrintingDeviceId))
                throw new HttpRequestException("Нет устройства печати с таким Id", null, System.Net.HttpStatusCode.BadRequest);
            if (Request.Form.ContainsKey("NumberInFilial"))
            {
                if (installation.NumberInFilial < 1)
                    throw new HttpRequestException("Порядковый номер должен быть больше 0", null, System.Net.HttpStatusCode.BadRequest);
                if (db.PrintingDevicesInFilials.Any(install => install.FilialId == installation.FilialId && install.NumberInFilial == installation.NumberInFilial))
                    throw new HttpRequestException("Инсталляция с таким порядковым номером уже существует", null, System.Net.HttpStatusCode.BadRequest);
            }
            if (!Request.Form.ContainsKey("IsDefault"))
                throw new HttpRequestException("Не указан признак использования \"По умолчанию\"", null, System.Net.HttpStatusCode.BadRequest);
            if (installation.IsDefault && db.PrintingDevicesInFilials.Any(install => install.FilialId == installation.FilialId && install.IsDefault == true))
                throw new HttpRequestException("В данном филиале уже существует инсталляция по умолчанию", null, System.Net.HttpStatusCode.BadRequest);
            if (!installation.IsDefault && !db.PrintingDevicesInFilials.Any(install => install.FilialId == installation.FilialId && install.IsDefault == true))
                throw new HttpRequestException("В данный филиал требуется добавить инсталляцию по умолчанию", null, System.Net.HttpStatusCode.BadRequest);
        }
    }
}