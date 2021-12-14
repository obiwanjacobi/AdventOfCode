// https://adventofcode.com/2021/day/10

open System
open FParsec

// part1

let lines = System.IO.File.ReadLines "input.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "example.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "debug.txt" |> Seq.toArray

// this can probably be done more efficient with a stack
// but I wanted to play with FParsec to see how that works

let test p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

let failedAt p str =
    match run p str with
    | Success(_, _, _)   -> 0
    | Failure(_, err, _) -> int err.Position.Column


type Token = None | Paren | Square | Brace | Angle
type UserState = unit
type Parser = Parser<Token, UserState>

// because parsing these brackets is recursive/nested
// we need to use this FParsec fn to create a parser hook that will complete the 'loop'
// the fst can be used as a reference, the snd (mutable) is later overwritten with the actual impl
// FParsec will forward any calls to the fst (ref) to the snd (impl)
let anyBracket, anyBracketImpl = createParserForwardedToRef()
let optionalAnyBracket = many anyBracket >>% Token.None
let paren: Parser = skipChar '(' >>. optionalAnyBracket .>> skipChar ')' >>% Token.Paren
let square: Parser = skipChar '[' >>. optionalAnyBracket .>> skipChar ']' >>% Token.Square
let braces: Parser = skipChar '{' >>. optionalAnyBracket .>> skipChar '}' >>% Token.Brace
let angle: Parser = skipChar '<' >>. optionalAnyBracket .>> skipChar '>' >>% Token.Angle
// overrides a reference cell (https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/reference-cells)
anyBracketImpl.Value <- choice [ paren; square; braces; angle; ]

//lines |> Seq.iter (fun line -> test anyBracket line)


let lookupScore c =
    match c with
    | ')' -> 3
    | ']' -> 57
    | '}' -> 1197
    | '>' -> 25137
    | _ -> failwith $"Unexpected bracket character: {c}"

let corruptedAt line =
    let col = failedAt anyBracket line
    if col > 0 && col < line.Length then
        lookupScore line[col - 1]
    else
        0

let score =
    lines 
    |> Seq.map corruptedAt
    |> Seq.sum

printfn "1) Score %d" score         // 323691


// part2

let incompleteLines =
    lines 
    |> Seq.filter (fun line -> corruptedAt line = 0)

// back to using a stack, I guess
// iterate chars in a line
// add opening brackets to a stack 
// pop when seeing the closed bracket
// when line ends, stack contains needed sequence in reverse to finish it


let isOpening c =
    match c with
    | '(' | '[' | '{' | '<' -> true
    | _ -> false

let toClosing c =
    match c with
    | '(' -> ')'
    | '[' -> ']'
    | '{' -> '}'
    | '<' -> '>'
    | _ -> failwith $"Unexpected bracket character: {c}"

let lookupPoints c =
    match c with
    | ')' -> 1
    | ']' -> 2
    | '}' -> 3
    | '>' -> 4
    | _ -> failwith $"Unexpected bracket character: {c}"

let complete line =
    let addOrRemove (stack: char list) (c: char) =
        if isOpening c then
            toClosing c :: stack
        else
            stack.Tail

    // WTF! Had to use in64 because F# overflowed the int32 - without warning!
    let calcPoints acc c =
        acc * 5L + int64 (lookupPoints c)

    line 
    |> Seq.fold addOrRemove []
    |> Seq.fold calcPoints 0L

let x = 
    let scorePoints = 
        incompleteLines 
        |> Seq.map complete
        |> Seq.sort
        |> Seq.toArray

    let index = (Array.length scorePoints) / 2
    scorePoints[index]

printfn "2) Middle Score Points %A" x       // 2858785164L