// https://adventofcode.com/2023/day/6
open System

//let lines = Array.ofSeq (System.IO.File.ReadLines "sample.txt")
let lines = Array.ofSeq (System.IO.File.ReadLines "input.txt")

// input parsing

let times = lines.[0].Substring(11).Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun t -> t |> int)
let distances = lines.[1].Substring(11).Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun t -> t |> int)
let games = Array.zip times distances

// --------------------------------------------------------
// part1

let countWins (time: int) (distance: int) =
  [0..time]
  |> List.map (fun t -> (time - t) * t) // rest-time * speed
  |> List.filter (fun d -> d > distance)
  |> List.length

let winProduct =
  games
  |> List.ofArray
  |> List.map (fun (t, d) -> countWins t d)
  |> List.reduce (fun c a -> c * a)

printf "Product of Win counts: %A\r\n" winProduct

// --------------------------------------------------------
// part2

// feels like there should be some kind of formula to get to the answer
// but all I can think of is a brute force solution...

// same as countWins but 64 bits
let countWins2 (time: int64) (distance: int64) =
  let times = int64(1) |> Seq.unfold (fun state ->
    if state < time then Some (state, state + int64(1)) else None)
  
  times
  |> Seq.map (fun t -> (time - t) * t) // rest-time * speed
  |> Seq.filter (fun d -> d > distance)
  |> Seq.length

let time = lines.[0].Split(':').[1].Replace(" ", "") |> int64
let distance = lines.[1].Split(':').[1].Replace(" ", "") |> int64
let winCount = countWins2 time distance

printf "Win count: %A\r\n" winCount