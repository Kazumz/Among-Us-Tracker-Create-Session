namespace AU.CreateSession.Function.Request
{
    public class PlayerRequest
    {
        public string Name { get; set; }

        public int Position { get; set; }

        public int Colour { get; set; }
    }

    public static class PlayerRequestExtensions
    {
        public static bool Validate(this PlayerRequest request)
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                if (request.Name.Length > 10)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
