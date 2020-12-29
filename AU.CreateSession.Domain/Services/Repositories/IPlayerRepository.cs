using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AU.CreateSession.Domain.Entities;
using AU.CreateSession.Domain.Enums;

namespace AU.CreateSession.Domain.Services.Repositories
{
    public interface IPlayerRepository
    {
        Task CreateUpdatePlayers(Guid sessionId, List<Player> players);

        Task<List<Player>> GetPlayers(Guid sessionId);

        Task DeletePlayer(Guid sessionId, Colour colour);
    }
}
