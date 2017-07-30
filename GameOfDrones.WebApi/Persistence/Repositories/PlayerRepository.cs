using GameOfDrones.Entity;
using GameOfDrones.WebApi.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfDrones.WebApi.Persistence.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private Model _context;
        public Player GetPlayerById(int playerId)
        {
            return _context.Players.Find(playerId);
        }

        public Player GetPlayerByName(string name)
        {
            return _context.Players.Where(r => r.Name == name).FirstOrDefault();
        }

        public void Insert(Player player)
        {
            _context.Players.Add(player);
        }

        public PlayerRepository(Model context)
        {
            _context = context;
        }
    }
}