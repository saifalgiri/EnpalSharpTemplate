using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EnpalSharpTemplate.Model;
using EnpalSharpTemplate.ServiceInterface;
using EnpalSharpTemplate.Services;
using EnpalSharpTemplate.ViewModel;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnpalSharpTemplate.Controllers
{
    [RequireHttps]
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HistoryController : Controller
    {
        private IHistoryRegistration _historyRegistration;
        private IHistorySearch _historySearch;

        public HistoryController(IHistoryRegistration historyRegistration, IHistorySearch historySearch)
        {
            _historyRegistration = historyRegistration;
            _historySearch = historySearch;
        }

        [HttpGet, Route("GetByKey"), MapToApiVersion("1.0")]
        public IActionResult GetSingleVersion(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return Json(new { Message = "Key is not given!", StatusCodes = HttpStatusCode.OK });
            }
            var result = _historySearch.GetSingleVersion(Key);
            return result == null ? 
                Json(new { HttpStatusCode.NoContent }) :Json(new { HttpStatusCode.OK, data = result });

        }

        [HttpGet]
        [MapToApiVersion("1.0"), Route("GetAll")]
        public IActionResult GetAllVersions()
        {
            var result = _historySearch.GetAll();
            return result == null ? Json(new
            {
                Message = "No version available at the moment!",
                HttpStatusCode.NoContent
            }) : Json(new { HttpStatusCode.OK, data = result });
        }

        [HttpPost]
        [MapToApiVersion("1.0"),Route("AddRecord")]
        public async Task<IActionResult> AddVersion([FromBody]PayloadModel payload)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("One or more required values not given!");
            }

            bool result = await _historyRegistration.Create(payload);
            return result == true ? Json(new { Message = "Added Successfully", 
                HttpStatusCode.Created }) : Json(new { HttpStatusCode.BadRequest });
        
        }
    }
}
