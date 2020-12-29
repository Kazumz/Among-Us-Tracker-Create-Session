using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AU.CreateSession.Domain.Entities;
using AU.CreateSession.Domain.Services.Repositories;

namespace AU.CreateSession.Domain.CreateSession
{
    public class CreateSessionHandler : ICreateSessionHandler
    {
        private readonly IPlayerRepository playerRepository;

        public CreateSessionHandler(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public async Task<Guid> CreateSession(Guid? existingId, List<Player> players)
        {
            var sessionId = existingId ?? Guid.NewGuid();
            if (existingId.HasValue)
            {
                var existingPlayers = await playerRepository.GetPlayers(existingId.Value);
                if (existingPlayers != null && existingPlayers.Any())
                {
                    await RemoveStalePlayers(existingId.Value, players, existingPlayers);
                }
            }

            await playerRepository.CreateUpdatePlayers(sessionId, players);
            return sessionId;
        }

        private async Task RemoveStalePlayers(Guid sessionId, List<Player> newPlayers, List<Player> existingPlayers)
        {
            foreach (var ep in existingPlayers)
            {
                if (!newPlayers.Any(p => p.Colour == ep.Colour))
                {
                    await playerRepository.DeletePlayer(sessionId, ep.Colour);
                }
            }
        }
    }
}
