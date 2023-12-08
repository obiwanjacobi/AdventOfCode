// https://adventofcode.com/2023/day/0

//let lines = System.IO.File.ReadLines "sample.txt"
//let lines = System.IO.File.ReadLines "sample2.txt"
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

type Node =
  { Id: string; Left: string; Right: string; DeadEnd: bool; }

let instructions = Seq.head lines

let parseNode (line: string) =
  let parts = line.Trim().Split(" = ")
  let nodes = parts.[1].Split(", ")
  let left = nodes.[0].Substring(1)
  let right = nodes[1].Substring(0, nodes[1].Length - 1)
  let deadEnd = parts.[0] = left && left = right
  
  { Id = parts.[0]; Left = left; Right = right; DeadEnd = deadEnd; }

let nodes =
  lines
  |> List.ofSeq
  |> List.skip 2
  |> List.map (fun l -> parseNode l)

printf "%A\r\n" instructions
printf "%A\r\n" nodes

// --------------------------------------------------------
// part1

let findNode nodeId =
  let result = nodes |> List.filter (fun n -> n.Id = nodeId)
  match result with
  | [node] -> node
  | _ -> failwith ("Could not find node " + nodeId)

let rec navigate nodeId index count =
  if nodeId = "ZZZ" then
    count
  else
    let dir = instructions.[index % instructions.Length]
    let node = findNode nodeId
    let nextNode = if dir = 'L' then node.Left else node.Right
    navigate nextNode (index + 1) (count + 1)

let count = navigate "AAA" 0 0

printf "Number of steps: %A\r\n" count

// --------------------------------------------------------
// part2
