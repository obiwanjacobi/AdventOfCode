// https://adventofcode.com/2021/day/1

// part1

let lines = System.IO.File.ReadLines "input.txt"
//lines |> Seq.iter(fun x -> printfn "%s" x)
//let numberOfLines = lines |> Seq.length
//printfn "Number of input lines: %d" numberOfLines

let depths = lines |> Seq.map System.Int32.Parse

let determine src =
    let incIfBigger (cnt, last) d =
        if d > last then
            (cnt + 1, d)
        else
            (cnt, d)

    src |> Seq.fold incIfBigger (-1, 0)

let (cnt1, _) = depths |> determine
printfn "Part1: %d" cnt1


// part 2

let grouped = depths |> Seq.windowed 3 |> Seq.map Seq.sum

let (cnt2, _) = grouped |> determine
printfn "Part2: %d" cnt2
