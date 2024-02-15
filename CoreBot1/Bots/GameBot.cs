// GameBot.cs
using CoreBot.BLL.Interfaces;
using CoreBot.DAL.Migrations;
using CoreBot1.Dialogs;
using CoreBot1.Options;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

public class GameBot : ActivityHandler
{
    private readonly BotState _conversationState;
    private readonly BotState _userState;
    private readonly DialogSet _dialogs;
    private readonly IUserService _userService;

    public GameBot(ConversationState conversationState, UserState userState, IConfiguration configuration
                 , IGameService gameService, IKeyService keyService, IUserService userService, IOrderService orderService)
    {
        _conversationState = conversationState;
        _userState = userState;
        _userService = userService;
        _dialogs = new DialogSet(_conversationState.CreateProperty<DialogState>(nameof(DialogState)));

        _dialogs.Add(new AddGameDialog(gameService));
        _dialogs.Add(new GetAllGamesDialog(gameService, userService));
        _dialogs.Add(new AddKeyDialog(keyService));
        _dialogs.Add(new GetKeysDialog(keyService));
        _dialogs.Add(new GetUsersDialog(userService));
        _dialogs.Add(new SearchGameDialog(gameService, userService));
        _dialogs.Add(new BuyKeyDialog(orderService));
        _dialogs.Add(new AddKeysDialog(keyService));
        _dialogs.Add(new GetGamesByPriceRange(gameService, userService));
        _dialogs.Add(new GetMyKeysDialog(orderService));
    }

    public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        File.AppendAllText("LogsRequst", turnContext.Activity.Text);

        var user = turnContext.Activity.From;

        // Доступ к основным данным о пользователе
        string userId = user.Id;
        string userName = user.Name;
        var channel = turnContext.Activity.ChannelId;

        await _userService.AddUserIfNotExistAsync(userId, userName, channel);
        try
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var dialogContext = await _dialogs.CreateContextAsync(turnContext, cancellationToken);
                var results = await dialogContext.ContinueDialogAsync(cancellationToken);

                var userMessage = turnContext.Activity.Text.ToLowerInvariant();

                if (userMessage.StartsWith("addkey_"))
                {
                    var gameId = int.Parse(userMessage.Replace("addkey_", ""));
                    await dialogContext.BeginDialogAsync(nameof(AddKeyDialog), new GameOption { GameId = gameId });
                }
                else if (userMessage.StartsWith("addkeys_"))
                {
                    var gameId = int.Parse(userMessage.Replace("addkeys_", ""));
                    await dialogContext.BeginDialogAsync(nameof(AddKeysDialog), new GameOption { GameId = gameId });
                }
                else
                if (userMessage.StartsWith("buykey_"))
                {
                    var gameId = int.Parse(userMessage.Replace("buykey_", ""));
                    await dialogContext.BeginDialogAsync(nameof(BuyKeyDialog), new GameOption { GameId = gameId });
                }

                else if (userMessage.StartsWith("getkeys_"))
                {
                    var gameId = int.Parse(userMessage.Replace("getkeys_", ""));
                    await dialogContext.BeginDialogAsync(nameof(GetKeysDialog), new GameOption { GameId = gameId });
                }

                else if (results.Status == DialogTurnStatus.Empty && turnContext.Activity.Text == "addgame")
                {
                    await dialogContext.BeginDialogAsync(nameof(AddGameDialog));
                }
                else if (results.Status == DialogTurnStatus.Empty && turnContext.Activity.Text == "Вывести список игр")
                {
                    await dialogContext.BeginDialogAsync(nameof(GetAllGamesDialog));
                }
                else if (results.Status == DialogTurnStatus.Empty && turnContext.Activity.Text == "Пользователи")
                {
                    await dialogContext.BeginDialogAsync(nameof(GetUsersDialog));
                }
                else if (results.Status == DialogTurnStatus.Empty && turnContext.Activity.Text == "Мои ключи")
                {
                    await dialogContext.BeginDialogAsync(nameof(GetMyKeysDialog));
                }
                else if (results.Status == DialogTurnStatus.Empty)
                {
                    var match = Regex.Match(turnContext.Activity.Text, @"(\d+)\s*-\s*(\d+)");

                    if (match.Success)
                    {
                        int minPrice = int.Parse(match.Groups[1].Value);
                        int maxPrice = int.Parse(match.Groups[2].Value);

                        await dialogContext.BeginDialogAsync(nameof(GetGamesByPriceRange), new CoreBot1.Options.PriceOption { MinPrice = minPrice, MaxPrice = maxPrice });
                    }
                    else
                    {
                        await dialogContext.BeginDialogAsync(nameof(SearchGameDialog), new CoreBot1.Options.SearchOption { SearchString = turnContext.Activity.Text });
                    }
                }

                await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
                await _userState.SaveChangesAsync(turnContext, false, cancellationToken);

                var userr = await _userService.GetUserByIdAsync(userId);

                if (dialogContext.ActiveDialog == null)
                {
                    await ShowMenuButtons(turnContext, cancellationToken);
                }
            }
        }
        catch (Exception ex)
        {
            File.AppendAllText("log.txt", ex.Message);
            throw;
        }
    }

    private async Task ShowMenuButtons(ITurnContext turnContext, CancellationToken cancellationToken)
    {
        var reply = turnContext.Activity.CreateReply();
        reply.Text = "Выберите действие:";

        var currentUser = await _userService.GetUserByIdAsync(turnContext.Activity.From.Id);

        if (currentUser.IsAdmin)
        {
            reply.SuggestedActions = new SuggestedActions
            {
                Actions = new List<CardAction>
        {
            new CardAction { Title = "Пользователи", Type = ActionTypes.PostBack, Value = "Пользователи" },
            new CardAction { Title = "Вывести список игр", Type = ActionTypes.PostBack, Value = "Вывести список игр" },
             new CardAction { Title = "Добавить игру", Type = ActionTypes.PostBack, Value = "addgame" },
             new CardAction { Title = "Помощь", Type = ActionTypes.PostBack, Value = "addgame" },
            // Добавьте другие ваши кнопки
        },
            };
        }
        else
        {
            reply.SuggestedActions = new SuggestedActions
            {
                Actions = new List<CardAction>
        {
            new CardAction { Title = "Мои ключи", Type = ActionTypes.PostBack, Value = "Мои ключи" },
            new CardAction { Title = "Вывести список игр", Type = ActionTypes.PostBack, Value = "Вывести список игр" },
            // Добавьте другие ваши кнопки
             new CardAction { Title = "Помощь", Type = ActionTypes.PostBack, Value = "addgame" },
              new CardAction { Title = "Подписаться на рассылки", Type = ActionTypes.PostBack, Value = "addgame" },
        },
            };
        }

        await turnContext.SendActivityAsync(reply, cancellationToken);
    }
}
