using GameOfDrones.Entity;

namespace GameOfDrones.WebApi.Kernel
{
    public interface IGameRepository
    {
        Game GetGameById(int idGame);
        int GetNumberOfVictoriesByPlayer(int idGame, int idPlayer);
        void Insert(Game game);
    }
}