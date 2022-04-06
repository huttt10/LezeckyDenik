using LezeckyDenik.Data;
using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;

namespace LezeckyDenik.Repository
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        private ApplicationDbContext _db;

        public RecordRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Record obj)
        {
            _db.Records.Update(obj);
        }
    }
}
