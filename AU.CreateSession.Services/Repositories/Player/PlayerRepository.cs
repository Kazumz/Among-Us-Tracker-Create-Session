using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AU.CreateSession.Domain.Enums;
using AU.CreateSession.Domain.Services.Repositories;
using AU.CreateSession.Services.External.Cosmos;
using AutoMapper;
using Microsoft.Azure.Cosmos.Table;

namespace AU.CreateSession.Services.Repositories.Player
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ITableContext tableContext;
        private readonly CloudTable table;
        private readonly IMapper mapper;

        public PlayerRepository(
            CloudTable table,
            ITableContext tableContext,
            IMapper mapper)
        {
            this.tableContext = tableContext;
            this.table = table;
            this.mapper = mapper;
        }

        public async Task CreateUpdatePlayers(Guid sessionId, List<Domain.Entities.Player> players)
        {
            foreach (var player in players)
            {
                var model = new DataModel.Player(sessionId, (int)player.Colour)
                {
                    Position = (int)player.Position,
                    Name = player.Name ?? ""
                };

                await tableContext.InsertOrUpdate(table, model);
            }
        }

        public async Task<List<Domain.Entities.Player>> GetPlayers(Guid sessionId)
        {
            var results = await tableContext.RetrieveByPartitionKey<DataModel.Player>(table, sessionId.ToString());

            return mapper.Map<IEnumerable<DataModel.Player>, List<Domain.Entities.Player>>(results);
        }

        public async Task DeletePlayer(Guid sessionId, Colour colour)
        {
            await tableContext.Delete(table, new DataModel.Player(sessionId, (int)colour) { ETag = "*" });
        }
    }
}
