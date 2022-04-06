using LezeckyDenik.Data;
using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;

namespace LezeckyDenik.Repository
{
    public class StatisticDataRepository : Repository<StatisticData>, IStatisticDataRepository
    {
        private ApplicationDbContext _db;

        public StatisticDataRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(StatisticData obj)
        {
            _db.StatisticsData.Update(obj);
        }
    }
}
