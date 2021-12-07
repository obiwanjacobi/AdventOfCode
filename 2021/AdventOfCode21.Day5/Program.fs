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

printfn "%d" (Array.length vectors)
//printfn "%A" vectors

let map = 
    let plot map vector =
        let range st nd =
            if st > nd then
                [nd..st]
            else
                [st..nd]

        let plotLine xrange yrange map =
            for x in xrange do
                for y in yrange do
                    let value = Array2D.get map x y
                    Array2D.set map x y (value + 1)

        let yRange = range vector.orig.y vector.dest.y
        let xRange = range vector.orig.x vector.dest.x
        map |> plotLine xRange yRange
        map

    vectors
    |> Seq.fold plot (Array2D.zeroCreate 1000 1000)

let countGreaterThan map value =
    let isMatch x y = 
        value = Array2D.get map x y

    for x in [0..999]
        for y in [0..999]
            

//printfn "%A" map
