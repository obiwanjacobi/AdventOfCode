// https://adventofcode.com/2021/day/7

open System;

// part1

let lines = System.IO.File.ReadLines "input.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "example.txt" |> Seq.toArray

let values = 
    (lines |> Seq.head).Split ','
    |> Seq.map Int32.Parse
    |> Seq.toArray


printfn "%d values" (Seq.length values)
printfn ""

let positions = 
    [values |> Seq.min .. values |> Seq.max]

let calcFuelByPos target =
    let diff pos =
        abs (target - pos)
    
    values
    |> Seq.map diff
    |> Seq.sum
    
let (position1, fuel1) =
    let totalFuelForPos (pos, minFuel) value =
        let fuel = calcFuelByPos value
        if fuel < minFuel then
            (value, fuel)
        else
            (pos, minFuel)

    positions |> Seq.fold totalFuelForPos (0, Int32.MaxValue)

printfn "1) Best position %d for %d Fuel." position1 fuel1      // fuel=356922

// part2

let calcFuelByPos2 target =
    let diff pos =
        let len = abs (target - pos)
        // had to look this one up
        // n-th triangular number: (len * (len+1))/2
        (len * len + len) / 2
    
    values
    |> Seq.map diff
    |> Seq.sum

let (position2, fuel2) =
    let totalFuelForPos (pos, minFuel) value =
        let fuel = calcFuelByPos2 value
        if fuel < minFuel then
            (value, fuel)
        else
            (pos, minFuel)

    positions |> Seq.fold totalFuelForPos (0, Int32.MaxValue)

printfn "2) Best position %d for %d Fuel." position2 fuel2      // fuel=100347031
