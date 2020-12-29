using AU.CreateSession.Domain.Enums;

namespace AU.CreateSession.Domain.Entities
{
    public class Player
    {
        public string Name { get; set; }

        public Position Position { get; set; }

        public Colour Colour { get; set; }
    }
}
