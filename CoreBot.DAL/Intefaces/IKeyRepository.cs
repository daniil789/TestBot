using CoreBot.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.DAL.Intefaces
{
    public interface IKeyRepository
    {
        Task<List<Key>> GetAllKeysAsync();
        Task<Key> GetKeyByIdAsync(int keyId);
        Task<List<Key>> GetKeysByGameIdAsync(int gameId);
        Task AddKeyAsync(Key key);
        Task UpdateKeyAsync(Key key);
        Task DeleteKeyAsync(int keyId);
    }
}
