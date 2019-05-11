using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DNet_Communication.Connection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using DNet_DataContracts;

namespace DNet_Processing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IConnect _connectionInstance;
        IConfiguration _configuration;
        ILogger<ValuesController> _logger;

        public ValuesController(IConfiguration configuration, IConnect connectionInstance, ILogger<ValuesController> logger)
        {
            _connectionInstance = connectionInstance;
            _configuration = configuration;
            _logger = logger;

            //_connectionInstance.Connect("http://localhost:5000/mainhub", DNet_DataContracts.ModuleTypes.Processing);
            ConnectToHub().RunSynchronously();
        }

        private async Task ConnectToHub()
        {
            if (await _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["PrimaryHubUri"], ModuleTypes.Processing))
            {
                Console.WriteLine("Connected to first hub");
            }
            else if (await _connectionInstance.Connect(_configuration.GetSection("ConnectionInfo")["SecondaryHubUri"], ModuleTypes.Processing))
            {
                Console.WriteLine("Connected to second hub");
            }
            else
            {
                _logger.LogCritical("Could not connect to any hub, looking for available hubs in network"); //TODO: Make this feature
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    await ConnectToHub();

        //    return Ok();
        //}

        //GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //await ConnectToHub();
            
            return Ok(new string[] { "value1", "value2" });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
