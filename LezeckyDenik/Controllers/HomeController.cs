using LezeckyDenik.Models;
using LezeckyDenik.Models.ViewModels;
using LezeckyDenik.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Dynamic;

namespace LezeckyDenik.Controllers
{    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {            
            MainPageViewModel viewModel = new MainPageViewModel();
            
            var mainPage = _unitOfWork.MainPage.GetFirstOrDefault(x => x.Title == "Home");
            var articleName = _unitOfWork.Article.GetAll(includeProperties: "User").Where(x => x.Published);
            articleName = articleName.TakeLast(3).Reverse();

            if (mainPage == null)
            {
                mainPage = new MainPage();
                mainPage.Title = "Home";
                mainPage.Content = " ";
                viewModel.MainPage = mainPage;
            }
            else
            {
                viewModel.MainPage = mainPage;
            }

            viewModel.Articles = articleName;

            return View(viewModel);
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


        public IActionResult Article(string title)
        {
            var article = _unitOfWork.Article.GetFirstOrDefault(x => x.Title == title, includeProperties: "User");
            return View(article);
        }

        public IActionResult ShowAllArticle()
        {
            var articles = _unitOfWork.Article.GetAll(includeProperties: "User").Where(x => x.Published);
            return View(articles);
        }
    }
}