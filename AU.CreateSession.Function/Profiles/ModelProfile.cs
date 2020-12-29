using AU.CreateSession.Domain.Entities;
using AutoMapper;

namespace AU.CreateSession.Function.Profiles
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Services.Repositories.Player.DataModel.Player, Player>()
                .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.RowKey));
        }
    }
}
