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
            if(Fullness == null || Happiness == null || Meals == null || Energy == null)
            {
                TempData["img"] = "start.jpg";
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
                TempData["img"] = "winner.png";
                TempData["action"] = "Congrats! You have won!";
            }
            if(Fullness == 0 || Happiness == 0)
            {
                TempData["img"] = "rip.png";
                TempData["action"] = "Your Dojodachi has passed away";
            }
            ViewBag.action = TempData["action"];
            ViewBag.img = TempData["img"];
            return View();
        }
        [HttpGet("feed")]
        public IActionResult Feed()
        {
            Random rand = new Random();
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            int? Meals = HttpContext.Session.GetInt32("Meals");
            int randomRaise = rand.Next(5, 11);
            if(Meals == 0)
            {
                TempData["action"] = "You cannot feed your Dojodachi if you do not have meals.  You have lost.";
            }
            if(randomRaise <= 6)
            {
                Fullness+=randomRaise;
                TempData["img"] = "mad.png";
                TempData["action"] = "Your Dojodatchi doesn't like your poor cooking";
            }
            else
            {
                Fullness+=randomRaise;
                TempData["img"] = "meal-happy.png";
                TempData["action"] = "Your Dojodachi enjoys the meal you gave it";
            }
            Meals-=1;
            HttpContext.Session.SetInt32("Fullness", (int)Fullness);
            HttpContext.Session.SetInt32("Meals", (int)Meals);
            ViewBag.action = TempData["action"];
            return RedirectToAction("Index");
        }
        [HttpGet("work")]
        public IActionResult Work()
        {
            Random rand = new Random();
            int? Meals = HttpContext.Session.GetInt32("Meals");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            int addedMeal = rand.Next(1,4);
            Energy-=5;
            if(addedMeal == 1)
            {
                Meals+=addedMeal;
                TempData["img"] = "lazy-worker.png";
                TempData["action"] = $"Your Dojodachi just worked a normal shift and only got {addedMeal} meal.";
            }
            else 
            {
                Meals+=addedMeal;
                TempData["img"] = "hard-worker.png";
                TempData["action"] = $"Your Dojodachi has worked some overtime and gained {addedMeal} meals!";
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
            int sadMeter = rand.Next(1, 4);
            if(sadMeter == 2)
            {
                TempData["img"] = "sad.png";
                TempData["action"] = "Your Dojodachi is not having fun and is very sad";
            }
            else 
            {
                Happiness+=randomRaise;
                TempData["img"] = "happy.png";
                TempData["action"] = "Your Dojodachi is having loads of fun";
            }
            Energy-=5;
            TempData["action"] = $"You gained {randomRaise} happiness";
            HttpContext.Session.SetInt32("Happiness", (int)Happiness);
            HttpContext.Session.SetInt32("Energy", (int)Energy);
            return RedirectToAction("Index");
        }
        [HttpGet("sleep")]
        public IActionResult Sleep()
        {
            int? Fullness = HttpContext.Session.GetInt32("Fullness");
            int? Happiness = HttpContext.Session.GetInt32("Happiness");
            int? Energy = HttpContext.Session.GetInt32("Energy");
            Energy+=15;
            Happiness-=5;
            Fullness-=5;
            TempData["action"] = $"You wake up well rested and gained {Energy} energy but lost 5 in Happiness and Fullness.";
            TempData["img"] = "sleeping.png";
            HttpContext.Session.SetInt32("Happiness", (int)Happiness);
            HttpContext.Session.SetInt32("Energy", (int)Energy);
            HttpContext.Session.SetInt32("Fullness", (int)Fullness);
            return RedirectToAction("Index");
        }
        [HttpGet("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
