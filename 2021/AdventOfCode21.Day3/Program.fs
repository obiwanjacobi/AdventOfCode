// https://adventofcode.com/2021/day/3

// part1

let lines = System.IO.File.ReadLines "input.txt"
//let lines = System.IO.File.ReadLines "test.txt"
//lines |> Seq.iter(fun x -> printfn "%s" x)
//printfn "Number of input lines: %d" (lines |> Seq.length)


let count arr (b: string) =
    let countAt (i: int) (x: int): int =
        let index = i / 2
        let token = if i % 2 = 0 then '0' else '1'
        if b[index] = token then
            x + 1
        else
            x

    arr |> Seq.mapi countAt

let counts = lines |> Seq.fold count (Array.create 24 0)

//counts |> Seq.iter(fun x -> printfn "%d" x)

let normalize (arr: int[]) =
    if arr[0] > arr[1] then
        0
    else
        1

let binToInt (gamma: int, epsilon: int) (bit: int) =
    let inverted = ~~~bit &&& 1
    ((gamma <<< 1) ||| bit, (epsilon <<< 1) ||| inverted)

let (gamma, epsilon) =
    counts 
    |> Seq.splitInto 12
    |> Seq.map normalize 
    |> Seq.fold binToInt (0, 0)

printfn "Gamma: %d Epsilon: %d Power: %d" gamma epsilon (gamma * epsilon)


// part2

let filterOnCount index filterToken items =
    let countZerosAndOnes (zeros, ones) (item: string) =
        if item[index] = '0' then
            (zeros + 1, ones)
        else
            (zeros, ones + 1)

    if items |> Seq.length = 1 then
        items
    else
        let (zeros, ones) = items |> Seq.fold countZerosAndOnes (0, 0)
        let token = filterToken zeros ones
        items |> Seq.filter (fun item -> item[index] = token)

let rec recFilterOn index filterToken (items: seq<string>) =
    match index with
        | i when i >= 0 && i < 11 -> 
            items 
            |> filterOnCount i filterToken 
            |> recFilterOn (i + 1) filterToken
        | 11 ->
            items 
            |> filterOnCount index filterToken
        | _ -> failwith "invalid index"

let tokenMax zeros ones = if zeros > ones then '0' else '1'
let tokenMin zeros ones = if zeros <= ones then '0' else '1'

let parseBin (str: string) =
    let toBit value c =
        value <<< 1 ||| if c = '0' then 0 else 1
    str |> Seq.fold toBit 0

let oxygenGen = lines |> recFilterOn 0 tokenMax |> Seq.last |> parseBin
let co2scrubber = lines |> recFilterOn 0 tokenMin |> Seq.last |> parseBin

printfn "OxygenGen: %d CO2-scrubber %d Product: %d" oxygenGen co2scrubber (oxygenGen * co2scrubber)

