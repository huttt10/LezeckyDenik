namespace LezeckyDenik.Models
{

    public class GlobalViewModel
    {
        public IEnumerable<StatisticData> StatisticsData { get; set; }
        public Dictionary<string, int> AvarageAndCounts { get; set; }
        public Dictionary<string, int> DatesAndCounts { get; set; }

    }
}
