using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FizzBuzzWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FizzBuzzController : ControllerBase
    {
        private readonly IClusterClient _clusterClient;

        public FizzBuzzController(IClusterClient clusterClient)
        {
            this._clusterClient = clusterClient;
        }

        [HttpGet]
        [Route("{value}")]
        public async Task<IActionResult> GetFizzBuzz(int value)
        {
            IFizzBuzz fizzBuzz = _clusterClient.GetGrain<IFizzBuzz>(0);
            string result = await fizzBuzz.Evaluate(value);
            return Ok(result);
        }
    }
}
