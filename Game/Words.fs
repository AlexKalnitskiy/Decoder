module Game.Words

type RoundWords = private RoundWords of string array
let create generator = RoundWords [| generator; generator; generator; generator |]