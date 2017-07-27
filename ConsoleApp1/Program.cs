using GameOfDrones.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new Model();

            var game = new Game();

            var player = new Player { Name = "Lug", GamesPlayed = 1, Victories = 100 };
            var player2 = new Player { Name = "Zum", GamesPlayed = 1, Victories = 100 };
            //player.Games.Add(game);
            

            var round = new Round { HandShape = 1, Won = true };
            player.Rounds.Add(round);

            var round2 = new Round { HandShape = 1, Won = false };
            player2.Rounds.Add(round2);

            game.Rounds.Add(round);
            game.Rounds.Add(round2);

            context.Games.Add(game);
            context.Players.Add(player);
            context.Players.Add(player2);

            context.SaveChanges();
        }
    }
}
