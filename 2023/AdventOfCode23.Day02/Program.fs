// https://adventofcode.com/2023/day/2

//let lines = System.IO.File.ReadLines "sample.txt"
let lines = System.IO.File.ReadLines "input.txt"

// parsing input

type GameScore =
  { Red: int; Green: int; Blue: int; }
type Game = 
  { Id: int; Scores: GameScore[]; }

let getColorScore (line: string) =
  let parts = line.Trim().Split(' ')
  parts[1].Trim(), parts[0] |> int

let filterColorScore colorName scores =
  let colorScores = scores |> Array.filter (fun (name, score) -> name = colorName)
  match colorScores with
  | [| (name, score) |] -> score
  | _ -> 0

let getGameScore (line: string) = 
  let colorScores = 
    line.Trim().Split(',')
    |> Array.map (fun cs -> getColorScore cs)  
  
  let red = colorScores |> filterColorScore "red"
  let green = colorScores |> filterColorScore "green"
  let blue = colorScores |> filterColorScore "blue"

  { Red = red; Green = green; Blue = blue; }

let getGameScores (line: string) =
  let parts = line.Trim().Split(';')
  parts |> Array.map (fun p -> getGameScore p)

let getGame (line: string) =
  let parts = line.Trim().Split(':')
  { Id = parts.[0][5..] |> int; Scores = getGameScores parts[1]; }

// --------------------------------------------------------
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

let idSum = selectGameIds games score

printf "Sum of all compatible game-id's: %d\r\n" idSum

// --------------------------------------------------------
// part 2

let determineMinimalScore gameScore minScore =
  { Red = max gameScore.Red minScore.Red; Green = max gameScore.Green minScore.Green; Blue = max gameScore.Blue minScore.Blue; }

let getMinimalScore game =
  game.Scores |> Seq.reduce (fun a s -> determineMinimalScore s a)

let scorePower score =
  score.Red * score.Green * score.Blue

let powerSum =
  games
  |> Seq.map (fun g -> g |> getMinimalScore |> scorePower)
  |> Seq.sum

printf "Sum of all minimal score powers: %d\r\n" powerSum