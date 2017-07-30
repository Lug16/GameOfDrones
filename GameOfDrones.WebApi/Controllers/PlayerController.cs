using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using GameOfDrones.Entity;
using GameOfDrones.WebApi.Kernel;
using GameOfDrones.WebApi.Persistence;
using GameOfDrones.WebApi.Kernel.Models;

namespace GameOfDrones.WebApi.Controllers
{
    public class PlayerController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IHttpActionResult PostGame([FromBody]Request[] request)
        {
            if (!request.Any())
            {
                return BadRequest("No Data within the request");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var game = new Game();
                var dbPlayers = new List<Player>();

                foreach (var player in request)
                {
                    var round = new Round();

                    var dbPlayer = _unitOfWork.Players.GetPlayerByName(player.Name);

                    if (dbPlayer == null)
                    {
                        var newPlayer = new Player { Name = player.Name };
                        newPlayer.Rounds.Add(round);
                        dbPlayers.Add(newPlayer);
                        _unitOfWork.Players.Insert(newPlayer);
                    }
                    else
                    {
                        dbPlayer.Rounds.Add(round);
                        dbPlayers.Add(dbPlayer);
                    }
                    game.Rounds.Add(round);
                }

                _unitOfWork.Games.Insert(game);
                _unitOfWork.Complete();

                var response = dbPlayers.Select(r => new Request { Id = r.IdPlayer, Name = r.Name }).ToArray();

                return Ok(response);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}