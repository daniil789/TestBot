using CoreBot.BLL.Interfaces;
using CoreBot1.Options;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class GetMyKeysDialog : ComponentDialog
    {
        private IOrderService _orderService;
        public GetMyKeysDialog(IOrderService orderService)
           : base(nameof(GetMyKeysDialog))
        {
            _orderService = orderService;

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
            DisplayKeysStep
            }));

            AddDialog(new TextPrompt("textPrompt"));
            InitialDialogId = nameof(WaterfallDialog);
        }
        private async Task<DialogTurnResult> DisplayKeysStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(stepContext.Context.Activity.From.Id);

            string result = string.Empty;
            foreach (var order in orders)
            {
                result += order.Key.Game.Title + ": " + order.Key.KeyValue + "\n";
            }

            var reply = MessageFactory.Text(result);
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
