using System;
using System.Web.Http;
using GameOfDrones.Entity;
using GameOfDrones.Entity.Extensions;
using GameOfDrones.WebApi.Kernel;
using GameOfDrones.WebApi.Kernel.Models;
using GameOfDrones.WebApi.Persistence;

namespace GameOfDrones.WebApi.Controllers
{
    public class RoundController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoundController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IHttpActionResult PostRound([FromBody]Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var player = _unitOfWork.Players.GetPlayerById(request.Id);
                var activeRound = _unitOfWork.Rounds.GetLastByPlayer(request.Id);
                var game = _unitOfWork.Games.GetGameById(activeRound.IdGame);

                var round = new Round { IdPlayer = player.IdPlayer, IdGame = activeRound.IdGame, HandShape = request.Handshape };

                if (request.Turn == 0)
                {
                    _unitOfWork.Rounds.Insert(round);
                }
                else
                {
                    var lastPlayerRound = _unitOfWork.Rounds.GetPreviousRound(activeRound.IdGame);
                    var lastPlayer = _unitOfWork.Players.GetPlayerById(lastPlayerRound.IdPlayer);

                    var handshape = (HandShapeTypes)request.Handshape;
                    var lastPlayerHandshape = (HandShapeTypes)lastPlayerRound.HandShape;

                    var gameResult = handshape.GetResult(lastPlayerHandshape);

                    if (gameResult.Item1)//Won
                    {
                        round.Won = true;
                        _unitOfWork.Rounds.Insert(round);

                        if (IsWinner(game.IdGame, player.IdPlayer))
                        {
                            request.IdWinner = activeRound.IdPlayer;
                            game.IdWinner = activeRound.IdPlayer;
                            player.SetAsWinner();
                            lastPlayer.GamesPlayed = lastPlayer.GamesPlayed.HasValue ? ++lastPlayer.GamesPlayed : 1;
                        }
                    }
                    else if (gameResult.Item2)//Lose
                    {
                        lastPlayerRound.Won = true;
                        _unitOfWork.Rounds.Insert(round);

                        if (IsWinner(game.IdGame, lastPlayer.IdPlayer))
                        {
                            request.IdWinner = lastPlayerRound.IdPlayer;
                            game.IdWinner = lastPlayerRound.IdPlayer;
                            lastPlayer.SetAsWinner();
                            player.GamesPlayed = player.GamesPlayed.HasValue ? ++player.GamesPlayed : 1;
                        }
                    }

                    activeRound.HandShape = request.Handshape;
                }

                _unitOfWork.Complete();
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(request);
        }

        private bool IsWinner(int idGame, int idPlayer)
        {
            var victories = _unitOfWork.Games.GetNumberOfVictoriesByPlayer(idGame, idPlayer);

            return victories > 2;
        }
    }
}