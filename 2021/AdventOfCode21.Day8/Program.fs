// https://adventofcode.com/2021/day/8

open System;

// part1

let lines = System.IO.File.ReadLines "input.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "example.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "debug.txt" |> Seq.toArray

(*

 aaaa 
b    c 
b    c 
 dddd 
e    f 
e    f 
 gggg 

Note: seems zero 0 is not used and need not to be considered.

seg => digits
a => 2, 3, 5, 6, 7, 8, 9
b => 4, 5, 6, 8, 9
c => 1, 2, 3, 4, 7, 8, 9
d => 2, 3, 4, 5, 6, 8, 9
e => 2, 6, 8
f => 1, 3, 4, 5, 6, 7, 8, 9
g => 2, 3, 5, 6, 8, 9


#seg => digits
2 => 1
3 => 7
4 => 4
5 => 2, 3, 5
6 => 6, 9
7 => 8

*)

// => Detect 1, 4, 7 and 8 in output (after |)

type Signal = { Pattern: string[]; Output: string[] }

let parseSignal (txt: string) =
    let parts = txt.Split '|'
    let signals = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)
    let output = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
    { Pattern = signals; Output = output }

let signals = lines |> Seq.map parseSignal

let count1478 signals =
    let countByLength count signal =
        let countDigits count (txt: string) =
            match txt.Length with
            | 2 | 3 | 4 | 7 -> count + 1
            | _ -> count

        signal.Output |> Seq.fold countDigits count

    signals |> Seq.fold countByLength 0

let count = count1478 signals
printfn "1) 1, 4, 7 and 8 digit count: %d" count    // 294

// part2


let countMatchedChars (matchChars: string) (toThis: string) =
    if matchChars.Length > 0 then
        // I still think this is more readable
        // and it being mutable is contained in this function.
        let mutable count = 0
        for c in matchChars do
            if toThis.Contains c then
                count <- count + 1
    
        Some count
    else
        None


(*

easy digits: 1, 4, 7 (and 8 - but useless for decoding)
to decode: (2, 3, 5)-5 chars and (6, 9)-6 chars

2 => 1:1-match, 4:1-match, 7:2-match  | 6:3-match,  9:4-match
3 => 1:2-match, 4:3-match, 7:3-match  | 6:4-match,  9:5-match
5 => 1:1-match, 4:3-match, 7:2-match  | 6:5-match,  9:5-match

6 => 1:1-match, 4:2-match, 7:2-match  | 3:4-match,
9 => 1:2-match, 4:4-match, 7:3-match  | 3:5-match,

- if found one len6 is found then the other is also known.
- use the first part of the input (line) to discover the bits

*)

let decodeLen5use147 (display: string[]) seg =
    let match1 = countMatchedChars display[1] seg
    let match4 = countMatchedChars display[4] seg
    let match7 = countMatchedChars display[7] seg

    let m1is1 = match1.IsSome && match1.Value = 1
    let m1is2 = match1.IsSome && match1.Value = 2
    let m4is1 = match4.IsSome && match4.Value = 1
    let m4is3 = match4.IsSome && match4.Value = 3
    let m7is2 = match7.IsSome && match7.Value = 2
    let m7is3 = match7.IsSome && match7.Value = 3

    if m4is3 && (m7is2 || m1is1) then
        Some '5'
    elif m7is3 || m1is2 then
        Some '3'
    elif m4is1 || (m1is1 && m7is2) then
        Some '2'
    else
        None

let decodeLen5use69 (display: string[]) seg =
    let match6 = countMatchedChars display[6] seg
    let match9 = countMatchedChars display[9] seg
    if match6.IsSome && match6.Value = 3 then
        Some '2'
    elif match6.IsSome && match6.Value = 4 then
        Some '3'
    elif match6.IsSome && match6.Value = 5 then
        Some '5'
    elif match9.IsSome && match9.Value = 4 then
        Some '2'
    else
        None
        
let decodeLen6 (display: string[]) seg =
    let match1 = countMatchedChars display[1] seg
    let match4 = countMatchedChars display[4] seg
    let match7 = countMatchedChars display[7] seg

    if match4.IsSome && match4.Value = 2 then
        Some '6'
    elif match4.IsSome && match4.Value = 4 then
        Some '9'
    elif match7.IsSome && match7.Value = 2 then
        Some '6'
    elif match7.IsSome && match7.Value = 3 then
        Some '9'
    elif match1.IsSome && match1.Value = 1 then
        Some '6'
    elif match1.IsSome && match1.Value = 2 then
        Some '9'
    else
        None

let decodeStage1 (display: string[]) (seg: string) =
    match seg.Length with
    | 2 -> Some '1'
    | 3 -> Some '7'
    | 4 -> Some '4'
    | 7 -> Some '8'
    | _ -> None

let decodeStage2 (display: string[]) (seg: string) =
    match seg.Length with
    | 5 -> decodeLen5use147 display seg
    | 6 -> decodeLen6 display seg
    | _ -> failwith "invalid segment length"
    

let intToChar value =
    match value with
    | 1 -> Some '1'
    | 2 -> Some '2'
    | 3 -> Some '3'
    | 4 -> Some '4'
    | 5 -> Some '5'
    | 6 -> Some '6'
    | 7 -> Some '7'
    | 8 -> Some '8'
    | 9 -> Some '9'
    | _ -> None

let runDecode (display: string[]) (segPairs) (fnDecode:string[] -> string -> char option) =
    let decode index = 
        let (seg, code: char option) = Array.get segPairs index
        if code.IsNone then
            let dispIndex = Array.IndexOf(display, seg)
            if dispIndex > -1 then
                Array.set segPairs index (seg, intToChar dispIndex)
            else
                let c = fnDecode display seg
                if c.IsSome then
                    Array.set segPairs index (seg, c)
                    let s = c.Value.ToString();
                    let i = Int32.Parse(s)
                    Array.set display i seg

    for i in [0..13] do decode i

let decodeKnown display segPairs = runDecode display segPairs decodeStage1
let decodeUnknown display segPairs = runDecode display segPairs decodeStage2

let isDecoded (seg, code: char option) = code.IsSome

let outputsAreDecoded (segPairs:(string * char option)[]) =
    segPairs[10..13] // output sub range
    |> Seq.forall isDecoded

let decodeStages display segPairs =
    decodeKnown display segPairs
    // TODO: if hangs here, need more decoding rules?
    while outputsAreDecoded segPairs = false do
        decodeUnknown display segPairs

let decodeDisplay signal =
    let makeDisplay segs =
        let display = Array.create<string> 10 ""  // pos 0 is unused
        let segPairs = (segs |> Array.map (fun s -> (s, None)))
        decodeStages display segPairs
        segPairs[10..13]

    let output = makeDisplay (Array.append signal.Pattern signal.Output)

    let codeToString (str: string) (seg, code: char option) =
        str + code.Value.ToString()
    
    
    output 
    |> Array.fold codeToString ""
    |> Int32.Parse
    
let result = signals |> Seq.map decodeDisplay

result |> Seq.iter (fun x -> printf "%d, " x)
printfn ""
printfn "Sum %d" (result |> Seq.sum)    // 1191290 too high (example.txt works)
