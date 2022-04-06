using LezeckyDenik.Models;

namespace LezeckyDenik.Repository.IRepository
{
    public interface IArticleRepository : IRepository<Article>
    {
        void Update(Article obj);
    }
}
