using CoreBot.BLL.Dto;
using CoreBot1.Options;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class BuyKeyDialog : ComponentDialog
    {
        private IOrderService _orderService;
        private string Link = "https://a24251-4a99.u.d-f.pw/api/Order?orderid=1";
        public BuyKeyDialog(IOrderService orderService)
           : base(nameof(BuyKeyDialog))
        {
            _orderService = orderService;

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
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(Link), cancellationToken);

            // Принудительно перейти к следующему шагу
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> AddKeyStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await Task.Delay(15000);
            var options = stepContext.Options as GameOption;

            var keyId = options.GameId;

            stepContext.Values["KeyValue"] = (string)stepContext.Result;

            var orderDto = new OrderDto
            {
                GameId = keyId,
                UserId = stepContext.Context.Activity.From.Id,
                OrderDate = DateTime.UtcNow

            };


            await _orderService.BuyKeyAsync(orderDto);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text("Ключ получен"), cancellationToken);
            return await stepContext.EndDialogAsync("", cancellationToken);
        }
    }
}
