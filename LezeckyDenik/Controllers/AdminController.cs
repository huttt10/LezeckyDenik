using LezeckyDenik.Models;
using LezeckyDenik.Models.ViewModels;
using LezeckyDenik.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LezeckyDenik.Controllers
{
    [Authorize(Roles = Role.RoleAdmin + "," + Role.RoleSuperAdmin + "," + Role.RoleEditor)]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = Role.RoleAdmin + "," + Role.RoleSuperAdmin)]
        public IActionResult EditMainPage(string title)
        {
            var mainPage = _unitOfWork.MainPage.GetFirstOrDefault(x => x.Title == title);

            if (mainPage == null)
            {
                mainPage = new MainPage();
                mainPage.Title = title;
                _unitOfWork.MainPage.Add(mainPage);
                _unitOfWork.Save();
            }

            return View(mainPage);
       }

        [HttpPost]
        public IActionResult SaveMainPage(string title, string content)
        {
            var mainPage = _unitOfWork.MainPage.GetFirstOrDefault(x => x.Title == title);

            mainPage.Content = content;

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        //Otevře panel
        public IActionResult CreateArticle()
        {
            var article = new Article();
            
            article.Title = "Zde napište název článku";
            article.Content = "Zde napište obsah článku";

            return View(article);                        
        }

        
        [HttpPost]
        public IActionResult SaveArticle(int? id, string? title, string? content, bool published)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(title == null || content == null)
            {
                Article errorArticle = new Article();
                if(title == null)                
                    errorArticle.Title = "Cbybí název článku!";
                if (content == null)
                    errorArticle.Content = "Chybí obsah článku!";
                return View("CreateArticle", errorArticle);
            }

            if (id == 0 || id == null)
            {
                Article createdNewArticle = new Article();

                createdNewArticle.Title = title;
                createdNewArticle.Content = content;
                createdNewArticle.UserId = claim.Value;
                createdNewArticle.Published = published;
                _unitOfWork.Article.Add(createdNewArticle);
            }
            else
            {
                var existsArticle = _unitOfWork.Article.GetFirstOrDefault(x => x.Id == id);

                existsArticle.Title = title;
                existsArticle.Content = content;
                existsArticle.Published = published;
                _unitOfWork.Article.Update(existsArticle);
            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
        
        public IActionResult EditArticle(int? id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var article = _unitOfWork.Article.GetFirstOrDefault(x => x.Id == id && x.UserId == claim.Value);


            
            return View("CreateArticle", article);
        }

        public IActionResult DeleteArticle(int? id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var obj = _unitOfWork.Article.GetFirstOrDefault(x => x.Id == id && x.UserId == claim.Value);

            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Article.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("ShowArticle");
        }

        //zobrazí seznam napsaných článků uživatelem
        public IActionResult ShowArticle(string search)
        {
            ViewData["CurrentFilter"] = search;
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);


            if (!String.IsNullOrEmpty(search))
            {
                IEnumerable<Article> obj = _unitOfWork.Article.GetAll().Where(x => x.UserId == claim.Value && x.Title.Contains(search));
                return View(obj);
            }
            else
            {
                IEnumerable<Article> obj = _unitOfWork.Article.GetAll().Where(x => x.UserId == claim.Value);
                return View(obj);
            }
        }

        //ukáže list všech uživatelů
        [Authorize(Roles = Role.RoleAdmin + "," + Role.RoleSuperAdmin)]
        public async Task<IActionResult> ShowRoles(string search)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ViewData["CurrentFilter"] = search;

            List<ApplicationUser> users = new List<ApplicationUser>();

            if (!String.IsNullOrEmpty(search))
            {
                users = _userManager.Users.Where(x => x.UserName.Contains(search)).ToList();                
            }
            else
            {
                users = _userManager.Users.ToList();
            }
            users.Remove(users.Find(x => x.Id == claim.Value));

            var userRolesViewModel = new List<ShowRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new ShowRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.UserName = user.Email;
                thisViewModel.Roles = await _userManager.GetRolesAsync(user);
                
                if(!thisViewModel.Roles.Contains(Role.RoleSuperAdmin) && User.IsInRole(Role.RoleAdmin))
                {
                  userRolesViewModel.Add(thisViewModel);
                }
                else if (User.IsInRole(Role.RoleSuperAdmin))
                {
                    userRolesViewModel.Add(thisViewModel);
                }
            }
            return View(userRolesViewModel);
        }

        public async Task<IActionResult> ChangeRole(string? userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return View();
            var role = await _userManager.GetRolesAsync(user);


            ShowRolesViewModel thisViewModel = new ShowRolesViewModel();
            thisViewModel.UserName = user.UserName;
            thisViewModel.Roles = role;

            if (User.IsInRole(Role.RoleAdmin))
            {
                thisViewModel.ChooseRole.Remove(thisViewModel.ChooseRole.First());
            }


            return View(thisViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string? userId, string? ChooseRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var oldRoleId = await _userManager.GetRolesAsync(user);
            string oldRoleStringId = oldRoleId[0];
            
            if (oldRoleStringId != ChooseRole)
            {
                await _userManager.RemoveFromRoleAsync(user, oldRoleStringId);
                await _userManager.AddToRoleAsync(user, ChooseRole);
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("ShowRoles");
        }

        [Authorize(Roles = Role.RoleSuperAdmin)]
        public IActionResult DeleteUser(string? userId)
        {
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            _unitOfWork.ApplicationUser.Remove(user);
            _unitOfWork.Save();
            return RedirectToAction("ShowRoles");
        }


    }
}

