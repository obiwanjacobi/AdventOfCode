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

let countWinningNumbers card =
  (card.Numbers |> Array.filter (fun n -> card.Winning |> Array.contains n)).Length
  
let winningNumberPoints card =
  ((2.0 ** countWinningNumbers card) |> int) / 2

let winningNumbersSum =
  cards 
  |> Seq.map (fun c -> winningNumberPoints c)
  |> Seq.sum

printf "Sum of Winning Number points: %A\r\n" winningNumbersSum

// --------------------------------------------------------
// part2

type CountedCard(c: Card) =
  let mutable _instances = 1
  
  member this.Card = c
  member this.Instances
    with get() = _instances
    and set(value) =  _instances <- value

let countedCards = 
  cards 
  |> Array.ofSeq
  |> Array.map (fun c -> new CountedCard(c))

let collectCardInstancess cards (card: CountedCard) =
  let winningCount = countWinningNumbers card.Card

  cards 
  |> Array.skipWhile (fun c -> c <> card)
  |> Array.skip 1
  |> Array.take winningCount
  |> Array.iter (fun cc -> cc.Instances <- cc.Instances + card.Instances)

let countWinningCards =
  countedCards
  |> Array.iter (fun cc -> collectCardInstancess countedCards cc)
  
  countedCards
  |> Array.map (fun cc -> cc.Instances)
  |> Array.sum

printf "Total number of winning cards: %A\r\n" countWinningCards
