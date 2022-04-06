using LezeckyDenik.Models;

namespace LezeckyDenik.Repository.IRepository
{
    public interface IGoalRepository : IRepository<Goal>
    {
        void Update(Goal obj);
    }
}
