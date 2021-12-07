// https://adventofcode.com/2021/day/4

open System;

// part1

let lines = System.IO.File.ReadLines "input.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "test.txt" |> Seq.toArray
//lines |> Seq.iter(fun x -> printfn "%s" x)
//printfn "Number of input lines: %d" (lines |> Seq.length)

let numbers = (lines |> Seq.head).Split ',' |> Seq.map Int32.Parse |> Seq.toArray

type BoardCell = { Value: int; Marked: bool }


let createBoard grid =
    let newRow (raw: string) =
        raw.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        |> Seq.map (fun v -> { Value = Int32.Parse v; Marked = false })
        |> Seq.toArray

    grid 
    |> Seq.skip 1 
    |> Seq.map newRow
    |> Seq.toArray
    |> Array.reduce Array.append

let initBoards =
    lines
    |> Seq.skip 1
    |> Seq.splitInto (((lines |> Seq.length) - 1) / 6)
    |> Seq.map createBoard

let boards = initBoards |> Seq.toArray

let numberOfBoards = boards |> Seq.length
//printfn "board count: %d" (numberOfBoards)


let printBoard board = 
    let printRow row = 
        let printCell cell = 
            let mark = if cell.Marked then "*" else " "
            printf "%d%s " cell.Value mark
        
        row |> Array.iter printCell
        printfn ""
    
    board
    |> Array.chunkBySize 5
    |> Array.iter printRow
    printfn ""

let markValue value board =
    let index = Array.FindIndex(board, (fun cell -> cell.Value = value))
    if index >= 0 then
        let cell = Array.get board index
        Array.set board index { cell with Marked = true }
    else
        ()

let markBoards value =
    boards |> Seq.iter (markValue value)
   
let isWinner cells =
    let areAllMarked cells =
        cells 
        |> Seq.map (fun c -> c.Marked)
        |> Seq.reduce (fun res mark -> res && mark)
    
    cells 
    |> Seq.map areAllMarked
    |> Seq.filter (fun b -> b)
    |> Seq.length > 0

let boardIsWinner board =
    let cols board =
        board
        |> Seq.chunkBySize 5
        // https://stackoverflow.com/questions/12766552/pivot-or-zip-a-seqseqa-in-f
        // I don't understand this fully
        |> Seq.collect Seq.indexed 
        |> Seq.groupBy fst // group by index
        |> Seq.map (snd >> Seq.map snd)
    let rows board = board |> Seq.chunkBySize 5

    board |> rows |> isWinner || board |> cols |> isWinner
    
let checkForWinners =
    boards |> Seq.filter boardIsWinner

let (value, winners) = 
    let processValue (winningValue, arr) value =
        if Array.length arr > 0 then
            (winningValue, arr)
        else
            markBoards value
            (value ,checkForWinners |> Seq.toArray)

    numbers 
    |> Seq.fold processValue (0, Array.empty)

printfn "Winning Value: %d" value
//printBoard leader

let sumUnmarked board =
    let addIfUnmarked sum cell =
        if cell.Marked then
            sum
        else
            sum + cell.Value

    board
    |> Seq.fold addIfUnmarked 0

let leader = winners |> Seq.head
let unmarkedSum = sumUnmarked leader
printfn "score1 %d" (value * unmarkedSum)   // 33348
printfn ""

// part2

let boards2 = initBoards |> Seq.toArray
boards2 |> Seq.iter printBoard


let markBoards2 value arr boards =
    let markBoard board =
        if board |> boardIsWinner then
            //printfn "Skip board for value: %d" value
            //printBoard board
            None
        else
            board |> markValue value
            if board |> boardIsWinner then
                //printBoard board
                Some board
            else
                None

    boards 
    |> Seq.map markBoard
    //|> Seq.filter (fun x -> x.IsSome)
    //|> Seq.map (fun x -> x.Value)
    |> Seq.choose id

let (value2, winners2) = 
    let processValue (winningValue, arr) value =
        if Array.length arr = numberOfBoards then
            (winningValue, arr)
        else
            //printfn "Value: %d" value
            let winners = markBoards2 value arr boards2 |> Seq.toArray
            (value, Array.append arr winners)

    numbers 
    |> Seq.fold processValue (0, Array.empty)


let loser = winners2 |> Seq.last
//printBoard loser
let unmarkedLoserSum = sumUnmarked loser

printfn "Winning Value2: %d" value2
printfn "score2 %d" (value2 * unmarkedLoserSum)
