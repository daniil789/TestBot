// GameBot.cs
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.BLL.Interfaces;
using CoreBot1.Dialogs;
using CoreBot1.Options;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public class GameBot : ActivityHandler
{
    private readonly BotState _conversationState;
    private readonly BotState _userState;
    private readonly DialogSet _dialogs;
    private readonly IUserService _userService;

    public GameBot(ConversationState conversationState, UserState userState, IConfiguration configuration
                 , IGameService gameService, IKeyService keyService, IUserService userService)
    {
        _conversationState = conversationState;
        _userState = userState;
        _userService = userService;
        _dialogs = new DialogSet(_conversationState.CreateProperty<DialogState>(nameof(DialogState)));

        _dialogs.Add(new AddGameDialog(gameService));
        _dialogs.Add(new GetAllGamesDialog(gameService));
        _dialogs.Add(new AddKeyDialog(keyService));
        _dialogs.Add(new GetKeysDialog(keyService));
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

                if (userMessage.StartsWith("getkeys_"))
                {
                    var gameId = int.Parse(userMessage.Replace("getkeys_", ""));
                    await dialogContext.BeginDialogAsync(nameof(GetKeysDialog), new GameOption { GameId = gameId });
                }

                if (results.Status == DialogTurnStatus.Empty && turnContext.Activity.Text == "addgame")
                {
                    await dialogContext.BeginDialogAsync(nameof(AddGameDialog));
                }
                else if (results.Status == DialogTurnStatus.Empty && turnContext.Activity.Text == "list")
                {
                    await dialogContext.BeginDialogAsync(nameof(GetAllGamesDialog));
                }
                else
                {
                    // turnContext.SendActivityAsync($"{userName}, Извините, я вас не понимаю");
                }

                await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
                await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            File.AppendAllText("log.txt", ex.Message);
        }
    }
}
