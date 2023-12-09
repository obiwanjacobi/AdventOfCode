// https://adventofcode.com/2023/day/9
open System

//let lines = System.IO.File.ReadLines "sample.txt"
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

let parseLine (line: string) =
  line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
  |> Seq.map (fun n -> int(n))
  |> Seq.toList

let sequences = 
  lines
  |> Seq.toList
  |> List.map (fun l -> parseLine l)

//printf "%A" sequences

// --------------------------------------------------------
// part1

let seqIsAllZero sequence =
  // is there not an 'List.all' in F#?
  let zeros = sequence |> List.filter (fun n -> n = 0)
  sequence.Length = zeros.Length

let rec calcDeltas sequence deltas =
  if seqIsAllZero sequence then
    deltas
  else
    let seqDelta = 
      sequence
      |> List.pairwise 
      |> List.map (fun (n1, n2) -> n2 - n1)

    calcDeltas seqDelta ((seqDelta |> List.rev) :: deltas)

let makeResult sequence =
  calcDeltas sequence [sequence |> List.rev]

let seqResults =
  sequences
  |> List.map (fun s -> makeResult s)

//printf "%A" seqResults

let getExpandValue prevValue delta =
  prevValue + List.head delta

let rec expand deltas prevValue =
  if deltas = [] then
    prevValue
  else
    let newValue = getExpandValue prevValue deltas.Head
    expand deltas.Tail newValue

let expandSum =
  seqResults
  |> List.map (fun d -> expand d 0)
  |> List.sum

printf "Expand Sum 1: %A\r\n" expandSum

// --------------------------------------------------------
// part2

// just copied the code from 1
// could not be bothered to refactor into one codebase

let rec calcDeltas2 sequence deltas =
  if seqIsAllZero sequence then
    deltas
  else
    let seqDelta = 
      sequence
      |> List.pairwise 
      |> List.map (fun (n1, n2) -> n2 - n1)

    calcDeltas2 seqDelta ((seqDelta) :: deltas)

let makeResult2 sequence =
  calcDeltas2 sequence [sequence]

let seqResults2 =
  sequences
  |> List.map (fun s -> makeResult2 s)

let getExpandValue2 prevValue delta =
  List.head delta - prevValue

let rec expand2 deltas prevValue =
  if deltas = [] then
    prevValue
  else
    let newValue = getExpandValue2 prevValue deltas.Head
    expand2 deltas.Tail newValue

let expandSum2 =
  seqResults2
  |> List.map (fun d -> expand2 d 0)
  |> List.sum

printf "Expand Sum 2: %A\r\n" expandSum2
