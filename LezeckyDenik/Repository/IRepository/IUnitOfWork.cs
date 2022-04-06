using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LezeckyDenik.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUser {  get; }
        IRecordRepository Record { get; }
        IMainPageRepository MainPage { get; }
        IArticleRepository Article { get; }
        IStatisticDataRepository StatisticData { get; }
        IGoalRepository Goal { get; }
        ITrainingRepository Training { get; }

        void Save();
    }
}
