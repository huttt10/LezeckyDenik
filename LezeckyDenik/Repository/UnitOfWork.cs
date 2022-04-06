using LezeckyDenik.Data;
using LezeckyDenik.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LezeckyDenik.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;

            ApplicationUser = new ApplicationUserRepository(_db);
            Record = new RecordRepository(_db);
            MainPage = new MainPageRepository(_db);
            Article = new ArticleRepository(_db);
            StatisticData = new StatisticDataRepository(_db);
            Goal = new GoalRepository(_db);
            Training = new TrainingRepository(_db);
        }

        public IApplicationUserRepository ApplicationUser {  get; private set; }

        public IRecordRepository Record { get; private set; }

        public IMainPageRepository MainPage { get; private set; }

        public IArticleRepository Article { get; private set; }

        public IStatisticDataRepository StatisticData { get; private set; }

        public IGoalRepository Goal { get; private set; }

        public ITrainingRepository Training { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
