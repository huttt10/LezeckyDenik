namespace LezeckyDenik.Models
{

    public class RecordViewModel
    {
        public IEnumerable<Record> Record { get; set; }
        public Dictionary<string, int> DifficultyAndCounts { get; set; }
        public Dictionary<string, int> DatesAndCounts { get; set; }
        public IEnumerable<StatisticData> Statistics { get; set; }
    }
}
