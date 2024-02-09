using AutoMapper;
using CoreBot.BLL.Dto;
using CoreBot.BLL.Interfaces;
using CoreBot.DAL.Intefaces;
using CoreBot.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBot.BLL.Services
{
    public class KeyService : IKeyService
    {
        private readonly IKeyRepository _keyRepository;

        public KeyService(IKeyRepository keyRepository)
        {
            _keyRepository = keyRepository;
        }

        private KeyDto MapToDto(Key key)
        {
            return new KeyDto
            {
                Id = key.Id,
                GameId = key.GameId,
                KeyValue = key.KeyValue
                // Другие свойства, если они есть
            };
        }

        private Key MapToEntity(KeyDto keyDto)
        {
            return new Key
            {
                Id = keyDto.Id,
                GameId = keyDto.GameId,
                KeyValue = keyDto.KeyValue,
                IsBought = keyDto.IsBought
                // Другие свойства, если они есть
            };
        }

        public async Task<List<KeyDto>> GetAllKeysAsync()
        {
            var keys = await _keyRepository.GetAllKeysAsync();
            return keys.Select(key => MapToDto(key)).ToList();
        }

        public async Task<KeyDto> GetKeyByIdAsync(int keyId)
        {
            var key = await _keyRepository.GetKeyByIdAsync(keyId);
            return key != null ? MapToDto(key) : null;
        }

        public async Task<List<KeyDto>> GetKeysByGameIdAsync(int gameId)
        {
            var keys = await _keyRepository.GetKeysByGameIdAsync(gameId);
            return keys.Select(key => MapToDto(key)).ToList();
        }

        public async Task AddKeyAsync(KeyDto keyDto)
        {
            var keyEntity = MapToEntity(keyDto);
            // Дополнительная бизнес-логика, если необходимо, перед сохранением в репозиторий
            await _keyRepository.AddKeyAsync(keyEntity);
        }

        public async Task UpdateKeyAsync(KeyDto keyDto)
        {
            var keyEntity = MapToEntity(keyDto);
            // Дополнительная бизнес-логика, если необходимо, перед обновлением в репозитории
            await _keyRepository.UpdateKeyAsync(keyEntity);
        }

        public async Task DeleteKeyAsync(int keyId)
        {
            // Дополнительная бизнес-логика, если необходимо, перед удалением в репозитории
            await _keyRepository.DeleteKeyAsync(keyId);
        }
    }


}
