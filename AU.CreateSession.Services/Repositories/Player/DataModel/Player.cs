using System;
using Microsoft.Azure.Cosmos.Table;

namespace AU.CreateSession.Services.Repositories.Player.DataModel
{
    public class Player : TableEntity
    {
        public Player()
        {
        }

        public Player(Guid guid, int colour)
        {
            PartitionKey = guid.ToString();
            RowKey = colour.ToString();
        }

        public int Position { get; set; }

        public string Name { get; set; }
    }
}
