using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dotnetCore_SearchEngine.Models;
using dotnetCore_SearchEngine.Services;
using dotnetCore_SearchEngine.Services.Logging;

namespace dotnetCore_SearchEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private IService _service;
        private EventLogger _eventLogger;
        public SearchController(IService service)
        {
            _service = service;
            _eventLogger = EventLogger.Instance;
        }

        // GET: api/Search
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/Search
        [HttpPost]
        public async Task<IActionResult> Post(SearchData formData)
        {
            await _eventLogger.LogEvent(".SearchController api/Search/ -> Post");
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            return await _service.ProcessData(formData);
        }
    }
}