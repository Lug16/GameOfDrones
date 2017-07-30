using GameOfDrones.Entity;

namespace GameOfDrones.WebApi.Kernel
{
    public interface IPlayerRepository
    {
        Player GetPlayerById(int playerId);
        Player GetPlayerByName(string name);
        void Insert(Player player);
    }
}