using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using GameOfDrones.Entity;

namespace GameOfDrones.WebApi.Controllers
{
    public class GameController : ApiController
    {
        private Model db = new Model();

        public async Task<IHttpActionResult> PostGame([FromBody]Models.Request[] request)
        {
            if (!request.Any())
            {
                return BadRequest("No Data within the request");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = new Game();
            var dbPlayers = new List<Player>();

            foreach (var player in request)
            {
                var round = new Round();

                var dbPlayer = db.Players.Where(r => r.Name == player.Name).FirstOrDefault();

                if (dbPlayer == null)
                {
                    var newPlayer = new Player { Name = player.Name };
                    newPlayer.Rounds.Add(round);
                    dbPlayers.Add(newPlayer);
                    db.Players.Add(newPlayer);
                }
                else
                {
                    dbPlayer.Rounds.Add(round);
                    dbPlayers.Add(dbPlayer);
                }

                game.Rounds.Add(round);
            }

            db.Games.Add(game);
            await db.SaveChangesAsync();

            var t = dbPlayers.Select(r => new Models.Request { Id = r.IdPlayer, Name = r.Name }).ToArray();

            return Ok(t);
        }
    }
}