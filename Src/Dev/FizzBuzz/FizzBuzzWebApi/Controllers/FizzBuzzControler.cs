using GrainInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FizzBuzzWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FizzBuzzController : ControllerBase
    {
        private readonly IFizzBuzzService _fizzBuzzService;

        public FizzBuzzController(IFizzBuzzService fizzBuzzService)
        {
            _fizzBuzzService = fizzBuzzService;
        }

        [HttpGet]
        [Route("{value}")]
        public async Task<IActionResult> GetFizzBuzz(int value)
        {
            string result = await _fizzBuzzService.GetFizzBuzz(value);
            return Ok(result);
        }
    }
}
