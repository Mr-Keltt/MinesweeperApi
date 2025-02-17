namespace MinesweeperApi.Application.Services.Settings;

public class MainSettings
{
    public string PublicUrl { get; private set; }
    public string InternalUrl { get; private set; }
    public string AllowedOrigins { get; private set; }
}