using CoreBot.BLL.Dto;
using CoreBot.BLL.Interfaces;
using CoreBot1.Options;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class AddKeysDialog : ComponentDialog
    {
        private IKeyService _keyService;
        public AddKeysDialog(IKeyService keyService)
           : base(nameof(AddKeysDialog))
        {
            _keyService = keyService;

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
            AskKeyCountStep,
            AddKeyStep
            }));

            AddDialog(new TextPrompt("textPrompt"));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> AskKeyCountStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync("textPrompt", new PromptOptions
            {
                Prompt = MessageFactory.Text("Введите количество ключей для добавления")
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AddKeyStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Options as GameOption;

            var gameId = options.GameId;

            stepContext.Values["KeyCount"] = int.Parse((string)stepContext.Result);

            var count = (int)stepContext.Values["KeyCount"];

            for (int i = 0; i < count; i++)
            {
                var keyDto = new KeyDto
                {
                    KeyValue = GenerateKey(),
                    GameId = gameId,
                    IsBought = false

                };
                await _keyService.AddKeyAsync(keyDto);
            }



            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ключи добавлены"), cancellationToken);
            return await stepContext.EndDialogAsync("", cancellationToken);
        }

        private string GenerateKey()
        {
            // Генерация случайных символов для каждой части ключа
            string part1 = GenerateRandomString(5);
            string part2 = GenerateRandomString(5);
            string part3 = GenerateRandomString(5);

            // Сборка ключа в формате XBBRF-MR09R-NX8V
            string key = $"{part1}-{part2}-{part3}";

            return key;
        }

        private string GenerateRandomString(int length)
        {
            // Список символов, которые могут быть в ключе
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Генерация случайной строки из указанных символов
            Random random = new Random();
            string randomString = new string(Enumerable.Repeat(characters, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }
    }
}
