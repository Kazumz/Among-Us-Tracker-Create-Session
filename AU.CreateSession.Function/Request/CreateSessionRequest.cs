using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AU.CreateSession.Function.Request
{
    public class CreateSessionRequest
    {
        public Guid? SessionId { get; set; }

        [Required]
        public List<PlayerRequest> Players { get; set; }
    }

    public static class CreateSessionRequestExtensions
    {
        public static bool Validate(this CreateSessionRequest request)
        {
            return 
                request.Players != null &&
                request.Players.Count > 0 &&
                request.Players.Count <= 13 &&
                request.Players.All(x => x.Validate());
        }
    }
}
