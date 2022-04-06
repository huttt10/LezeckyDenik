using LezeckyDenik.Models.ViewModels;
using LezeckyDenik.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LezeckyDenik.Controllers
{
    public class OverviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OverviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var records = _unitOfWork.Record.GetAll().Where(x => x.UserId == claim.Value && x.DateRecord.Month == DateTime.Now.Month);
            var training = _unitOfWork.Training.GetAll().Where(x => x.UserId == claim.Value && x.Date.Day == DateTime.Now.Day);
            var goal = _unitOfWork.Goal.GetAll().Where(x => x.UserId == claim.Value && x.Month.Month == DateTime.Now.Month);
            var doneGoals = _unitOfWork.Goal.GetAll().Where(x => x.UserId == claim.Value && x.Achieved == true).Count();
            var allGoals = _unitOfWork.Goal.GetAll().Where(x => x.UserId == claim.Value).Count();

            OverviewViewModel viewModel = new OverviewViewModel();
            viewModel.Records = records;
            viewModel.Training = training;
            viewModel.Goal = goal;
            viewModel.DoneGoals = doneGoals;
            viewModel.AllGoals = allGoals;
            viewModel.GoalMonthAndCount = GetDataForLineChart();

            return View(viewModel);
        }

        private Dictionary<string, int> GetDataForLineChart()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var columssOfDate = _unitOfWork.Goal.GetAll().Where(x => x.UserId == claim.Value && x.Achieved == true).Select(x => x.Month).OrderBy(x => x.Month);

            if(!columssOfDate.Any())
            {
                return null;
            }

            List<string> ListOfDates = new List<string>();

            foreach (var item in columssOfDate)
            {
                ListOfDates.Add(item.ToString("MMMM"));
            }

            Dictionary<string, int> DatesAndGoals = new Dictionary<string, int>();

            foreach (var column in ListOfDates)
            {
                if (DatesAndGoals.ContainsKey(column))
                {
                    DatesAndGoals[column] = DatesAndGoals[column] + 1;
                }
                else
                {
                    DatesAndGoals.Add(column, 1);
                }
            }
            return DatesAndGoals;
        }
    }
}
