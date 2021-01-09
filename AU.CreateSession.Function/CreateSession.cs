using System;
using System.Collections.Generic;
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
                var requestGuid = Guid.NewGuid();
                logger.LogInformation($"{requestGuid} - Processing Request {JsonConvert.SerializeObject(request.Value)}");

                if (!request.IsValid)
                {
                    var message = $"Request is invalid: {string.Join(", ", request.ValidationResults.Select(s => s.ErrorMessage).ToArray())}";
                    logger.LogError($"{requestGuid} - {message}");
                    return new BadRequestObjectResult(message);
                }

                if (!request.Value.Validate())
                {
                    var message = $"Request data failed validation.";
                    logger.LogError($"{requestGuid} - {message}");
                    return new BadRequestObjectResult(message);
                }

                var players = mapper.Map<List<PlayerRequest>, List<Player>>(request.Value.Players);
                var sessionId = await handler.CreateSession(request.Value.SessionId, players);

                logger.LogInformation($"{requestGuid} - Processing Request Succeeded for Session ID: {sessionId}");
                return new OkObjectResult(new CreateSessionResponse { SessionId = sessionId });
            }
            catch (Exception e)
            {
                logger.LogError(e, "Failure in Create Session Function. Unable to process");
                throw;
            }
        }
    }
}
