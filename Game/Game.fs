module Game

open System
open Game.Team

type Lobby =
    | Empty
    | Active of PlayerId list
    | Closed of PlayerId array
    
type LobbyCommand =
    | Connect of PlayerId
    | StartGame

let go lobby command =
    match lobby, command with
    | Empty, Connect player -> Active [player]
    | Active players, Connect player -> Active [player]

type Game =
    | NewGame
    | InitializedGame of InitializedGame
    | FinishedGame
and InitializedGame = { Id: Guid; RoundNumber: int; TeamOne: Team; TeamTwo: Team }

type RoundResult = RoundResult of int

type GameResult = GameResult of int

type GameEvent =
    | GameStart
    | RoundStart
    | RoundEnd of RoundResult
    | GameEnd of GameResult

type RoundEvent =
    | PickPlayer
    | GetCode
    | Encrypt
    | OwnGuess
    | EnemyGuess
    | RoundResult
    | GameResult
    
let go game event =
    match game, event with
    | NewGame, GameStart ->