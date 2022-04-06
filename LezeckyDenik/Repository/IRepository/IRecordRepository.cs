using LezeckyDenik.Models;

namespace LezeckyDenik.Repository.IRepository
{
    public interface IRecordRepository : IRepository<Record>
    {
        void Update(Record obj);
    }
}
