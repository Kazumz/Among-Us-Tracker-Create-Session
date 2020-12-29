using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AU.CreateSession.Function.Request
{
    public class CreateSessionRequest
    {
        public Guid? SessionId { get; set; }

        [Required]
        public List<PlayerRequest> Players { get; set; }
    }
}
