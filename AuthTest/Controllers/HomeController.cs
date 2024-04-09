using AuthTest.Models;
using DP_backend.Models.DTOs;
using DP_backend.Models.DTOs.TSUAccounts;
using DP_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace AuthTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITSUAccountService _tsuAccountService;
        public HomeController(ILogger<HomeController> logger, ITSUAccountService tSUAccountService)
        {
            _logger = logger;
            _tsuAccountService = tSUAccountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/account/finish_login")]
        public async Task<IActionResult> TSUAuth(string token)
        {
            

            var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:7086/api/Auth/Auth", new StringContent(JsonConvert.SerializeObject(token), Encoding.UTF8, "application/json"));           
            response.EnsureSuccessStatusCode();
            var responseMsg = await response.Content.ReadAsStringAsync();
            return Ok(responseMsg);
        }
    }
}
