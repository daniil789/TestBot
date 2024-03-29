﻿using CoreBot.BLL.Interfaces;
using CoreBot1.Options;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class GetKeysDialog : ComponentDialog
    {
        private IKeyService _keyService;
        public GetKeysDialog(IKeyService keyService)
           : base(nameof(GetKeysDialog))
        {
            _keyService = keyService;

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
            DisplayKeysStep
            }));

            AddDialog(new TextPrompt("textPrompt"));
            InitialDialogId = nameof(WaterfallDialog);
        }
        private async Task<DialogTurnResult> DisplayKeysStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Options as GameOption;

            var gameId = options.GameId;
            var keys = await _keyService.GetKeysByGameIdAsync(gameId);

            string result = string.Empty;
            foreach (var key in keys)
            {
                result += key.KeyValue + "\n";
            }

            var reply = MessageFactory.Text(result);
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
