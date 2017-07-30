using GameOfDrones.Entity;
using GameOfDrones.WebApi.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfDrones.WebApi.Persistence.Repositories
{
    public class GameRepository : IGameRepository
    {
        private Model _context;

        public int GetNumberOfVictoriesByPlayer(int idGame,int idPlayer)
        {
            return _context.Games.Find(idGame).Rounds.Where(r => r.IdPlayer == idPlayer && r.Won).Count();
        }

        public GameRepository(Model context)
        {
            _context = context;
        }

        public void Insert(Game game)
        {
            _context.Games.Add(game);
        }

        public Game GetGameById(int idGame)
        {
            return _context.Games.Find(idGame);
        }
    }
}