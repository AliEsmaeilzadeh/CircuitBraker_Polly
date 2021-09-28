using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CircuitBreaker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CicuitBreaker.Client.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string _baseAddress = "http://localhost:63735/Home/Index";
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(_baseAddress);
            var str = await result.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<ClientMessage>(str);
            return Ok(obj);
        }


        [HttpGet]
        public async Task<IActionResult> Odd()
        {
            string _baseAddress = "http://localhost:63735/Home/Odd";
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(_baseAddress);
            var str = await result.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<ClientMessage>(str);
            return Ok(obj);
        }


    }
}
