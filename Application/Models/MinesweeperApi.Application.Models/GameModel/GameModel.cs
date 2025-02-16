namespace MinesweeperApi.Application.Models;

using AutoMapper;
using MinesweeperApi.Infrastructure.Data.Entities;
using Newtonsoft.Json;

public class CreateGameModel : IGameModel
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int MinesCount { get; set; }
    public bool Completed { get; set; }
    public int[,] CurrentField { get; set; }
}

public class GameModel : IGameModel
{
    public Guid Id { get; set; }
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
            .ForMember(dest => dest.Game, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src)));
    }
}

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<GameEntity, GameModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
            .AfterMap((src, dest) =>
            {
                var tmp = JsonConvert.DeserializeObject<GameModel>(src.Game);
                dest.Width = tmp.Width;
                dest.Height = tmp.Height;
                dest.Completed = tmp.Completed;
                dest.CurrentField = tmp.CurrentField;
            });

        CreateMap<GameModel, GameEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Game, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src)));
    }
}