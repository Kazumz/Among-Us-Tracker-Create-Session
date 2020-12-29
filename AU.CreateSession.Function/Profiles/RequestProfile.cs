using AU.CreateSession.Domain.Entities;
using AU.CreateSession.Function.Request;
using AutoMapper;

namespace AU.CreateSession.Function.Profiles
{
    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<PlayerRequest, Player>();
        }
    }
}
