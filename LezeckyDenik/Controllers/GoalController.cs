#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LezeckyDenik.Data;
using LezeckyDenik.Models;
using System.Security.Claims;
using LezeckyDenik.Repository.IRepository;
using LezeckyDenik.Utility;

namespace LezeckyDenik.Controllers
{
    public class GoalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GoalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Goal
        public IActionResult Index(string? sortOrder)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<Goal> goal = new List<Goal>();

            if(sortOrder != null)
            {
                goal = _unitOfWork.Goal.GetAll().Where(x => x.UserId == claim.Value).OrderBy(x => x.Month).ToList();
                ViewBag.All = true;                
            }
            else
            {
                goal = _unitOfWork.Goal.GetAll().Where(x => x.UserId == claim.Value && x.Month.Year == DateTime.Now.Year).OrderBy(x => x.Month.Month).ToList();
                if (goal.Any())
                {
                    CheckMonth(goal, claim.Value);
                    goal = _unitOfWork.Goal.GetAll().Where(x => x.UserId == claim.Value && x.Month.Year == DateTime.Now.Year).OrderBy(x => x.Month.Month).ToList();
                }
            }

            return View(goal);
        }

        public IActionResult Create()
        {
            Goal goal = new Goal();
            return View(goal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Average,Highest,Count,Month,Achieved,UserId")] Goal goal)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            goal.UserId = claim.Value;

            if (ModelState.IsValid)
            {
                _unitOfWork.Goal.Add(goal);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            
            return View(goal);
        }

        private void CheckMonth(List<Goal> goalsToCheck, string idUser)
        {
            foreach (var goalCheck in goalsToCheck)
            {
                if (goalCheck.Achieved == false && goalCheck.Month.Month < DateTime.Now.Month)
                {
                    int averageMonth = Convert.ToInt32(Math.Round(_unitOfWork.Record.GetAll().Where(x => x.UserId == idUser && x.DateRecord.Month == goalCheck.Month.Month).Select(x => x.ModifyDifficulty).Average()));
                    int highestMonth = _unitOfWork.Record.GetAll().Where(x => x.UserId == idUser && x.DateRecord.Month == goalCheck.Month.Month).Select(x => x.ModifyDifficulty).Max();
                    int countMonth = _unitOfWork.Record.GetAll().Where(x => x.UserId == idUser && x.DateRecord.Month == goalCheck.Month.Month).Select(x => x.ModifyDifficulty).Count();

                    int averageGoal = ConverterDifficulty.GetIntFromDifficultyString(goalCheck.Average);
                    int highestGoal = ConverterDifficulty.GetIntFromDifficultyString(goalCheck.Highest);

                    //To druhý musím převíst na int abych to mohl porovnat
                    if (averageMonth >= averageGoal &&
                        highestMonth >= highestGoal &&
                        countMonth >= goalCheck.Count)
                    {
                        goalCheck.Achieved = true;
                        _unitOfWork.Goal.Update(goalCheck);
                        _unitOfWork.Save();                        
                    }
                }
            }
        }
    }
}



