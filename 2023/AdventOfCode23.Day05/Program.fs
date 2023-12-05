// https://adventofcode.com/2023/day/5
open System

//let lines = System.IO.File.ReadLines "sample.txt"
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

type Range =
  { Start: int64; End: int64; }
type MapEntry =
  { Source: Range; Destination: Range; }
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
      let entry = { Source = srcRange; Destination = destRange; }
      
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

let minLocation =
  seeds
  |> List.ofArray
  |> List.map (fun s -> translateMap s 0 maps)
  |> List.min

printf "Lowest location: %A\r\n" minLocation