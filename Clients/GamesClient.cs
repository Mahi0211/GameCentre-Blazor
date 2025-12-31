using System.Text.Json;
using Microsoft.JSInterop;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient
{
    private const string StorageKey = "games";
    private readonly IJSRuntime js;

    private List<GameSummary> games = [];

    public GamesClient(IJSRuntime js)
    {
        this.js = js;
    }

    public async Task InitializeAsync()
    {
        var json = await js.InvokeAsync<string>(
            "gameStorage.get", StorageKey);

        if (!string.IsNullOrWhiteSpace(json))
        {
            games = JsonSerializer.Deserialize<List<GameSummary>>(json) ?? [];
        }
        else
        {
            games =
            [
                new()
                {
                    Id = 1,
                    Title = "Adventure Quest",
                    Genre = "Adventure",
                    Price = 29.99M,
                    ReleaseDate = new DateOnly(2023, 5, 15)
                }
            ];

            await SaveAsync();
        }
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(games);
        await js.InvokeVoidAsync("gameStorage.set", StorageKey, json);
    }

    public GameSummary[] GetGames() => [.. games];

    public async Task AddGameAsync(GameDetails game, Genre[] genres)
    {
        var genre = genres.Single(g => g.Id == int.Parse(game.GenreId!));

        games.Add(new GameSummary
        {
            Id = games.Any() ? games.Max(g => g.Id) + 1 : 1,
            Title = game.Title,
            Genre = genre.Name,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        });

        await SaveAsync();
    }

    public Task<GameDetails?> GetGameAsync(int id, Genre[] genres)
    {
        var game = games.SingleOrDefault(g => g.Id == id);

        if (game is null)
            return Task.FromResult<GameDetails?>(null);

        var genre = genres.Single(
            g => string.Equals(g.Name, game.Genre, StringComparison.OrdinalIgnoreCase)
        );

        var details = new GameDetails
        {
            Id = game.Id,
            Title = game.Title,
            GenreId = genre.Id.ToString(),
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };

        return Task.FromResult<GameDetails?>(details);
    }

    public async Task UpdateGameAsync(GameDetails game, Genre[] genres)
    {
        var genre = genres.Single(g => g.Id == int.Parse(game.GenreId!));
        var existing = games.Single(g => g.Id == game.Id);

        existing.Title = game.Title;
        existing.Genre = genre.Name;
        existing.Price = game.Price;
        existing.ReleaseDate = game.ReleaseDate;

        await SaveAsync();
    }

    public async Task DeleteGameAsync(int id)
    {
        games.RemoveAll(g => g.Id == id);
        await SaveAsync();
    }
}
