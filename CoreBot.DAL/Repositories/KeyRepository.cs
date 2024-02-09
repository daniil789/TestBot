using CoreBot.DAL.Intefaces;
using CoreBot.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.DAL.Repositories
{
    public class KeyRepository : IKeyRepository
    {
        private readonly GameStoreDbContext _dbContext;

        public KeyRepository(GameStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Key>> GetAllKeysAsync()
        {
            return await _dbContext.Keys.ToListAsync();
        }

        public async Task<Key> GetKeyByIdAsync(int keyId)
        {
            return await _dbContext.Keys.FirstOrDefaultAsync(k => k.Id == keyId);
        }

        public async Task<List<Key>> GetKeysByGameIdAsync(int gameId)
        {
            return await _dbContext.Keys.Where(k => k.GameId == gameId).ToListAsync();
        }

        public async Task AddKeyAsync(Key key)
        {
            _dbContext.Keys.Add(key);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateKeyAsync(Key key)
        {
            _dbContext.Keys.Update(key);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteKeyAsync(int keyId)
        {
            var key = await _dbContext.Keys.FirstOrDefaultAsync(k => k.Id == keyId);
            if (key != null)
            {
                _dbContext.Keys.Remove(key);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
