using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EnpalSharpTemplate.Model;
using EnpalSharpTemplate.ServiceInterface;
using EnpalSharpTemplate.Services;
using FluentValidation;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnpalSharpTemplate.Controllers.V1_0
{
    /// <summary>
    /// 
    /// </summary>
    [RequireHttps]
    [Authorize]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SampleController : Controller
    {
        private ISampleService _smapleService;

        public SampleController(ISampleService sampleService)
        {
            _smapleService = sampleService;
        }

        /// <summary>
        /// Sample Get
        /// </summary>
        /// <param name="sampleValue"></param>
        /// <param name="config">Some sample test value</param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0"), Route("{sampleValue}")]
        public async Task<IActionResult> Search(string sampleValue)
        {
            if (string.IsNullOrEmpty(sampleValue))
            {
                return Json(new { Message = "Key is not given!", StatusCodes = HttpStatusCode.OK });
            }
            var result = await _smapleService.ExecuteResultAsync(sampleValue);
            return result == null ? 
                Json(new { HttpStatusCode.NoContent }) : Json(new { HttpStatusCode.OK, data = result });
        }


    }
}
