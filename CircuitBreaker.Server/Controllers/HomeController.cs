using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CircuitBreaker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CircuitBreaker.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class HomeController : ControllerBase
    {
        static int counter = 0;


        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new Message
            {
                Id = 1,
                Text = "درخواست با موفقیت انجام شد"
            });
        }


        [HttpGet]
        public IActionResult Odd()
        {
            counter += 1;
            if (counter % 2 != 0)
            {
                return Ok(new Message
                {
                    Id = counter,
                    Text = "درخواست در دفعات فرد با موفقیت انجام شد"
                });
            }
            return BadRequest(new Message
            {
                Id = counter,
                Text = "به علت زوج بودن شماره درخواست عملیات ناموفق بود"
            });

        }


        [HttpGet]
        public IActionResult Three()
        {
            counter += 1;
            if (counter % 3 == 0)
            {
                return Ok(new Message
                {
                    Id = counter,
                    Text = "درخواست در دفعات ضریب 3 با موفقیت انجام شد"
                });
            }
            return BadRequest(new Message
            {
                Id = counter,
                Text = "به علت ضریب 3 نبودن شماره درخواست عملیات ناموفق بود"
            });

        }


    }
}
