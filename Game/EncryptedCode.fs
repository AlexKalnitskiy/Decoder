module Game.EncryptedCode

open Game.Code

type EncryptedCode = private EncryptedCode of Code * string array

let create code e1 e2 e3 = EncryptedCode (code, [|e1; e2; e3|])