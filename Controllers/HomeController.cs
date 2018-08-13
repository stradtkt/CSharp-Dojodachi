using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            int? Happiness = HttpContext.Session.GetInt32("Happiness");
            int? Meals = HttpContext.Session.GetInt32("Meals");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            if(Fullness == null || Happiness == null | Meals == null || Energy == null)
            {
                HttpContext.Session.SetInt32("Fullness", 20);
                HttpContext.Session.SetInt32("Happiness", 20);
                HttpContext.Session.SetInt32("Meals", 3);
                HttpContext.Session.SetInt32("Energy", 50);
            }
            ViewBag.Fullness = HttpContext.Session.GetInt32("Fullness");
            ViewBag.Happiness = HttpContext.Session.GetInt32("Happiness");
            ViewBag.Meals = HttpContext.Session.GetInt32("Meals");
            ViewBag.Energy = HttpContext.Session.GetInt32("Energy");
            if(Fullness > 100 && Happiness > 100 && Energy > 100)
            {
                TempData["action"] = "Congrats! You have won!";
            }
            if(Fullness == 0 || Happiness == 0)
            {
                TempData["action"] = "Your Dojodachi has passed away";
            }
            ViewBag.action = TempData["action"];
            return View();
        }
        [HttpGet("feed")]
        public IActionResult Feed()
        {
            Random rand = new Random();
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            int? Meals = HttpContext.Session.GetInt32("Meals");
            if(Meals == 0)
            {
                TempData["action"] = "You cannot feed your Dojodachi if you do not have meals.  You have lost.";
            }
            int randomRaise = rand.Next(5, 10);
            if(randomRaise <= 7)
            {
                TempData["action"] = "Your Dojodatchi doesn't like your poor cooking";
            }
            else
            {
                TempData["action"] = "Your Dojodachi enjoys the meal you gave it";
            }
            Meals-=1;
            HttpContext.Session.SetInt32("Fullness", (int)Fullness);
            HttpContext.Session.SetInt32("Meals", (int)Meals);
            return RedirectToAction("Index");
        }
        [HttpGet("working")]
        public IActionResult Work()
        {
            Random rand = new Random();
            int? Meals = HttpContext.Session.GetInt32("Meals");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            int addedMeal = rand.Next(1,3);
            Energy-=5;
            Meals+=addedMeal;
            if(addedMeal == 1)
            {
                TempData["action"] = "Your Dojodachi just worked a normal shift";
            }
            else 
            {
                TempData["action"] = "Your Dojodachi has worked some overtime";
            }
            HttpContext.Session.SetInt32("Meals", (int)Meals);
            HttpContext.Session.SetInt32("Energy", (int)Energy);
            return RedirectToAction("Index");
        }
        [HttpGet("play")]
        public IActionResult Play()
        {
            Random rand = new Random();
            int? Happiness = HttpContext.Session.GetInt32("Happiness");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            int randomRaise = rand.Next(5,10);
            if(randomRaise == 5)
            {
                TempData["action"] = "Your Dojodachi is not having fun";
            }
            else 
            {
                TempData["action"] = "Your Dojodachi is having loads of fun";
            }
            Energy-=5;
            Happiness+=randomRaise;
            TempData["action"] = $"You gained {randomRaise} happiness";
            HttpContext.Session.SetInt32("Happiness", (int)Happiness);
            HttpContext.Session.SetInt32("Energy", (int)Energy);
            return RedirectToAction("Index");
        }
        [HttpGet("sleeping")]
        public IActionResult Sleep()
        {
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            int? Happiness = HttpContext.Session.GetInt32("Happiness");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            Energy+=15;
            Happiness-=5;
            Fullness-=5;
            HttpContext.Session.SetInt32("Happiness", (int)Happiness);
            HttpContext.Session.SetInt32("Energy", (int)Energy);
            HttpContext.Session.SetInt32("Fullness", (int)Fullness);
            TempData["action"] = $"You wake up well rested and gained {Energy} but lost 5 in {Happiness} and {Fullness}.";

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
