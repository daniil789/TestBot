// GameBot.cs
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CoreBot.BLL.Interfaces;
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

    public GameBot(ConversationState conversationState, UserState userState, IConfiguration configuration, IGameService gameService)
    {
        _conversationState = conversationState;
        _userState = userState;

        _dialogs = new DialogSet(_conversationState.CreateProperty<DialogState>(nameof(DialogState)));

        _dialogs.Add(new AddGameDialog(gameService));
    }

    public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        try
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var dialogContext = await _dialogs.CreateContextAsync(turnContext, cancellationToken);
                var results = await dialogContext.ContinueDialogAsync(cancellationToken);

                if (results.Status == DialogTurnStatus.Empty && turnContext.Activity.Text == "Добавить игру")
                {
                    await dialogContext.BeginDialogAsync(nameof(AddGameDialog));
                }
                else
                {
                    turnContext.SendActivityAsync("Извините, я вас не понимаю");
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
