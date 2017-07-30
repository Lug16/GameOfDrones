using GameOfDrones.Entity;
using GameOfDrones.WebApi.Kernel;
using GameOfDrones.WebApi.Persistence.Repositories;

namespace GameOfDrones.WebApi.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Model _context;

        public IPlayerRepository Players { get; private set; }

        public IRoundsRepository Rounds { get; private set; }

        public IGameRepository Games { get; private set; }

        public UnitOfWork()
        {
            _context = new Model();
            Players = new PlayerRepository(_context);
            Rounds = new RoundsRepository(_context);
            Games = new GameRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}