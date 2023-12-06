// https://adventofcode.com/2023/day/6
open System

//let lines = Array.ofSeq (System.IO.File.ReadLines "sample.txt")
let lines = Array.ofSeq (System.IO.File.ReadLines "input.txt")

// input parsing

let times = lines.[0].Substring(11).Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun t -> t |> int)
let distances = lines.[1].Substring(11).Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun t -> t |> int)
let games = Array.zip times distances

//printf "Times: %A" times
//printf "Distances: %A" distances

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
