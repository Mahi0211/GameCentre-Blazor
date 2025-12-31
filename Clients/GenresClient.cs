using System;
using GameStore.Frontend.Models;

namespace GameStore.Frontend.Clients;

public class GenresClient
{
    private readonly List<Genre> genres = [
        new(){
        Id = 1,
        Name = "Adventure"
    },
    new(){
        Id = 2,
        Name = "Racing"
    },
    new(){
        Id = 3,
        Name = "Puzzle"
    },
    new(){
        Id = 4,
        Name = "Action"
    },
    new(){
        Id = 5,
        Name = "Sports"
    }
        ];

    public Genre[] GetGenres() => [.. genres];
}
