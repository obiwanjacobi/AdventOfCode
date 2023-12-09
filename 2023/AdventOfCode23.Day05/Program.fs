// https://adventofcode.com/2023/day/5
open System

//let lines = System.IO.File.ReadLines "sample.txt"
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

type Range =
  { Start: int64; End: int64; }
type MapEntry =
  { Source: Range; Destination: Range; Delta: int64 }
type Map =
  { Name: string; Entries: MapEntry list; }

let rec parseMapEntry index (entries: MapEntry list) (lines: string list) =
  if index >= List.length lines then
    entries
  else
    let line = lines.[index]
    if line.Length = 0 then
      entries
    else
      let parts = 
        line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun p -> p |> int64)

      let srcRange = { Start = parts.[1]; End = parts.[1] + parts.[2] }
      let destRange = { Start = parts.[0]; End = parts.[0] + parts.[2] }
      let entry = { Source = srcRange; Destination = destRange; Delta = (destRange.Start - srcRange.Start) }
      
      lines |> parseMapEntry (index + 1) (entries |> List.append (List.singleton entry))

let rec parseMap index (maps: Map list) (lines: string list) =
  if index >= List.length lines then
      maps
  else
    let nameLine = lines.[index]
    let entries = 
      lines 
      |> parseMapEntry (index + 1) []
      |> List.sortBy (fun e -> e.Source.Start)
    
    let l = nameLine.Length - 2 // chop off ':'
    let map = { Name = nameLine[0..l]; Entries = entries }
    
    // index + 2 => skip name line of this map + empty line at end of map entries
    lines  |> parseMap (index + List.length entries + 2) maps |> List.append (List.singleton map)
  
let seeds = 
  let numbers = (Array.ofSeq lines).[0].Split(':').[1];
  numbers.Trim().Split(' ') |> Array.map (fun n -> n |> int64)

let maps =
  lines
  |> List.ofSeq
  |> parseMap 2 []  // index=2 => skip seeds and empty line

//printf "%A" seeds
//printf "%A" maps

// --------------------------------------------------------
// part1

let translate srcVal map =
  let entries = (map.Entries |> List.filter (fun e -> e.Source.Start <= srcVal && e.Source.End >= srcVal))
  if List.length entries = 0 then
    srcVal  // unmapped values pass thru
  else
    let entry = entries[0]
    let offset = srcVal - entry.Source.Start
    entry.Destination.Start + offset

let rec translateMap srcVal index maps =
  if index >= List.length maps then
    srcVal
  else
    let dstVal = translate srcVal maps.[index]
    maps |> translateMap dstVal (index + 1)

let minLocation1 =
  seeds
  |> List.ofArray
  |> List.map (fun s -> translateMap s 0 maps)
  |> List.min

printf "Lowest location 1: %A\r\n" minLocation1

// --------------------------------------------------------
// part2

let one = int64(1)

let rec toRange list = 
  match list with
  | (x: int64)::(y: int64)::rest -> { Start = x; End = x + y - one }:: toRange rest
  | _ -> []

let seedsRanges = 
  seeds
  |> List.ofArray
  |> toRange

//printf "%A" seedsRanges

let intersectRange srcRng1 srcRng2 = 
  srcRng1.End >= srcRng2.Start && srcRng1.Start <= srcRng2.End

let subtractRange rng1 rng2 =
  if rng1.Start < rng2.Start && rng1.End > rng2.End then
    // |-----rng1----|
    //     |-rng2-|
    // |low|      |hi|
    let low = { Start = rng1.Start; End = rng2.Start - one }
    let hi = { Start = rng2.End + one; End = rng1.End }
    [low; hi]
  elif rng1.Start < rng2.Start && rng1.End <= rng2.End then
    // |-rng1-|
    //    |-rng2-|
    // |--|
    [{ Start = rng1.Start; End = rng2.Start - one}]
  elif rng1.Start > rng2.Start && rng1.End > rng2.End then
    //     |-rng1-|
    // |-rng2-|
    //        |---|
    [{ Start = rng2.End + one; End = rng1.End - one}]
  else
    []

let subtractRanges rngList rng =
  rngList 
  |> List.collect (fun r -> subtractRange rng r)
  
let translateEntry (mapEntry: MapEntry) srcRng =
  let startRng = max srcRng.Start mapEntry.Source.Start 
  let endRng = min srcRng.End mapEntry.Source.End
  let startOffset = Math.Abs(mapEntry.Source.Start - startRng)
  let endOffset = Math.Abs(mapEntry.Source.End - endRng)
  { Start = mapEntry.Destination.Start + startOffset; End = mapEntry.Destination.End - endOffset }

let translateIntersect srcRng map =
  // do srcRng match entry ranges?
  let entries = map.Entries |> List.filter (fun e -> intersectRange srcRng e.Source)
  if entries.Length = 0 then
    [srcRng]
  else
    // do srcRng not match entry ranges (what's left)?
    let thruRngs = subtractRanges (entries |> List.map(fun e -> e.Source)) srcRng
    let ranges = entries |> List.map (fun entry -> translateEntry entry srcRng)
    ranges |> List.append thruRngs


let rec intersectMap srcRng index maps =
  if index >= List.length maps then
    [srcRng]
  else
    let dstRng = translateIntersect srcRng maps.[index]
    dstRng |> List.collect (fun rng -> maps |> intersectMap rng (index + 1) )

let minLocation2 =
  seedsRanges
  |> List.collect (fun r -> intersectMap r 0 maps)
  |> List.minBy (fun r -> r.Start)
  
printf "Lowest location 2: %A\r\n" minLocation2.Start
