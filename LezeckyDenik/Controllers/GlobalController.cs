using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;


namespace LezeckyDenik.Controllers
{
    public class GlobalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GlobalController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index(string sortOrder, string search)
        {
            ViewData["CurrentFilter"] = search;

            GlobalViewModel modelView = new GlobalViewModel();
            /*
            var joinUserAndStatisticData = _unitOfWork.StatisticData.GetAll().Join(_unitOfWork.ApplicationUser.GetAll(),
                user => user.UserId, applicationUser => applicationUser.Id,
                (a, b) => new StatisticData { User = b, Average = a.Average, Highest = a.Highest, Count = a.Count});
            */
            var joinUserAndStatisticData = _unitOfWork.StatisticData.GetAll(includeProperties: "User");

            if (!String.IsNullOrEmpty(search))
            {
                joinUserAndStatisticData = joinUserAndStatisticData.Where(x => x.User.UserName.Contains(search));
            }

            switch (sortOrder)
            {
                case "OrderByAverage":
                    joinUserAndStatisticData = joinUserAndStatisticData.OrderByDescending(x => x.Average);
                    break;
                case "OrderByHighest":
                    joinUserAndStatisticData = joinUserAndStatisticData.OrderByDescending(x => x.Highest);
                    break;
                case "OrderByCount":
                    joinUserAndStatisticData = joinUserAndStatisticData.OrderByDescending(x => x.Count);
                    break;
                default:
                    joinUserAndStatisticData = joinUserAndStatisticData.OrderByDescending(x => x.Average);
                    break;
            }


            modelView.StatisticsData = joinUserAndStatisticData;
            modelView.AvarageAndCounts = GetAvarageAndCounts();
            modelView.DatesAndCounts = GetDatesAndCounts();

            return View(modelView);
        }

        private Dictionary<string, int> GetAvarageAndCounts()
        {
            var columnsOfAvarage = _unitOfWork.StatisticData.GetAll().Select(x => x.Average);

            Dictionary<string, int> AvarageAndCounts = new Dictionary<string, int>();

            foreach (var column in columnsOfAvarage)
            {
                if (AvarageAndCounts.ContainsKey(column.ToString()))
                {
                    AvarageAndCounts[column.ToString()] = AvarageAndCounts[column.ToString()] + 1;
                }
                else
                {
                    AvarageAndCounts.Add(column.ToString(), 1);
                }
            }
            return AvarageAndCounts;
        }

        private Dictionary<string, int> GetDatesAndCounts()
        {
            var columssOfDate = _unitOfWork.Record.GetAll().OrderBy(x => x.DateRecord).Select(x => x.DateRecord);

            List<string> ListOfDates = new List<string>();

            foreach (var item in columssOfDate)
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
