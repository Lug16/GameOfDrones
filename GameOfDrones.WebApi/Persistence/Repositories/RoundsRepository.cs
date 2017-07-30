using GameOfDrones.Entity;
using GameOfDrones.WebApi.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfDrones.WebApi.Persistence.Repositories
{
    public class RoundsRepository : IRoundsRepository
    {
        private Model _context;

        public Round GetLastByPlayer(int playerId)
        {
            return _context.Rounds.Where(r => r.IdPlayer == playerId).OrderByDescending(r => r.CreationDate).FirstOrDefault();
        }

        public void Insert(Round round)
        {
            _context.Rounds.Add(round);
        }

        public Round GetPreviousRound(int idGame)
        {
            return _context.Rounds.Where(r => r.IdGame == idGame).OrderByDescending(r => r.CreationDate).First();
        }
        public RoundsRepository(Model context)
        {
            _context = context;
        }
    }
}