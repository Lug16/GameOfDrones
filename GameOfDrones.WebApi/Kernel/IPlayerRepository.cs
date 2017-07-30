using GameOfDrones.Entity;
using System.Collections.Generic;

namespace GameOfDrones.WebApi.Kernel
{
    public interface IPlayerRepository
    {
        Player GetPlayerById(int playerId);
        Player GetPlayerByName(string name);
        void Insert(Player player);
        IEnumerable<Player> GetLeaderboard();
    }
}