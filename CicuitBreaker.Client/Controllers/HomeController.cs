using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using CircuitBreaker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;

namespace CicuitBreaker.Client.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class HomeController : ControllerBase
    {
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly AsyncFallbackPolicy<HttpResponseMessage> _fallbackPolicy;
        private static AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreaker = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode).Or<HttpRequestException>()
                                                                                              .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10),
        (d, c) =>
        {
            string a = "Break";
        },
        () =>
        {
            string a = "Reset";
        },
        () =>
        {
            string a = "Half";
        });



        public HomeController()
        {
            _retryPolicy = Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode)
                                 .RetryAsync(10, (d, c) => 
                                 {
                                    string a = "Retry";
                                 });

            _fallbackPolicy = Policy.HandleResult<HttpResponseMessage>(result => !result.IsSuccessStatusCode).Or<BrokenCircuitException>()
                                    .FallbackAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                                    {
                                        Content = new ObjectContent(typeof(Message), new Message
                                        {
                                            Id = 100,
                                            Text = "متن پیش فرض"
                                        }, new JsonMediaTypeFormatter())
                                    });
        }



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
            var result = await _retryPolicy.ExecuteAsync(() => client.GetAsync(_baseAddress));
            var str = await result.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<ClientMessage>(str);
            return Ok(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Three()
        {
            string _baseAddress = "http://localhost:63735/Home/Three";
            HttpClient client = new HttpClient();
            var result = await _fallbackPolicy.ExecuteAsync(() => _circuitBreaker.ExecuteAsync(() => client.GetAsync(_baseAddress)));
            var str = await result.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<ClientMessage>(str);
            return Ok(obj);
        }
    }
}
