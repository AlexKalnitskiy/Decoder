module Game.Team

open Game.EncryptedCode
open Game.Player

type Team =
    {
        Name: string
        CurrentPlayer: int
        Players: PlayerId array
        OwnHistory: EncryptedCode array
        EnemyHistory: EncryptedCode array
        Points: int
    }

let create name players = { Name = name; CurrentPlayer = 0; Players = players; OwnHistory = Array.empty; EnemyHistory = Array.empty; Points = 0 }