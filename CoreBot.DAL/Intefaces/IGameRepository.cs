using CoreBot.DAL.Models;

public interface IGameRepository
{
    // Создание новой игры
    void AddGame(Game game);

    // Получение списка всех игр
    IEnumerable<Game> GetAllGames();

    // Получение игры по идентификатору
    Game GetGameById(int gameId);

    // Обновление информации об игре
    void UpdateGame(Game game);

    // Удаление игры по идентификатору
    void DeleteGame(int gameId);
}
