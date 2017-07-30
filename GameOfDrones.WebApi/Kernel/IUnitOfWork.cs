using GameOfDrones.WebApi.Kernel;

namespace GameOfDrones.WebApi.Kernel
{
    public interface IUnitOfWork
    {
        IGameRepository Games { get; }
        IPlayerRepository Players { get; }
        IRoundsRepository Rounds { get; }

        void Complete();
    }
}