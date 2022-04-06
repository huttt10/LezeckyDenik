using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;
using LezeckyDenik.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LezeckyDenik.Controllers
{
    public class AddRecordController : Controller
    {        
        private readonly IUnitOfWork _unitOfWork;

        public AddRecordController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            Record record = new Record();
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Record obj)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            obj.UserId = claim.Value;

            _unitOfWork.Record.Add(obj);
            _unitOfWork.Save();

            UpdateStatistic(obj.UserId);
            
            return RedirectToAction("Index", "Record");
        }

        private void UpdateStatistic(string Id)
        {
               
            var data = _unitOfWork.StatisticData.GetFirstOrDefault(x => x.UserId == Id);
            if(data == null)
            {
                data = new StatisticData();
                data.UserId = Id;

                var average = Math.Round(_unitOfWork.Record.GetAll().Where(x => x.UserId == Id).Select(x => x.ModifyDifficulty).Average());
                var max = _unitOfWork.Record.GetAll().Where(x => x.UserId == Id).Select(x => x.ModifyDifficulty).Max();
                var count = _unitOfWork.Record.GetAll().Where(x => x.UserId == Id).Select(x => x.ModifyDifficulty).Count();

                int averageInt = Convert.ToInt32(average);

                data.Average = ConverterDifficulty.GetStringFromDifficultyInt(averageInt); 
                data.Highest = ConverterDifficulty.GetStringFromDifficultyInt(max);
                data.Count = count;

                _unitOfWork.StatisticData.Add(data);
            }
            else
            {
                var average = Math.Round(_unitOfWork.Record.GetAll().Where(x => x.UserId == Id).Select(x => x.ModifyDifficulty).Average());
                var max = _unitOfWork.Record.GetAll().Where(x => x.UserId == Id).Select(x => x.ModifyDifficulty).Max();
                var count = _unitOfWork.Record.GetAll().Where(x => x.UserId == Id).Select(x => x.ModifyDifficulty).Count();

                int averageInt = Convert.ToInt32(average);

                data.Average = ConverterDifficulty.GetStringFromDifficultyInt(averageInt);
                data.Highest = ConverterDifficulty.GetStringFromDifficultyInt(max);
                data.Count = count;

                _unitOfWork.StatisticData.Update(data);
            }
            _unitOfWork.Save();
        }
    }
}
