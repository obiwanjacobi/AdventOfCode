// https://adventofcode.com/2023/day/7

//let lines = System.IO.File.ReadLines "sample.txt"
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

let CardValues1 = [| '2'; '3'; '4'; '5'; '6'; '7'; '8'; '9'; 'T'; 'J'; 'Q'; 'K'; 'A' |]

type HandType = 
| FiveOfaKind = 7
| FourOfaKind = 6
| FullHouse = 5
| ThreeOfaKind = 4
| TwoPair = 3
| OnePair = 2
| HighCard = 1
| None = 0
type Hand =
  { Cards: string; Type: HandType; Bid: int; Jokers: int; AdjustedType: HandType; }

let determineHandType cards =
  
  let accumulateHandType handType cardCount =
    match handType with
    | HandType.None -> 
      match cardCount with
      | 1 -> HandType.HighCard
      | 2 -> HandType.OnePair
      | 3 -> HandType.ThreeOfaKind
      | 4 -> HandType.FourOfaKind
      | 5 -> HandType.FiveOfaKind
      | _ -> failwith "Initial cardCount too large!" 
    | HandType.OnePair when cardCount = 2 -> HandType.TwoPair
    | HandType.ThreeOfaKind when cardCount = 2 -> HandType.FullHouse
    | _ -> handType

  cards
  |> Seq.groupBy (fun c -> c)
  |> Seq.map (fun (k, l) -> Seq.length l)
  |> Seq.sortDescending
  |> Seq.fold (fun s l -> accumulateHandType s l) HandType.None

let upgradeHandType handType jokerCount =
  if jokerCount = 0 then
    handType
  else
    match handType with
    | HandType.HighCard when jokerCount = 1 -> HandType.OnePair
    | HandType.OnePair when jokerCount = 1 || jokerCount = 2 -> HandType.ThreeOfaKind
    | HandType.TwoPair when jokerCount = 1 -> HandType.FullHouse
    | HandType.TwoPair when jokerCount = 2 -> HandType.FourOfaKind
    | HandType.ThreeOfaKind when jokerCount = 1 || jokerCount = 3 -> HandType.FourOfaKind
    | HandType.FullHouse when jokerCount = 2 || jokerCount = 3 -> HandType.FiveOfaKind
    | HandType.FourOfaKind when jokerCount = 1 || jokerCount = 4 -> HandType.FiveOfaKind
    | _ -> handType

let parseHand (line: string) =
  let parts = line.Split(' ')
  let cards = parts.[0]
  let handType = determineHandType cards
  let jokers = cards |> Seq.filter (fun c -> c = 'J') |> Seq.length
  let jokerType = upgradeHandType handType jokers
  { Cards = parts.[0]; Type = handType; Bid = int(parts.[1]); Jokers = jokers; AdjustedType = jokerType; }

let hands = 
  lines 
  |> Seq.map (fun l -> parseHand l)
  |> Seq.toList

printf "%A\r\n" (hands |> List.filter (fun h -> h.Jokers > 0))

// --------------------------------------------------------
// part1

let compareHands1 hand1 hand2 =
  if (hand1.Type = hand2.Type) then
    hand2.Cards |> Seq.compareWith (fun h1 h2 -> 
      let i1 = CardValues1 |> Array.findIndex (fun c -> c = h1)
      let i2 = CardValues1 |> Array.findIndex (fun c -> c = h2)
      i1 - i2) hand1.Cards
  else
    (int(hand1.Type) - int(hand2.Type))

let winnings =
  hands
  |> List.sortWith (fun h1 h2 -> compareHands1 h1 h2)
  |> List.mapi (fun i h -> h.Bid * (i + 1))
  |> List.sum

printf "Winnings %A\r\n" winnings

// --------------------------------------------------------
// part2

let CardValues2 = [| 'J'; '2'; '3'; '4'; '5'; '6'; '7'; '8'; '9'; 'T'; 'J'; 'Q'; 'K'; 'A' |]

let compareHands2 hand1 hand2 =
  if (hand1.AdjustedType = hand2.AdjustedType) then
    hand2.Cards |> Seq.compareWith (fun h1 h2 -> 
      let i1 = CardValues2 |> Array.findIndex (fun c -> c = h1)
      let i2 = CardValues2 |> Array.findIndex (fun c -> c = h2)
      i1 - i2) hand1.Cards
  else
    (int(hand1.AdjustedType) - int(hand2.AdjustedType))

let jokerWinnings =
  hands
  |> List.sortWith (fun h1 h2 -> compareHands2 h1 h2)
  |> List.mapi (fun i h -> h.Bid * (i + 1))
  |> List.sum

printf "Winnings %A\r\n" jokerWinnings
