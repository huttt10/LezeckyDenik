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
using LezeckyDenik.Repository;
using LezeckyDenik.Repository.IRepository;

namespace LezeckyDenik.Controllers
{
    public class TrainingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Training
        public IActionResult Index(string sortOrderDate, string sortOrderIsDone, string search, DateTime searchDate)
        {
            ViewData["CurrentFilter"] = search;

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var training = _unitOfWork.Training.GetAll().Where(x => x.UserId == claim.Value);

            if (!String.IsNullOrEmpty(search))
            {
                training = training.Where(x => x.Description.Contains(search));
            }
            else if(searchDate != DateTime.MinValue)
            {
                training = training.Where(x => x.Date == searchDate);                
            }

            switch (sortOrderDate)
            {
                case "OrderByDateYear":
                    training = training.Where(x => x.Date.Year == DateTime.Now.Year).OrderBy(x => x.Date);
                    break;
                case "OrderByIsDateMonth":
                    training = training.Where(x => x.Date.Month == DateTime.Now.Month).OrderByDescending(x => x.Date.Month);
                    break;
                case "OrderByIsDateOlder":
                    training = training.OrderBy(x => x.Date);
                    break;
                default:
                    training = training.OrderByDescending(x => x.Date);
                    break;
            }

            switch (sortOrderIsDone)
            {
                case "OrderByIsDoneTrue":
                    training = training.Where(x => x.IsDone == true);
                    break;
                case "OrderByIsDoneFalse":
                    training = training.Where(x => x.IsDone == false);
                    break;                
            }

            return View(training);
        }

        // GET: Training/Details/5
        public IActionResult Details(int? id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var training = _unitOfWork.Training.GetFirstOrDefault(x => x.Id == id && x.UserId == claim.Value);

            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: Training/Create
        public IActionResult Create()
        {
            Training training = new Training();
            return View(training);
        }

        // POST: Training/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Date,Focus,Description,Time,Note,IsDone,UserId")] Training training)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                training.UserId = claim.Value;
                _unitOfWork.Training.Add(training);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            
            return View(training);
        }

        // GET: Training/Edit/5
        public IActionResult Edit(int? id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var training = _unitOfWork.Training.GetFirstOrDefault(x => x.Id == id && x.UserId == claim.Value);

            if (training == null)
            {
                return NotFound();
            }
            
            return View(training);
        }

        // POST: Training/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Date,Focus,Description,Time,Note,IsDone,UserId")] Training training)
        {
            if (id != training.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                try
                {
                    training.UserId = claim.Value;
                    _unitOfWork.Training.Update(training);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(training);
        }

        // GET: Training/Delete/5
        public IActionResult Delete(int? id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var training = _unitOfWork.Training.GetFirstOrDefault(x => x.Id == id && x.UserId == claim.Value);

            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Training/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var training = _unitOfWork.Training.GetFirstOrDefault(x => x.Id == id && x.UserId == claim.Value);
            _unitOfWork.Training.Remove(training);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
            var v = _unitOfWork.Training.GetFirstOrDefault(x => x.Id == id);
            return v.Equals(0);                
        }
    }
}
