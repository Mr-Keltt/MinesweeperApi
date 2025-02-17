using AutoMapper;
using MinesweeperApi.Application.Models;

namespace MinesweeperApi.API.Models;

public class GameResponse
{
    public Guid GameId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int MinesCount { get; set; }
    public bool Completed { get; set; }
    public string[][] Field { get; set; }
}

public class GameResponseProfile : Profile
{
    public GameResponseProfile()
    {
        CreateMap<GameModel, GameResponse>()
                .ForMember(dest => dest.GameId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Field, opt => opt.MapFrom<FieldToApiResolver>()); 
    }
}

public class FieldToApiResolver : IValueResolver<GameModel, GameResponse, string[][]>
{
    public string[][] Resolve(GameModel source, GameResponse destination, string[][] destMember, ResolutionContext context)
    {
        int rows = source.CurrentField.GetLength(0);
        int cols = source.CurrentField.GetLength(1);
        var apiField = new string[rows][];

        bool lost = false;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (source.CurrentField[i, j] == -3)
                {
                    lost = true;
                    break;
                }
            }
            if (lost)
                break;
        }

        for (int i = 0; i < rows; i++)
        {
            apiField[i] = new string[cols];
            for (int j = 0; j < cols; j++)
            {
                int cell = source.CurrentField[i, j];
                if (cell >= 0)
                {
                    apiField[i][j] = cell.ToString();
                }
                else
                {
                    if (!source.Completed)
                    {
                        apiField[i][j] = " ";
                    }
                    else
                    {
                        apiField[i][j] = lost
                            ? (cell == -3 ? "X" : "M")
                            : "M";
                    }
                }
            }
        }
        return apiField;
    }
}