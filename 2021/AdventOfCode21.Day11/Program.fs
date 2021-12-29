// https://adventofcode.com/2021/day/11

open System


// part1

//let lines = System.IO.File.ReadLines "input.txt" |> Seq.toArray
let lines = System.IO.File.ReadLines "example.txt" |> Seq.toArray
//let lines = System.IO.File.ReadLines "debug.txt" |> Seq.toArray

let grid = 
    lines
    |> Seq.map (fun line -> line.ToCharArray())
    |> array2D
    |> Array2D.map (fun c -> Int32.Parse(c.ToString()))

let maxWidth = Array2D.length1 grid
let maxHeight = Array2D.length2 grid


let printGrid grid =
    for x = 0 to (Array2D.length2 grid) - 1 do
        for y = 0 to (Array2D.length1 grid) - 1 do
            printf "%d" grid[x, y]
        printfn ""


(*
    (x-1, y-1) | (x, y-1) | (x+1, y-1)
    (x-1, y)   | (x, y)   | (x+1, y)
    (x-1, y+1) | (x, y+1) | (x+1, y+1)
*)
let neighbors (x, y) =
    let mutable poslist = []
    
    if x > 0 then
        poslist <- (x - 1, y) :: poslist
        if y > 0 then
            poslist <- (x - 1, y - 1) :: poslist
        if y < maxHeight - 1 then
            poslist <- (x - 1, y + 1) :: poslist
    
    if x < maxWidth - 1 then
        poslist <- (x + 1, y) :: poslist
        if y > 0 then
            poslist <- (x + 1, y - 1) :: poslist
        if y < maxHeight - 1 then
            poslist <- (x + 1, y + 1) :: poslist
    
    if y > 0 then
        poslist <- (x, y - 1) :: poslist
    if y < maxHeight - 1 then
        poslist <- (x, y + 1) :: poslist

    Set.ofList poslist

let increment x y (grid: int[,]) = grid[x, y] <- grid[x, y] + 1
let incrementGrid (grid: int[,]) =
    for y = 0 to (Array2D.length2 grid) - 1 do
        for x = 0 to (Array2D.length1 grid) - 1 do
            increment x y grid

let compute (grid: int[,]) =
    let flash grid' x y v =
        if v > 9 then
            let neighbors = neighbors (x, y) 
            neighbors |> Seq.iter (fun (x, y) -> increment x y grid')
    let rec recFlash =
        
    let countFlashes grid' =
        let mutable flashCount = 0
        for y = 0 to (Array2D.length2 grid') - 1 do
            for x = 0 to (Array2D.length1 grid') - 1 do
                if grid'[x, y] > 9 then
                    grid'[x, y] <- 0
                    flashCount <- flashCount + 1

        flashCount

    incrementGrid grid
    grid |> Array2D.iteri (flash grid)
    let flashCount = countFlashes grid

    printGrid grid
    printfn "%d" flashCount
    printfn ""

    flashCount

let computePasses numberOfPasses =
    let mutable flashCount = 0
    for i = 0 to numberOfPasses - 1 do
        flashCount <- compute grid + flashCount

    flashCount

printGrid grid
printfn ""
let flashCount = computePasses 5


printfn ""
printfn "%d" flashCount