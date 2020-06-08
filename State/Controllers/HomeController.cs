using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using State.Models;

namespace State.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string resultaat = "Dit is jouw eerste bezoek";

            //zijn er cookies?
            if (Request.Cookies != null)
            {
                //is er een cookie met de naam lastvisit?
                if (Request.Cookies["lastvisit"] != null)
                {
                    //dan lezen we het resultaat uit de cookie
                    resultaat = "Welkom terug, je laatste bezoek was op " + Request.Cookies["lastvisit"];
                }

                //we slaan het huidige tijdstip op als laatste bezoek
                string laatsteBezoek = DateTime.Now.ToString();
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(365);
                Response.Cookies.Append("lastvisit", laatsteBezoek, option);                
            }

            ViewBag.Tijdstip = resultaat;

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
    }
}
