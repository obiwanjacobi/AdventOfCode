// https://adventofcode.com/2023/day/1
// part1

//let lines = System.IO.File.ReadLines "sample1.txt"
let lines = System.IO.File.ReadLines "input.txt"

let scanValue line = 
  let firstDigit = line |> Seq.find System.Char.IsDigit
  let lastDigit = line |> Seq.rev |> Seq.find System.Char.IsDigit
  System.Int32.Parse $"{firstDigit}{lastDigit}"

let sum = 
  List.ofSeq lines
  |> List.map (fun l -> scanValue l)
  |> List.sum

printf "\r\nPart 1: Total sum: %d\r\n" sum

// part2

let lines2 = lines;
//let lines2 = System.IO.File.ReadLines "sample2.txt"
let words = ["zero"; "one"; "two"; "three"; "four"; "five"; "six"; "seven"; "eight"; "nine"]

let tryParseWord (txt: string) =
  let index = words |> List.tryFindIndex (fun w -> txt.StartsWith w)
  match index with
    | Some i -> Some i
    | _ -> None

let tryParseDigit (txt: string) =
  if System.Char.IsDigit txt[0] then Some ((int)(txt[0] - '0')) else None

let tryParseDigitOrWord txt = 

    let digitResult = tryParseDigit txt

    if digitResult.IsSome then
      digitResult
    else
      let wordResult = tryParseWord txt

      if wordResult.IsSome then
        wordResult
      else
          None

let parseValue (line: string) =
  let mutable digits = []
  let mutable index = 0
  while index < line.Length do
    let result = tryParseDigitOrWord line[index..]
    match result with
      | Some digit -> 
        digits <- List.append digits [digit]
        index <- index + 1
      | None -> index <- index + 1
  
  let value = System.Int64.Parse $"{digits.[0]}{digits.[digits.Length - 1]}"
  //printf "%s => %d\r\n" line value
  value

let sum2 = 
  List.ofSeq lines2 
  |> List.map (fun l -> parseValue l)
  |> List.sum

printf "\r\nPart 2: Total sum: %d\r\n" sum2
