using System;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GamesClient(HttpClient httpClient)
{
    private readonly List<GameSummary> games = [
    new(){
        Id = 1,
        Title = "Adventure Quest",
        Genre = "Adventure",
        Price = 29.99M,
        ReleaseDate = new DateOnly(2023, 5, 15)
    },
    new(){
        Id = 2,
        Title = "Racing Thunder",
        Genre = "Racing",
        Price = 49.99M,
        ReleaseDate = new DateOnly(2022, 11, 20)
    },
    new(){
        Id = 3,
        Title = "Mystery Manor",
        Genre = "Puzzle",
        Price = 19.99M,
        ReleaseDate = new DateOnly(2024, 2, 10)
    }
    ];

    private readonly Genre[] genres = new GenresClient(httpClient).GetGenres();

    public GameSummary[] GetGames() => [.. games];

    public void AddGame(GameDetails game)
    {
        Genre genre = GetGenreById(game.GenreId);

        var gameSummary = new GameSummary
        {
            Id = games.Count + 1,
            Title = game.Title,
            Genre = genre.Name,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };

        games.Add(gameSummary);
    }

    private Genre GetGenreById(string? id)
    {
        return genres.Single(genre => genre.Id == int.Parse(id ?? "0"));
    }

    public GameDetails GetGame(int id)
    {
        GameSummary game = GetGameSummaryById(id);

        var genre = genres.Single(genre => string.Equals(genre.Name, game.Genre, StringComparison.OrdinalIgnoreCase));

        return new GameDetails
        {
            Id = game.Id,
            Title = game.Title,
            GenreId = genre.Id.ToString(),
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public void UpdateGame(GameDetails updatedGame)
    {
        var genre = GetGenreById(updatedGame.GenreId);
        GameSummary existingGame = GetGameSummaryById(updatedGame.Id);

        existingGame.Title = updatedGame.Title;
        existingGame.Genre = genre.Name;
        existingGame.Price = updatedGame.Price;
        existingGame.ReleaseDate = updatedGame.ReleaseDate;
    }

    public void DeleteGame(int id)
    {
        GameSummary game = GetGameSummaryById(id);
        games.Remove(game);
    }

    private GameSummary GetGameSummaryById(int id)
    {
        GameSummary? game = games.Find(game => game.Id == id);
        ArgumentNullException.ThrowIfNull(game);
        return game;
    }
}
