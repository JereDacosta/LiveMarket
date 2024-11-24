namespace XboxStore.Services;

public interface IXboxGameService
{
    Task<List<VideoGame>> GetXboxGames();
}
