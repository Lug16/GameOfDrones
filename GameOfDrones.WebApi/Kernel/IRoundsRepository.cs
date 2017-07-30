using GameOfDrones.Entity;

namespace GameOfDrones.WebApi.Kernel
{
    public interface IRoundsRepository
    {
        Round GetLastByPlayer(int playerId);
        Round GetPreviousRound(int idGame);
        void Insert(Round round);
    }
}