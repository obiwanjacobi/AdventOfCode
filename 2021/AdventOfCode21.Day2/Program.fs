// https://adventofcode.com/2021/day/2

// part1

let lines = System.IO.File.ReadLines "input.txt"
//lines |> Seq.iter(fun x -> printfn "%s" x)
//let numberOfLines = lines |> Seq.length
//printfn "Number of input lines: %d" numberOfLines

type Action =
    {
        name: string;
        value: int64;
    }

let parseAction (txt: string) =
    let parts = txt.Split ' '
    { name = parts[0]; value = System.Int64.Parse parts[1] }

let actions = lines |> Seq.map parseAction

let move (pos, depth) (action: Action) =
    match action.name with
    | "forward" -> (pos + action.value, depth)
    | "up" -> (pos, depth - action.value)
    | "down" -> (pos, depth + action.value)
    | _ -> (pos, depth)

let (pos, depth) = actions |> Seq.fold move (0, 0)

printfn "Part1: Position: %d Depth: %d Product: %d" pos depth (pos * depth)

// part 2

let moveWithAim (pos, depth, aim) (action: Action) =
    match action.name with
    | "forward" -> (pos + action.value, depth + (aim * action.value), aim)
    | "up" -> (pos, depth, aim - action.value)
    | "down" -> (pos, depth, aim + action.value)
    | _ -> (pos, depth, aim)

let (pos2, depth2, _) = actions |> Seq.fold moveWithAim (0, 0, 0)

printfn "Part1: Position: %d Depth: %d Product: %d" pos2 depth2 (pos2 * depth2)
