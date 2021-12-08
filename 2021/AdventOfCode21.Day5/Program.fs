// https://adventofcode.com/2021/day/5

open System;

// part1

let lines = System.IO.File.ReadLines "input.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "test.txt" |> Seq.toArray
//lines |> Seq.iter(fun x -> printfn "%s" x)
//printfn "Number of input lines: %d" (lines |> Seq.length)

type Point = { x: int; y: int }
type Vector = { orig: Point; dest: Point }

let parseVector (txt: string) =
    let parsePoint (pt: string) = 
        let cs = pt.Split ','
        { x = Int32.Parse cs[0]; y = Int32.Parse cs[1] }

    let points = txt.Split " -> "
    { orig = parsePoint points[0]; dest = parsePoint points[1] }

let vectorIsOnX vector = vector.orig.x = vector.dest.x
let vectorIsOnXorY vector =
    vectorIsOnX vector || vector.orig.y = vector.dest.y

let vectors = 
    lines 
    |> Seq.map parseVector 
    |> Seq.filter vectorIsOnXorY
    |> Seq.toArray

//printfn "%d" (Array.length vectors)
//printfn "%A" vectors

let plotMap vectors = 
    let plot map vector =
        let range st nd =
            if st > nd then
                [|nd..st|]
            else
                [|st..nd|]

        let plotLine xrange yrange =
            let plotPoint x y =
                let value = Array2D.get map x y
                Array2D.set map x y (value + 1)

            let length = Array.length xrange
            if length = Array.length yrange then
                for i in [0..length - 1] do
                    let x = Array.get xrange i
                    let y = Array.get yrange i
                    plotPoint x y
            else
                for x in xrange do
                    for y in yrange do
                        plotPoint x y

        let yRange = range vector.orig.y vector.dest.y
        let xRange = range vector.orig.x vector.dest.x
        plotLine xRange yRange
        map

    vectors |> Seq.fold plot (Array2D.zeroCreate 1000 1000)

let countGreaterThan map value =
    let isGreaterThan x y = 
        value <= Array2D.get map x y

    // non-functional short cut
    let mutable count = 0
    for x in [0..999] do
        for y in [0..999] do
            if isGreaterThan x y then
                count <- count + 1

    count


let printMap map c =
    assert (c < 1000)
    for x in [0..c] do
        for y in [0..c] do
            printf "%d" (Array2D.get map x y)
        printfn ""

//printfn "%d" (Seq.length vectors)
let count = countGreaterThan (plotMap vectors) 2
printfn "Count1 %d" count   // 6687



// part2

let vectorIsOnXorYorDiag vector =
    let vectorIsDiag =
        let dx = abs (vector.orig.x - vector.dest.x)
        let dy = abs (vector.orig.y - vector.dest.y)
        dx = dy
    vectorIsOnXorY vector || vectorIsDiag


let vectors2 = 
    lines 
    |> Seq.map parseVector 
    |> Seq.filter vectorIsOnXorYorDiag
    |> Seq.toArray

printfn "%d" (Seq.length vectors2)
let map = plotMap vectors2
let count2 = countGreaterThan map 2
//printMap map 100
printfn "Count2 %d" count2  // 23512 too high! :-(
