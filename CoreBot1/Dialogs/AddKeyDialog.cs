using CoreBot.BLL.Dto;
using CoreBot.BLL.Interfaces;
using CoreBot1.Options;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class AddKeyDialog : ComponentDialog
    {
        private IKeyService _keyService;
        public AddKeyDialog(IKeyService keyService)
           : base(nameof(AddKeyDialog))
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
                Prompt = MessageFactory.Text("Введите ключ")
            }, cancellationToken);
        }

        private async Task<DialogTurnResult> AddKeyStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Options as GameOption;

            var gameId = options.GameId;

            stepContext.Values["KeyValue"] = (string)stepContext.Result;

            var keyDto = new KeyDto
            {
                KeyValue = (string)stepContext.Values["KeyValue"],
                GameId = gameId,
                IsBought = false

            };


            await _keyService.AddKeyAsync(keyDto);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ключ добавлен!"), cancellationToken);
            return await stepContext.EndDialogAsync(keyDto, cancellationToken);
        }
    }
}
