using loadsheddingapp.Models;
using loadsheddingapp.Repository;
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

        public IActionResult CreateJoke()
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