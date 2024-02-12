using CoreBot.BLL.Interfaces;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;
using System;

namespace CoreBot1.Dialogs
{
    public class GetUsersDialog : ComponentDialog
    {
        private readonly IUserService _userService;

        public GetUsersDialog(IUserService userService)
            : base(nameof(GetUsersDialog))
        {
            _userService = userService;

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
            DisplayGamesStep
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> DisplayGamesStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var games = await _userService.GetAllUsersAsync();

            var carouselCards = new List<Attachment>();
            foreach (var game in games)
            {
                var card = new HeroCard
                {
                    Title = game.Name,
                    Subtitle = $"Платформа: {game.Channel}",
                    Buttons = new List<CardAction>
                {
                    new CardAction
                    {
                        Type = ActionTypes.ImBack,
                        Title = "Удалить",
                        Value = $"addkey_{game.Id}"  // Значение, которое будет отправлено обработчику команды
                    },
                    new CardAction
                    {
                        Type = ActionTypes.ImBack,
                        Title = "Отправить уведомление",
                        Value = $"getkeys_{game.Id}"  // Значение, которое будет отправлено обработчику команды
                    },
                }

                };

                carouselCards.Add(card.ToAttachment());
            }

            var reply = MessageFactory.Carousel(carouselCards);
            await stepContext.Context.SendActivityAsync(reply, cancellationToken);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
        private string Parse(string htmlCode)
        {

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlCode);

            // Извлекаем ссылку
            HtmlNode linkNode = htmlDoc.DocumentNode.SelectSingleNode("//a/@href");

            if (linkNode != null)
            {
                string link = linkNode.GetAttributeValue("href", "");
                return link;
            }
            else
            {
                return htmlCode;
            }


        }
    }
}
