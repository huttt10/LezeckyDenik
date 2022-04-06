namespace LezeckyDenik.Models.ViewModels
{
    public class OverviewViewModel
    {
        public IEnumerable<Record> Records { get; set; }

        public IEnumerable<Training> Training { get; set; }

        //public TrainingWeek TrainigWeek { get; set; }

        //public IEnumerable<Note> Notes { get; set; }

        public IEnumerable<Goal> Goal { get; set; }

        public int DoneGoals { get; set; }

        public int AllGoals { get; set; }

        public Dictionary<string, int> GoalMonthAndCount { get; set; }




    }
}
