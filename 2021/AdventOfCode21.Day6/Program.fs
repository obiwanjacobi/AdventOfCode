// https://adventofcode.com/2021/day/6

open System;

// part1

let lines = System.IO.File.ReadLines "input.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "test.txt" |> Seq.toArray
//lines |> Seq.iter(fun x -> printfn "%s" x)
//printfn "Number of input lines: %d" (lines |> Seq.length)

let fishState =
    (lines |> Seq.head).Split ','
    |> Seq.map Int32.Parse
    |> Seq.toArray

printfn "Fish Count (initial): %d" (Array.length fishState)

let processDay fishState =
    let fishRule spawn fishValue =
        if fishValue = 0 then
            (6, spawn |> Array.insertAt 0 8)
        else
            (fishValue - 1, spawn)

    let (result, spawn) = fishState |> Array.mapFold fishRule Array.empty
    Array.append result spawn
    
let rec recProcessDays day fishState =
    match day with
    | 0 -> fishState |> processDay
    | _ -> fishState |> processDay |> recProcessDays (day - 1)

let result = fishState |> recProcessDays (80 - 1)  // -1: we go down to 0

printfn "Fish Count (80 days): %d" (Array.length result)    // 360761


// part2

// brute force - takes forever
//let result2 = fishState |> recProcessDays (256 - 1)  // -1: we go down to 0

let countFishTimerVals (countArr: int64[]) index =
    let cnt = Array.get countArr index
    Array.set countArr index (cnt + 1L)
    countArr

let timerCounts = fishState |> Array.fold countFishTimerVals (Array.zeroCreate<int64> 9)

let processDay2 timerValues =
    let reset = Array.head timerValues
    let newValues = timerValues |> Array.removeAt 0
    let countAt6 = Array.get newValues 6
    Array.set newValues 6 (countAt6 + reset)
    Array.append newValues (Array.singleton reset)
    
let rec recProcessDays2 day timerValues =
    match day with
    | 0 -> timerValues |> processDay2
    | _ -> timerValues |> processDay2 |> recProcessDays2 (day - 1)

let result2 = timerCounts |> recProcessDays2 (256 - 1)  // -1: we go down to 0

printfn "Fish Count (256 days): %d" (Array.sum result2)    // 1632779838045
