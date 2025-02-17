using AutoMapper;
using MinesweeperApi.Application.Models;

namespace MinesweeperApi.API.Models;

public class CreateGameRequest
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int MinesCount { get; set; }
}

public class CreateGameRequestProfile : Profile
{
    public CreateGameRequestProfile()
    {
        CreateMap<CreateGameRequest, CreateGameModel>();
    }
}