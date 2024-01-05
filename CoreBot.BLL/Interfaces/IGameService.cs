using CoreBot.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.BLL.Interfaces
{
    public interface IGameService
    {
        IEnumerable<GameDto> GetAllGames();
        GameDto GetGameById(int gameId);
        void AddGame(GameDto gameDto);
        void UpdateGame(GameDto gameDto);
        void DeleteGame(int gameId);
    }

}
