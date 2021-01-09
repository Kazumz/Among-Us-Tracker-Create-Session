using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AU.CreateSession.Domain.CreateSession;
using AU.CreateSession.Domain.Entities;
using AU.CreateSession.Function.Extensions;
using AU.CreateSession.Function.Request;
using AU.CreateSession.Function.Response;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AU.CreateSession.Function
{
    public class CreateSession
    {
        private readonly ICreateSessionHandler handler;
        private readonly IMapper mapper;

        public CreateSession(
            ICreateSessionHandler handler,
            IMapper mapper)
        {
            this.handler = handler;
            this.mapper = mapper;
        }

        [FunctionName("CreateSession")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger logger)
        {
            try
            {
                var request = await req.GetBodyAsync<CreateSessionRequest>();
                logger.LogInformation($"Processing Request {JsonConvert.SerializeObject(request.Value)}");

                if (request.IsValid && request.Value.Validate())
                {
                    var players = mapper.Map<List<PlayerRequest>, List<Player>>(request.Value.Players);

                    var sessionId = await handler.CreateSession(request.Value.SessionId, players);

                    logger.LogInformation($"Processing Request Succeeded for Session ID: {sessionId}");
                    return new OkObjectResult(new CreateSessionResponse { SessionId = sessionId });
                }
                else
                {
                    return new BadRequestObjectResult($"Request is invalid: {string.Join(", ", request.ValidationResults.Select(s => s.ErrorMessage).ToArray())}");
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failure in Create Session Function. Unable to process");
                throw;
            }
        }
    }
}
