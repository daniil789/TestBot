﻿using CoreBot.BLL.Dto;
using CoreBot.BLL.Interfaces;
using CoreBot.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public IEnumerable<GameDto> GetAllGames()
        {
            var games = _gameRepository.GetAllGames();
            return MapToGameDtoList(games);
        }

        public GameDto GetGameById(int gameId)
        {
            var game = _gameRepository.GetGameById(gameId);
            return MapToGameDto(game);
        }

        public void AddGame(GameDto gameDto)
        {
            // Дополнительные проверки бизнес-логики перед добавлением игры, если необходимо.
            var game = MapToGame(gameDto);
            _gameRepository.AddGame(game);
        }

        public void UpdateGame(GameDto gameDto)
        {
            // Дополнительные проверки бизнес-логики перед обновлением информации об игре, если необходимо.
            var game = MapToGame(gameDto);
            _gameRepository.UpdateGame(game);
        }

        public void DeleteGame(int gameId)
        {
            // Дополнительные проверки бизнес-логики перед удалением игры, если необходимо.
            _gameRepository.DeleteGame(gameId);
        }

        private GameDto MapToGameDto(Game game)
        {
            try
            {
                return new GameDto
                {
                    Id = game.Id,
                    Title = game.Title,
                    Genre = game.Genre,
                    Description = game.Description,
                    Developer = game.Developer,
                    Platform = game.Platform,
                    Price = game.Price,
                    ImageUrl = game.ImageUrl,
                };
            }
            catch (Exception ex)
            {
                File.AppendAllText("ServiceLog.txt", ex.Message);
                return null;
            }


        }

        private IEnumerable<GameDto> MapToGameDtoList(IEnumerable<Game> games)
        {
            return games.Select(MapToGameDto);
        }

        private Game MapToGame(GameDto gameDto)
        {
            return new Game
            {
                Id = gameDto.Id,
                Title = gameDto.Title,
                Description = gameDto.Description,
                Genre = gameDto.Genre,
                Developer = gameDto.Developer,
                Platform = gameDto.Platform,
                Price = gameDto.Price,
                ImageUrl = gameDto.ImageUrl,
            };
        }

        public IEnumerable<GameDto> SearchGames(string searchString)
        {
            var games = _gameRepository.SearchGames(searchString);
            return MapToGameDtoList(games);
        }

        public IEnumerable<GameDto> GetGamesByPriceRange(int minPrice, int maxPrice)
        {
            var games = _gameRepository.GetGamesByPriceRange(minPrice, maxPrice);
            return MapToGameDtoList(games);
        }
    }

}
