// https://adventofcode.com/2023/day/2
// part1

//let lines = System.IO.File.ReadLines "sample1.txt"
//let lines = System.IO.File.ReadLines "sample2.txt"
let lines = System.IO.File.ReadLines "input.txt"
//lines |> Seq.iter(fun x -> printfn "%s" x)
//let numberOfLines = lines |> Seq.length
//printfn "Number of input lines: %d" numberOfLines

type GameScore =
  { Red: int; Green: int; Blue: int; }
type Game = 
  { Id: int; Scores: GameScore[]; }

let getColorScore (line: string) =
  let parts = line.Trim().Split(' ')
  parts[1].Trim(), System.Int32.Parse(parts[0])

let getGameScore (line: string) = 
  let parts = line.Trim().Split(',')
  let colorScores = parts |> Array.map (fun cs -> getColorScore cs)
  
  let reds = colorScores |> Array.filter (fun (name, score) -> name = "red")
  let greens = colorScores |> Array.filter (fun (name, score) -> name = "green")
  let blues = colorScores |> Array.filter (fun (name, score) -> name = "blue")
  
  let red = 
    match reds with
    | [| (name, score) |] -> score
    | _ -> 0
  let green = 
      match greens with
      | [| (name, score) |] -> score
      | _ -> 0
  let blue = 
      match blues with
      | [| (name, score) |] -> score
      | _ -> 0

  { Red = red; Green = green; Blue = blue; }

let getGameScores (line: string) =
  let parts = line.Trim().Split(';')
  parts |> Array.map (fun p -> getGameScore p)

let getGame (line: string) =
  let parts = line.Trim().Split(':')
  { Id = System.Int32.Parse(parts.[0][5..]); Scores = getGameScores parts[1]; }

// ----------------------------------------------------------
// part1

let games = 
  lines |> Seq.map (fun l -> getGame l)

let compareScore gameScore score =
  gameScore.Red <= score.Red && gameScore.Green <= score.Green && gameScore.Blue <= score.Blue

let compareGame game score =
  let compatibleScores = game.Scores |> Array.filter (fun s -> compareScore s score)
  game.Scores.Length = compatibleScores.Length

let selectGameIds games score =
  games
  |> Seq.filter (fun g -> compareGame g score)
  |> Seq.map (fun g -> g.Id)
  |> Seq.sum


let score = { Red = 12; Green = 13; Blue = 14; }

let sum = selectGameIds games score

printf "Sum of all compatible game-id's: %d" sum

