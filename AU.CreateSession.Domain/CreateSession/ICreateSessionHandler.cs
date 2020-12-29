using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AU.CreateSession.Domain.Entities;

namespace AU.CreateSession.Domain.CreateSession
{
    public interface ICreateSessionHandler
    {
        Task<Guid> CreateSession(Guid? existingId, List<Player> players);
    }
}
