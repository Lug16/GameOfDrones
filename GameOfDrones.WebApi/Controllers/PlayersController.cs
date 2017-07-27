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
    public class PlayersController : ApiController
    {
        private Model db = new Model();

        // GET: api/Players
        public IEnumerable<Models.Player> GetPlayers()
        {
            var model = db.Players.Select(r => new Models.Player() { Id = r.IdPlayer, Name = r.Name });
            return model;
        }

        // GET: api/Players/5
        [ResponseType(typeof(Player))]
        public async Task<IHttpActionResult> GetPlayer(int id)
        {
            Player player = await db.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        // POST: api/Players
        //[ResponseType(typeof(Models.Player[]))]
        public async Task<IHttpActionResult> PostPlayers([FromBody]Models.Player[] players)
        {
            if (!players.Any())
            {
                return BadRequest("No players to save");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbPlayers = new List<Player>();

            foreach (var player in players)
            {
                var dbPlayer = db.Players.Where(r => r.Name == player.Name).FirstOrDefault();

                if (dbPlayer == null)
                {
                    var newPlayer = new Player { Name = player.Name };
                    dbPlayers.Add(newPlayer);
                    db.Players.Add(newPlayer);
                }
                else
                {
                    dbPlayers.Add(dbPlayer);
                }
            }

            await db.SaveChangesAsync();

            var t = dbPlayers.Select(r => new Models.Player { Id = r.IdPlayer, Name = r.Name }).ToArray();

            return Ok(t);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}