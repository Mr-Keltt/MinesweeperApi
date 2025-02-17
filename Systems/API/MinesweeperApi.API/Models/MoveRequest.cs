using AutoMapper;
using MinesweeperApi.Application.Models;

namespace MinesweeperApi.API.Models;

public class MoveRequest
{
    public Guid GameId { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
}

public class MoveRequestProfile : Profile
{
    public MoveRequestProfile()
    {
        CreateMap<MoveRequest, MoveModel>();
    }
}