using LezeckyDenik.Data;
using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;

namespace LezeckyDenik.Repository
{
    public class GoalRepository : Repository<Goal>, IGoalRepository
    {
        private ApplicationDbContext _db;

        public GoalRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Goal obj)
        {
            _db.Goals.Update(obj);
        }
    }
}
