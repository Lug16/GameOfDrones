using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfDrones.Entity.Extensions
{
    public static class PlayerExtension
    {
        public static void SetAsWinner(this Player player)
        {
            player.Victories = player.Victories.HasValue ? ++player.Victories : 1;
            player.GamesPlayed = player.GamesPlayed.HasValue ? ++player.GamesPlayed : 1;
        }
    }
}
