using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using CoreBot.BLL;
using CoreBot.BLL.Dto;
using CoreBot.BLL.Interfaces;
using CoreBot.DAL.Models;
using HtmlAgilityPack;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

public class AddGameDialog : ComponentDialog
{
    private IGameService _gameService;
    public AddGameDialog(IGameService gameService) : base(nameof(AddGameDialog))
    {
        _gameService = gameService;
        AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
        {
            AskTitleStep,
            AskDescriptionStep,
            AskPlatformStep,
            AskDeveloperStep,
            AskGenreStep,
            AskPriceStep,
            AskImageUrlStep,
            AddGameStep

        }));

        AddDialog(new TextPrompt("textPrompt"));

        InitialDialogId = nameof(WaterfallDialog);
        _gameService = gameService;
    }

    private async Task<DialogTurnResult> AskTitleStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        return await stepContext.PromptAsync("textPrompt", new PromptOptions
        {
            Prompt = MessageFactory.Text("Введите название игры:")
        }, cancellationToken);
    }

    private async Task<DialogTurnResult> AskDescriptionStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["Title"] = (string)stepContext.Result;
        return await stepContext.PromptAsync("textPrompt", new PromptOptions
        {
            Prompt = MessageFactory.Text("Введите описание игры:")
        }, cancellationToken);
    }

    private async Task<DialogTurnResult> AskPlatformStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["Description"] = (string)stepContext.Result;
        return await stepContext.PromptAsync("textPrompt", new PromptOptions
        {
            Prompt = MessageFactory.Text("Введите платформу игры:")
        }, cancellationToken);
    }

    private async Task<DialogTurnResult> AskDeveloperStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["Platform"] = (string)stepContext.Result;
        return await stepContext.PromptAsync("textPrompt", new PromptOptions
        {
            Prompt = MessageFactory.Text("Введите разработчика игры:")
        }, cancellationToken);
    }
    private async Task<DialogTurnResult> AskGenreStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["Developer"] = (string)stepContext.Result;
        return await stepContext.PromptAsync("textPrompt", new PromptOptions
        {
            Prompt = MessageFactory.Text("Введите жанр игры:")
        }, cancellationToken);
    }
    private async Task<DialogTurnResult> AskPriceStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["Genre"] = (string)stepContext.Result;
        return await stepContext.PromptAsync("textPrompt", new PromptOptions
        {
            Prompt = MessageFactory.Text("Введите цену игры:")
        }, cancellationToken);
    }

    private async Task<DialogTurnResult> AskImageUrlStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["Price"] = decimal.Parse((string)stepContext.Result);
        return await stepContext.PromptAsync("textPrompt", new PromptOptions
        {
            Prompt = MessageFactory.Text("Введите ссылку на обложку игры:")
        }, cancellationToken);
    }

    private async Task<DialogTurnResult> AddGameStep(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        stepContext.Values["ImageUrl"] = (string)stepContext.Result;

        var gameDto = new GameDto
        {
            Title = (string)stepContext.Values["Title"],
            Description = (string)stepContext.Values["Description"],
            Platform = (string)stepContext.Values["Platform"],
            Developer = (string)stepContext.Values["Developer"],
            Genre = (string)stepContext.Values["Genre"],
            Price = (decimal)stepContext.Values["Price"],
            ImageUrl = (string)stepContext.Values["ImageUrl"]
        };

        // Получаем сервис из контекста
        _gameService.AddGame(gameDto);

        await stepContext.Context.SendActivityAsync(MessageFactory.Text("Игра добавлена!"), cancellationToken);
        return await stepContext.EndDialogAsync(gameDto, cancellationToken);
    }


}
