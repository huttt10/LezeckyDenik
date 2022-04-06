using LezeckyDenik.Data;
using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;

namespace LezeckyDenik.Repository
{
    public class TrainingRepository : Repository<Training>, ITrainingRepository
    {
        private ApplicationDbContext _db;

        public TrainingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Training obj)
        {
            _db.Trainings.Update(obj);
        }
    }
}
