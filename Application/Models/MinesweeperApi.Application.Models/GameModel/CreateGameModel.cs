using AutoMapper;
using MinesweeperApi.Infrastructure.Data.Entities;
using Newtonsoft.Json;

namespace MinesweeperApi.Application.Models;

public class CreateGameModel : IGameModel
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int MinesCount { get; set; }
    public bool Completed { get; set; }
    public int[,] CurrentField { get; set; }
}

public class CreateGameProfile : Profile
{
    public CreateGameProfile()
    {
        CreateMap<CreateGameModel, GameEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.Game, opt => opt.MapFrom<CreateGameModelToGameJsonResolver>());
    }
}

public class CreateGameModelToGameJsonResolver : IValueResolver<CreateGameModel, GameEntity, string>
{
    public string Resolve(CreateGameModel source, GameEntity destination, string destMember, ResolutionContext context)
    {
        var json = JsonConvert.SerializeObject(source);
        return json;
    }
}
