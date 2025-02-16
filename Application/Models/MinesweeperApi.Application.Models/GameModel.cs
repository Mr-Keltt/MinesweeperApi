namespace MinesweeperApi.Application.Models;

using AutoMapper;
using MinesweeperApi.Infrastructure.Data.Entities;
using Newtonsoft.Json;

public class GameModel
{
    public int Id { get; set; }
    public int[,] Field { get; set; }
}

public class GameMappingProfile : Profile
{
    public GameMappingProfile()
    {
        CreateMap<GameEntity, GameModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.GetHashCode()))
            .ForMember(dest => dest.Field, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<int[,]>(src.FieldJson)));

        CreateMap<GameModel, GameEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.FieldJson, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Field)));
    }
}