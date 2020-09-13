using System.Collections.Generic;
using EnpalSharpTemplate.Model;
using EnpalSharpTemplate.ServiceInterface;
using EnpalSharpTemplate.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnpalSharpTemplate.Controllers.V1_1
{
    /// <summary>
    /// 
    /// </summary>
    [RequireHttps]
    [Authorize]
    [ApiController]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SampleController : Controller
    {
        private readonly TelemetryClient _telemetry;
        private IHistorySearch _historySearch;

        public SampleController(TelemetryClient telemetry, IHistorySearch historySearch)
        {
            _telemetry = telemetry;
            _historySearch = historySearch;
        }

        /// <summary>
        /// Sample Get
        /// </summary>
        /// <param name="sampleValue">Some sample test value</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<string>), 200)]
        [ProducesResponseType(typeof(List<ErrorMessage>), 400)]
        [ProducesResponseType(typeof(List<ErrorMessage>), 500)]
        [HttpGet]
        [MapToApiVersion("1.1")]
        public IActionResult Search(string sampleValue)
        {
            if(string.IsNullOrEmpty(sampleValue))
            {
                return Json(new { Message = "Sample Value is not given!", StatusCode = StatusCodes.Status400BadRequest });
            }
            var result = _historySearch.GetSingleVersion(sampleValue);
            return result == null ? Json(new { StatusCodes.Status204NoContent }) : Json(new { StatusCodes.Status200OK, data = result });
            
        }

    }
}
