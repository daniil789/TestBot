using CoreBot.BLL.Interfaces;
using HtmlAgilityPack;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot1.Dialogs
{
    public class GetAllGamesDialog : ComponentDialog
    {
        private readonly IGameService _gameService;
        private readonly IUserService _userService;

        public GetAllGamesDialog(IGameService gameService, IUserService userService)
            : base(nameof(GetAllGamesDialog))
        {
            _gameService = gameService;
            _userService = userService;

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
            DisplayGamesStep
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> DisplayGamesStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var games = _gameService.GetAllGames();

            var curUser = await _userService.GetUserByIdAsync(stepContext.Context.Activity.From.Id);
            var carouselCards = new List<Attachment>();
            foreach (var game in games)
            {
                if (curUser.IsAdmin)
                {
                    var card = new HeroCard
                    {
                        Title = game.Title,
                        Subtitle = $"Платформа: {game.Platform}, Разработчик: {game.Developer} Цена: {game.Price}",
                        Text = game.Description,
                        Images = new List<CardImage> { new CardImage(Parse(game.ImageUrl)) },
                        Buttons = new List<CardAction>
                {
                    new CardAction
                    {
                        Type = ActionTypes.ImBack,
                        Title = "Добавить ключ вручную",
                        Value = $"addkey_{game.Id}"  // Значение, которое будет отправлено обработчику команды
                    },

                     new CardAction
                    {
                        Type = ActionTypes.ImBack,
                        Title = "Добавить ключи автоматически",
                        Value = $"addkeys_{game.Id}"  // Значение, которое будет отправлено обработчику команды
                    },
                    new CardAction
                    {
                        Type = ActionTypes.ImBack,
                        Title = "Ключи",
                        Value = $"getkeys_{game.Id}"  // Значение, которое будет отправлено обработчику команды
                    },
                }

                    };

                    carouselCards.Add(card.ToAttachment());
                }
                else
                {
                    var card = new HeroCard
                    {
                        Title = game.Title,
                        Subtitle = $"Платформа: {game.Platform}, Разработчик: {game.Developer} Цена: {game.Price}",
                        Text = game.Description,
                        Images = new List<CardImage> { new CardImage(Parse(game.ImageUrl)) },
                        Buttons = new List<CardAction>
                {
                    new CardAction
                    {
                        Type = ActionTypes.ImBack,
                        Title = "Приобрести ключ",
                        Value = $"buykey_{game.Id}"  // Значение, которое будет отправлено обработчику команды
                    },
                }

                    };

                    carouselCards.Add(card.ToAttachment());
                }
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
