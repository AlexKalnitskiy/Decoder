namespace Game

open System
open Game.Team

type Lobby =
    | ActiveLobby of ActiveLobby
    | ClosedLobby of ClosedLobby
and ActiveLobby =
    {
        Id: Guid
        Host: PlayerId
        TeamOne: PlayerId list
        TeamTwo: PlayerId list
        Bench: PlayerId list
    }
and ClosedLobby =
    {
        Id: Guid
        Host: PlayerId
        TeamOne: PlayerId list
        TeamTwo: PlayerId list
    }
    
type LobbyCommand =
    | Connect of PlayerId
    | PickTeamOne of PlayerId
    | PickTeamTwo of PlayerId
    | StartGame of PlayerId

module Lobby =
    let execute command lobby =
        match command with
        | Connect player -> { lobby with Bench = List.append lobby.Bench [player] }
        | PickTeamOne player -> failwith "todo"
        | PickTeamTwo player -> failwith "todo"
        | StartGame player -> failwith "todo"
        