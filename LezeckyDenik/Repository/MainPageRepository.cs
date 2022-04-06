using LezeckyDenik.Data;
using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;

namespace LezeckyDenik.Repository
{
    public class MainPageRepository : Repository<MainPage>, IMainPageRepository
    {

        private ApplicationDbContext _db;

        public MainPageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MainPage obj)
        {
            _db.MainPages.Update(obj);
        }
    }
}
