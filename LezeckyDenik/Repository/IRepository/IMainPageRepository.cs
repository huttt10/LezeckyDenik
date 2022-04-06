using LezeckyDenik.Models;

namespace LezeckyDenik.Repository.IRepository
{
    public interface IMainPageRepository : IRepository<MainPage>
    {
        void Update(MainPage obj);
    }
}
