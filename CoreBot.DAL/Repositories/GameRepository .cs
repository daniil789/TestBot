using CoreBot.DAL.Models;
using Microsoft.EntityFrameworkCore;

public class GameRepository : IGameRepository
{
    private readonly GameStoreDbContext _dbContext;

    public GameRepository(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddGame(Game game)
    {
        _dbContext.Games.Add(game);
        _dbContext.SaveChanges();
    }

    public IEnumerable<Game> GetAllGames()
    {
        return _dbContext.Games.ToList();
    }

    public Game GetGameById(int gameId)
    {
        return _dbContext.Games.Find(gameId);
    }

    public void UpdateGame(Game game)
    {
        _dbContext.Entry(game).State = EntityState.Modified;
        _dbContext.SaveChanges();
    }

    public void DeleteGame(int gameId)
    {
        var gameToDelete = _dbContext.Games.Find(gameId);
        if (gameToDelete != null)
        {
            _dbContext.Games.Remove(gameToDelete);
            _dbContext.SaveChanges();
        }
    }

    public IEnumerable<Game> SearchGames(string searchString)
    {
        var matchingGames = _dbContext.Games
            .Where(game =>
                game.Title.Contains(searchString) ||
                game.Genre.Contains(searchString) ||
                game.Description.Contains(searchString) ||
                game.Developer.Contains(searchString) ||
                game.Platform.Contains(searchString))
            .ToList();
        return matchingGames;
    }

    public IEnumerable<Game> GetGamesByPriceRange(int minPrice, int maxPrice)
    {
        return _dbContext.Games
            .Where(game => game.Price >= minPrice && game.Price <= maxPrice)
            .ToList();
    }
}
