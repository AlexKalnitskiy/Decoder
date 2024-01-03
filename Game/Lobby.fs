namespace Game

open System
open Game.Player


type Lobby = {
        Id: Guid
        Host: PlayerId
        CurrentPlayers: int
        MaxPlayers: int
        TeamOne: PlayerId list
        TeamTwo: PlayerId list
        Bench: PlayerId list
        State: LobbyState
}
and LobbyState =
    | Active
    | Closed
    | Started
    
type LobbyCommand =
    | Connect of PlayerId
    | PickTeamOne of PlayerId
    | PickTeamTwo of PlayerId
    | Leave of PlayerId
    | StartGame

type LobbyErrors =
    | LobbyIsFull
    | Unknown

module Lobby =
    
    let create server hostId = { Id = Guid.NewGuid(); Host = hostId; CurrentPlayers = 1; MaxPlayers = 8; TeamOne = [ hostId ]; TeamTwo = []; Bench = []; State = Active }
    
    let isFull lobby = lobby.CurrentPlayers = lobby.MaxPlayers
    
    let tryRemove predicate list = match List.tryFind predicate list with None -> (list, None) | Some found -> (List.filter predicate list, Some found)
    
    let withNewHost lobby =
        match
            lobby.TeamOne |> List.tryHead |> Option.orElseWith (fun () -> lobby.TeamTwo |> List.tryHead) |> Option.orElseWith (fun () -> lobby.Bench |> List.tryHead)
        with
            | Some newHostId -> { lobby with Host = newHostId }
            | None -> { lobby with State = Closed }
    
    let execute command lobby =
        match command with
        | Connect player -> if isFull lobby then Error LobbyIsFull else Ok { lobby with Bench = List.append lobby.Bench [player]; CurrentPlayers = lobby.CurrentPlayers + 1 }
        | PickTeamOne playerId ->
            if lobby.Bench |> List.contains playerId || lobby.TeamTwo |> List.contains playerId
            then
                Ok {
                    lobby with
                        Bench = lobby.Bench |> List.filter (fun x -> not (x = playerId))
                        TeamOne = lobby.TeamOne |> List.append [ playerId ]
                        TeamTwo = lobby.TeamTwo |> List.filter (fun x -> not (x = playerId))
                }
            else Ok lobby
        | PickTeamTwo playerId ->
            if lobby.Bench |> List.contains playerId || lobby.TeamTwo |> List.contains playerId
            then
                Ok {
                    lobby with
                        Bench = lobby.Bench |> List.filter (fun x -> not (x = playerId))
                        TeamOne = lobby.TeamOne |> List.filter (fun x -> not (x = playerId))
                        TeamTwo = lobby.TeamTwo |> List.append [ playerId ]
                }
            else Ok lobby
        | Leave playerId ->
            match lobby with
            | { lobby.Host = playerId } -> { lobby with
                                                CurrentPlayers = lobby.CurrentPlayers - 1
                                                Bench = lobby.Bench |> List.filter (fun x -> not (x = playerId))
                                                TeamOne = lobby.TeamOne |> List.filter (fun x -> not (x = playerId))
                                                TeamTwo = lobby.TeamTwo |> List.filter (fun x -> not (x = playerId)) } |> withNewHost |> Ok
            | { lobby.CurrentPlayers = 1 } -> Ok { lobby with State = Closed }
            | _ -> Error Unknown
        | StartGame -> Ok { lobby with State = Started }