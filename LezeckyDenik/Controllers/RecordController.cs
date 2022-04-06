using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LezeckyDenik.Controllers
{
    public class RecordController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecordController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<Record> objCategoryList = _unitOfWork.Record.GetAll().Where(u => u.UserId == claim.Value).OrderByDescending(x => x.DateRecord);
            IEnumerable<StatisticData> statisticOfCurrentUser = _unitOfWork.StatisticData.GetAll().Where(u => u.UserId == claim.Value);
            var objPieChart = GetDataToPieChart();
            var objLineChart = GetDataToLineChart();

            RecordViewModel viewModel = new RecordViewModel();
            viewModel.Record = objCategoryList;
            viewModel.DifficultyAndCounts = objPieChart;
            viewModel.DatesAndCounts = objLineChart;
            viewModel.Statistics = statisticOfCurrentUser;

            return View(viewModel);
        }


        private Dictionary<string, int> GetDataToPieChart()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var columnsOfDifficulty = _unitOfWork.Record.GetAll().OrderBy(y => y.Difficulty).Where(x => x.UserId == claim.Value).Select(column => column.Difficulty);             

            Dictionary<string, int> DifficultyAndCounts = new Dictionary<string, int>();

            foreach(var column in columnsOfDifficulty)
            {
                if (DifficultyAndCounts.ContainsKey(column))
                {
                    DifficultyAndCounts[column] = DifficultyAndCounts[column] + 1;
                }
                else
                {
                    DifficultyAndCounts.Add(column, 1);
                }
            }            
            return DifficultyAndCounts;
        }

        private Dictionary<string, int> GetDataToLineChart()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var columnsOfDDate = _unitOfWork.Record.GetAll().OrderBy(x => x.DateRecord).Where(x => x.UserId == claim.Value).Select(c => c.DateRecord);

            List<string> ListOfDates = new List<string>();

            foreach (var item in columnsOfDDate)
            {
                ListOfDates.Add(item.ToString("d/M/yyyy"));
            }
            
            Dictionary<string, int> DatesAndCounts = new Dictionary<string, int>();

            foreach (var column in ListOfDates)
            {
                if (DatesAndCounts.ContainsKey(column))
                {
                    DatesAndCounts[column] = DatesAndCounts[column] + 1;
                }
                else
                {
                    DatesAndCounts.Add(column, 1);
                }
            }
            return DatesAndCounts;            
        }
    }
}
