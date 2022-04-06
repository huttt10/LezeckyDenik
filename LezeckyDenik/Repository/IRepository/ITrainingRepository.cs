using LezeckyDenik.Models;

namespace LezeckyDenik.Repository.IRepository
{
    public interface ITrainingRepository : IRepository<Training>
    {
        void Update(Training obj);
    }
}
