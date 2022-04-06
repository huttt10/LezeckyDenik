using LezeckyDenik.Data;
using LezeckyDenik.Models;
using LezeckyDenik.Repository.IRepository;

namespace LezeckyDenik.Repository
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private ApplicationDbContext _db;

        public ArticleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Article obj)
        {
            _db.Articles.Update(obj);
        }
    }
}
