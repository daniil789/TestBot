using CoreBot.BLL.Dto;
using CoreBot.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.BLL.Interfaces
{
    public interface IKeyService
    {
        Task<List<KeyDto>> GetAllKeysAsync();
        Task<KeyDto> GetKeyByIdAsync(int keyId);
        Task<List<KeyDto>> GetKeysByGameIdAsync(int gameId);
        Task AddKeyAsync(KeyDto keyDto);
        Task UpdateKeyAsync(KeyDto keyDto);
        Task DeleteKeyAsync(int keyId);
    }
}
