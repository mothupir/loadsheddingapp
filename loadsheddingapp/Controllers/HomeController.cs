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
        [Authorize]
        public IActionResult CreateJoke()
        {
            if (User.IsInRole("admin"))
            {

                var userName = User.Claims.FirstOrDefault(c => c.Type == "http://username/name")?.Value;
                _logger.LogInformation("Access Denied for user {user}, admin user not authorised to create jokes",userName);
                return RedirectToAction("AccessDenied", "Account");
            }

            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Admin()
        {
            return View(_repository.GetAllUnApproved().Result);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult AdminDuties(int jokeid, string rbAcceptance)
        {
            if (rbAcceptance.Equals("Accepted"))
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == "http://username/name")?.Value;
                _logger.LogInformation("Admin user {user}, approved joke with id {id}", userName, jokeid);
                _repository.ApproveJoke(jokeid);
            }
            else
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == "http://username/name")?.Value;
                _logger.LogInformation("Admin user {user}, rejected joke with id {id}", userName, jokeid);
                _repository.UnapproveJoke(jokeid);
            }

            return View("Admin", _repository.GetAllUnApproved().Result);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateJoke(string joke)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == "http://username/name")?.Value;

            if (User.IsInRole("admin")) {
                _logger.LogInformation("Access Denied for user {user}, admin user not authorised to create jokes", userName);
                return RedirectToAction("AccessDenied", "Account");
            }
          

            if (userName == null || String.IsNullOrEmpty(joke)) {
                _logger.LogInformation("Failed to create joke for user {user}", userName);
                return RedirectToAction("Error");
            }

            Task<Joke> task = _repository.AddAsync(new Joke(userName, joke, DateTime.Now, false));
            task.Wait();
            _logger.LogInformation("User {user} created a joke", userName);
            return RedirectToAction("Index");
        }

    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}