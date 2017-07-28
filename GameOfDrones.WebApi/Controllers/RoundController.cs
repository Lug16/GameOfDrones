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
    public class RoundController : ApiController
    {
        private Model db = new Model();

        public async Task<IHttpActionResult> PostRound([FromBody]Models.Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var player = db.Players.Find(request.Id);
                var activeRound = player.Rounds.Last();

                var currentGameRounds = db.Rounds.Where(
                    r => r.HandShape > 0 &&
                    r.IdGame == activeRound.IdGame &&
                    r.IdPlayer != request.Id &&
                    r.IdRound < activeRound.IdRound);

                if (currentGameRounds.Any())
                {
                    var lastPlayerRound = currentGameRounds.OrderByDescending(r => r.CreationDate).First();

                    var round = new Round { IdGame = activeRound.IdGame, HandShape = request.Handshape };
                    var handshape = (HandShapeTypes)request.Handshape;
                    var lastPlayerHandshape = (HandShapeTypes)lastPlayerRound.HandShape;
                    var won = false;
                    var loose = false;

                    switch (handshape)
                    {
                        case HandShapeTypes.Paper:
                            won = lastPlayerHandshape == HandShapeTypes.Rock;
                            loose = lastPlayerHandshape == HandShapeTypes.Scissors;
                            break;
                        case HandShapeTypes.Rock:
                            won = lastPlayerHandshape == HandShapeTypes.Scissors;
                            loose = lastPlayerHandshape == HandShapeTypes.Paper;
                            break;
                        case HandShapeTypes.Scissors:
                            won = lastPlayerHandshape == HandShapeTypes.Paper;
                            loose = lastPlayerHandshape == HandShapeTypes.Rock;
                            break;
                        default:
                            break;
                    }

                    if (won)
                    {
                        player.Rounds.Add(round);

                        var victories = db.Games.Find(activeRound.IdGame).Rounds.Where(r => r.IdPlayer == activeRound.IdPlayer && r.Won).Count();
                        round.Won = true;

                        if (victories > 3)
                        {
                            request.IdWinner = activeRound.IdPlayer;
                        }
                    }
                    else if (loose)
                    {
                        player.Rounds.Add(round);

                        var victories = db.Games.Find(lastPlayerRound.IdGame).Rounds.Where(r => r.IdPlayer == lastPlayerRound.IdPlayer && r.Won).Count();
                        lastPlayerRound.Won = true;

                        if (victories > 2)
                        {
                            request.IdWinner = lastPlayerRound.IdPlayer;
                        }
                    }

                    activeRound.HandShape = request.Handshape;
                }
                else
                {
                    player.Rounds.Add(new Round { IdGame = activeRound.IdGame, HandShape = request.Handshape });
                }


                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return Ok(request);
        }

    }
}