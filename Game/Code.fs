module Game.Code

open System.Collections.Generic
open Game.Extensions

type Code = private Code of int * int * int
let wordIndexes = [|1;2;3;4|]

let rec permute lst =
    match lst with
    | [] -> seq [ [] ]
    | _ ->
        seq {
            for i in 0 .. List.length lst - 1 do
                for perm in permute (List.take i lst @ List.skip (i + 1) lst) do
                    yield List.item i lst :: perm
        }

let generateCombinations () =
    let digits = [1; 2; 3; 4]
    permute digits
    |> Seq.toArray

let codesPool = generateCombinations() |> ArrayShuffle.shuffle |> Queue
let getCodeFrom (pool: Queue<int list>) =
    let nextCode = pool.Dequeue()
    Code (nextCode[0], nextCode[1], nextCode[2])