// https://adventofcode.com/2021/day/9

open System;

// part1

let lines = System.IO.File.ReadLines "input.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "example.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "debug.txt" |> Seq.toArray

let mapHeight = Array.length lines
let mapWidth = (lines |> Array.head).Length

let (_, heightMap) = 
    let writeHeights (index, map) (line: string) =
        let charToInt c =
            match c with
            | '0' -> 0
            | '1' -> 1
            | '2' -> 2
            | '3' -> 3
            | '4' -> 4
            | '5' -> 5
            | '6' -> 6
            | '7' -> 7
            | '8' -> 8
            | '9' -> 9
            | _ -> failwith "Invalid character."

        let copyRowInto arrTarget2D index arrSource =
            let mutable col = 0
            for i in [0..(Array.length arrSource) - 1] do
                Array2D.set arrTarget2D col index (Array.get arrSource i)
                col <- col + 1

        let row = line |> Seq.map charToInt |> Seq.toArray
        copyRowInto map index row
        (index + 1, map)

    lines |> Seq.fold writeHeights (0, (Array2D.create<int> mapWidth mapHeight 0))

let isLowPoint x y =
    let top = (x, if y = 0 then y else y - 1)
    let left = ((if x = 0 then x else x - 1), y)
    let right = ((if x = mapWidth - 1 then x else x + 1), y)
    let bottom = (x, if y = mapHeight - 1 then y else y + 1)

    let valPt = Array2D.get heightMap x y
    let valTop = Array2D.get heightMap (fst top) (snd top)
    let valLeft = Array2D.get heightMap (fst left) (snd left)
    let valRight = Array2D.get heightMap (fst right) (snd right)
    let valBottom = Array2D.get heightMap (fst bottom) (snd bottom)

    let rightMost = fst right = x && snd right = y
    let bottomMost = fst bottom = x && snd bottom = y

    let ret = 
        valPt <= valTop && 
        valPt <= valLeft &&
        (valPt < valRight || rightMost) &&
        (valPt < valBottom || bottomMost)

    ret

let determineLowPoints =
    let mutable lowPoints = Array.empty
    for y in [0..mapHeight - 1] do
        for x in [0..mapWidth - 1] do
            if isLowPoint x y = true then
                lowPoints <- Array.append lowPoints (Array.singleton (x, y))
    lowPoints


let lowPoints = determineLowPoints
//printfn "%A" lowPoints

let printHeightMap lowPoints =
    for y in [0..mapHeight - 1] do
        for x in [0..mapWidth - 1] do
            let i = Array.FindIndex(lowPoints, (fun (lpx, lpy) -> lpx = x && lpy = y))
            let value = Array2D.get heightMap x y
            if i > -1 then 
                printf "%d* " value
            else
                printf "%d  " value
        printfn ""

//printHeightMap lowPoints


let calcRiskLevel lowPoints =
    let riskLvl (x, y) =
        let value = Array2D.get heightMap x y
        value + 1

    lowPoints 
    |> Array.map riskLvl
    |> Array.sum

let riskLevel = calcRiskLevel lowPoints
printfn "1) Risk Level %d" riskLevel      // 496

// part2

// start at lowpoints and recursively move in 4 directions to discover <>9 fields

let getValue x y = Array2D.get heightMap x y

let findNeighborPos (x, y) =
    let mutable neighbors = []

    if x > 0 then
        neighbors <- (x - 1, y) :: neighbors
    if x < mapWidth - 1 then
        neighbors <- (x + 1, y) :: neighbors
    if y > 0 then
        neighbors <- (x, y - 1) :: neighbors
    if y < mapHeight - 1 then
        neighbors <- (x, y + 1) :: neighbors

    neighbors

let calcBasinSize (x, y) =
    let visitedPos = Set.singleton (x, y)

    let rec recVisit visitedCells =
        let neighborPos = 
            visitedCells 
            // use visited to expand neighbors (lots of double will be filtered out later)
            |> Set.fold (fun s pos -> Set.union s (Set.ofList (findNeighborPos pos))) Set.empty

        let remainingPos = 
            Set.difference neighborPos visitedCells
            |> Set.filter (fun (x, y) -> getValue x y <> 9)

        match remainingPos with
        | e when e.IsEmpty -> visitedCells
        | p -> recVisit (Set.union visitedCells p)

    recVisit visitedPos
    |> Set.toList
    |> List.length

let topThreeBasinSizeProduct =
    lowPoints
    |> Seq.map calcBasinSize
    |> Seq.sortDescending
    |> Seq.take 3
    |> Seq.reduce (*)

printfn "2) Product %d" topThreeBasinSizeProduct        // 902880