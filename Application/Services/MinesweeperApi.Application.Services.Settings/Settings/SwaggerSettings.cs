namespace MinesweeperApi.Application.Services.Settings;

public class SwaggerSettings
{
    public bool Enabled { get; private set; } = false;

    public SwaggerSettings()
    {
        Enabled = false;
    }
}