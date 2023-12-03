// https://adventofcode.com/2023/day/3

//let lines = System.IO.File.ReadLines "sample.txt"
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

type PartNumber =
  { Number: int; Span: (int * int); }
type Symbol =
  { Symbol: char; Index: int; }
type SchemaLine =
  { PartNumbers: PartNumber[]; Symbols: Symbol[]; }

let isSymbol c =
  not (System.Char.IsLetterOrDigit c) && c <> '.'

let parsePartNumber (line: string) index =
  let start = index
  let mutable stop = index
  while stop < line.Length && System.Char.IsDigit line.[stop] do
    stop <- stop + 1

  let number = line[start..stop - 1] |> int
  { Number = number; Span = (start, stop - 1); }

// example to perhaps rewrite parseLine with?
//let rec indexOfMany (s:string) (i:int) (acc: int list) =
//    match s.IndexOf('a', i) with
//    | -1 -> acc
//    | x -> indexOfMany s (x + 1) (x :: acc)
//
//let x = indexOfMany "abracadabra" 0 []

let parseLine (line: string) =
  let mutable partNumbers = [||]
  let mutable symbols = [||]
  let mutable index = 0

  while index < line.Length do
    if System.Char.IsDigit line.[index] then
      let partNo = parsePartNumber line index
      partNumbers <- partNumbers |> Array.append [|partNo|]
      let (start, stop) = partNo.Span
      index <- stop + 1
    elif isSymbol line.[index] then
      let symbol = { Symbol = line.[index]; Index = index; }
      symbols <- symbols |> Array.append [|symbol|]
      index <- index + 1
    else
      index <- index + 1

  { PartNumbers = partNumbers; Symbols = symbols; }

let schemaLines = 
  lines 
  |> Array.ofSeq
  |> Array.map parseLine

//printf "%A" schemaLines

// --------------------------------------------------------
// part 1

// note that the first and last line are not processed for 'adjacentSpan'
// still the answer is correct

let symbolRowIndex = 1

let adjacentSpan span index =
  let (start, stop) = span
  // same row as symbol so has to 'connect'
  index = start - 1 || index = stop + 1

let intersectSpan span index =
  let (start, stop) = span
  // extra margin (+/- 1) for diagonal match
  index >= start - 1 && index <= stop + 1

let intersectPartNumbers symbolIndex lineIndex partNumbers =
  partNumbers
  |> Array.filter (fun no ->
    if lineIndex = symbolRowIndex then
      adjacentSpan no.Span symbolIndex
    else
      intersectSpan no.Span symbolIndex)

let adjacentPartNumbers index threeLines =
  threeLines
  |> Array.indexed
  |> Array.collect (fun (i, l) -> l.PartNumbers |> intersectPartNumbers index i)
  |> Array.map (fun no -> no.Number)

let symbolAdjacentPartNumbers (threeLines: SchemaLine array) =
  threeLines.[symbolRowIndex].Symbols
  |> Array.collect (fun s -> adjacentPartNumbers s.Index threeLines)
  
let partNumbers =
  schemaLines
  |> Array.windowed 3
  |> Array.collect symbolAdjacentPartNumbers

//printf "%A" partNumbers

let sum = partNumbers |> Array.sum
printf "Part Number Sum: %A\r\n" sum

// --------------------------------------------------------
// part 2

let gearSymbol = '*'

let gearAdjacentPartNumbers (threeLines: SchemaLine array) =
  threeLines.[symbolRowIndex].Symbols
  |> Array.filter (fun s -> s.Symbol = gearSymbol)
  |> Array.map (fun s -> adjacentPartNumbers s.Index threeLines)
  |> Array.filter (fun r -> r.Length = 2)
  |> Array.map(fun no -> no.[0] * no.[1])

let gearRatios =
  schemaLines
  |> Array.windowed 3
  |> Array.collect gearAdjacentPartNumbers

let gearSum = gearRatios |> Array.sum
printf "Gear Ratio Sum: %A\r\n" gearSum
