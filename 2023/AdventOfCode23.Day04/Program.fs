// https://adventofcode.com/2023/day/4

//let lines = System.IO.File.ReadLines "sample.txt"
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

type Card = 
  { Id: int; Winning: int array; Numbers: int array; }

let parseNumbers (line: string) = 
  line.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries) 
  |> Array.map (fun n -> n |> int)

let parseCard (line: string) =
  let parts = line.Split(':')
  let id = parts.[0][5..] |> int
  let numberParts = parts.[1].Trim().Split('|')
  let winning = parseNumbers numberParts.[0]
  let numbers = parseNumbers numberParts.[1]

  { Id = id; Winning = winning; Numbers = numbers; }

// --------------------------------------------------------
// part1

let cards =
  lines |> Seq.map (fun l -> parseCard l)

let winningNumberPoints card =
  let winningNumbers = card.Numbers |> Array.filter (fun n -> card.Winning |> Array.contains n)
  (2.0 ** winningNumbers.Length) / 2.0 |> int

let winningNumbersSum =
  cards 
  |> Seq.map (fun c -> winningNumberPoints c)
  |> Seq.sum

printf "Sum of Winning Number points: %A" winningNumbersSum

// --------------------------------------------------------
// part2


