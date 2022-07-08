using loadsheddingapp.Models;
using loadsheddingapp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace loadsheddingapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJokeRepository _repository;

        public HomeController(ILogger<HomeController> logger, IJokeRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.GetAllApproved().Result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateJoke()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateJoke(string joke)
        {
            Task<Joke> task = _repository.AddAsync(new Joke("anon", joke, DateTime.Now, true));
            task.Wait();
            Joke _joke = task.Result;

            if (_joke == null) {
                throw new Exception();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult admin() {
            return new ObjectResult("hello world");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}