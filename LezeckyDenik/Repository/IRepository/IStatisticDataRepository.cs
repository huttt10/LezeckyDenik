using LezeckyDenik.Models;

namespace LezeckyDenik.Repository.IRepository
{
    public interface IStatisticDataRepository : IRepository<StatisticData>
    {
        void Update(StatisticData obj);
    }
}
