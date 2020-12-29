using System.ComponentModel.DataAnnotations;

namespace AU.CreateSession.Function.Request
{
    public class PlayerRequest
    {
        public string Name { get; set; }

        public int Position { get; set; }

        public int Colour { get; set; }
    }
}
