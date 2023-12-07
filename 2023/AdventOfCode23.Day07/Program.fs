// https://adventofcode.com/2023/day/0

//let lines = System.IO.File.ReadLines "sample.txt"
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

let CardValues = [| '2'; '3'; '4'; '5'; '6'; '7'; '8'; '9'; 'T'; 'J'; 'Q'; 'K'; 'A' |]

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
  { Cards: string; Type: HandType; Bid: int; }

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

let parseHand (line: string) =
  let parts = line.Split(' ')
  let handType = determineHandType parts[0]
  { Cards = parts.[0]; Type = handType; Bid = int(parts.[1]); }

let hands = 
  lines 
  |> Seq.map (fun l -> parseHand l)
  |> Seq.toList

//printf "%A\r\n" hands

// --------------------------------------------------------
// part1

let compareHands hand1 hand2 =
  if (hand1.Type = hand2.Type) then
    hand2.Cards |> Seq.compareWith (fun h1 h2 -> 
      let i1 = CardValues |> Array.findIndex (fun c -> c = h1)
      let i2 = CardValues |> Array.findIndex (fun c -> c = h2)
      i1 - i2) hand1.Cards
  else
    (int(hand1.Type) - int(hand2.Type))

let winnings =
  hands
  |> List.sortWith (fun h1 h2 -> compareHands h1 h2)
  |> List.mapi (fun i h -> h.Bid * (i + 1))
  |> List.sum

printf "Winnings %A\r\n" winnings
