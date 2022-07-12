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

        [HttpGet]
        [Authorize(Roles = "user")]
        public IActionResult CreateJoke()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        public IActionResult CreateJoke(string joke)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == "http://username/name")?.Value;

            if (userName == null || String.IsNullOrEmpty(joke)) {
                return RedirectToAction("Error");
            }

            Task<Joke> task = _repository.AddAsync(new Joke(userName, joke, DateTime.Now, false));
            task.Wait();

            return RedirectToAction("Index");
        }

    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}