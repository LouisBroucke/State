using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using State.Models;

namespace State.Controllers
{
    public class HomeController : Controller
    {
        private Counter _counter;

        public HomeController(Counter counter)
        {
            _counter = counter;
        }

        public IActionResult Index()
        {
            //string resultaat = "Dit is jouw eerste bezoek";

            ////zijn er cookies?
            //if (Request.Cookies != null)
            //{
            //    //is er een cookie met de naam lastvisit?
            //    if (Request.Cookies["lastvisit"] != null)
            //    {
            //        //dan lezen we het resultaat uit de cookie
            //        resultaat = "Welkom terug, je laatste bezoek was op " + Request.Cookies["lastvisit"];
            //    }

            //    //we slaan het huidige tijdstip op als laatste bezoek
            //    string laatsteBezoek = DateTime.Now.ToString();
            //    CookieOptions option = new CookieOptions();
            //    option.Expires = DateTime.Now.AddDays(365);
            //    Response.Cookies.Append("lastvisit", laatsteBezoek, option);                
            //}

            //ViewBag.Tijdstip = resultaat;

            var aantalBezoeken = HttpContext.Session.GetInt32("aantalBezoeken");

            if (aantalBezoeken == null)
                HttpContext.Session.SetInt32("aantalBezoeken", 1);
            else
                HttpContext.Session.SetInt32("aantalBezoeken", (int)aantalBezoeken + 1);

            ViewBag.aantalBezoeken = HttpContext.Session.GetInt32("aantalBezoeken");

            var laatsteBezoek = HttpContext.Session.GetString("lastvisit");
            if (string.IsNullOrEmpty(laatsteBezoek))
            {
                ViewBag.lastvisit = "geen";
            }
            else
            {
                ViewBag.lastvisit = JsonConvert.DeserializeObject<DateTime>(laatsteBezoek);
            }
            var geserializeerdeDatum = JsonConvert.SerializeObject(DateTime.Now);
            HttpContext.Session.SetString("lastvisit", geserializeerdeDatum);

            _counter.TotaalAantalBezoeken += 1;
            ViewBag.totaalAantalBezoeken = _counter.TotaalAantalBezoeken;

            return View();
        }

        public IActionResult Wissen()
        {
            //if (Request.Cookies != null)
            //{
            //    Response.Cookies.Delete("lastvisit");
            //}
            //if (HttpContext.Session.GetInt32("aantalBezoeken") != null)
            //{
            //    HttpContext.Session.Remove("aantalBezoeken");
            //}
            //if (HttpContext.Session.GetString("lastvisit") != null)
            //{
            //    HttpContext.Session.Remove("lastvisit");
            //}

            HttpContext.Session.Clear();
            _counter.TotaalAantalBezoeken = 0;

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
