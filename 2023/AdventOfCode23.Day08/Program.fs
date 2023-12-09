// https://adventofcode.com/2023/day/8

//let lines = System.IO.File.ReadLines "sample.txt"
//let lines = System.IO.File.ReadLines "sample2.txt"
//let lines = System.IO.File.ReadLines "sample3.txt"  // part 2 only
let lines = System.IO.File.ReadLines "input.txt"

// input parsing

type Node =
  { Id: string; Left: string; Right: string; Start: bool; Stop: bool }

let instructions = Seq.head lines

let parseNode (line: string) =
  let parts = line.Trim().Split(" = ")
  let nodes = parts.[1].Split(", ")
  let left = nodes.[0].Substring(1)
  let right = nodes[1].Substring(0, nodes[1].Length - 1)
  let start = parts.[0].EndsWith('A')
  let stop = parts.[0].EndsWith('Z')
  
  { Id = parts.[0]; Left = left; Right = right; Start = start; Stop = stop; }

let nodes =
  lines
  |> List.ofSeq
  |> List.skip 2
  |> List.map (fun l -> parseNode l)

//printf "%A\r\n" instructions
//printf "%A\r\n" nodes

// --------------------------------------------------------
// part1

let findNode nodeId =
  let result = nodes |> List.filter (fun n -> n.Id = nodeId)
  match result with
  | [node] -> node
  | _ -> failwith ("Could not find node " + nodeId)

let rec navigate1 nodeId index count =
  if nodeId = "ZZZ" then
    count
  else
    let dir = instructions.[index % instructions.Length]
    let node = findNode nodeId
    let nextNode = if dir = 'L' then node.Left else node.Right
    navigate1 nextNode (index + 1) (count + 1)

//let count1 = navigate1 "AAA" 0 0

//printf "Number of steps 1: %A\r\n" count1

// --------------------------------------------------------
// part2

let startNodes = nodes |> List.filter (fun n -> n.Start)
let stopNodes = nodes |> List.filter (fun n -> n.Stop)

printf "Start nodes: %A\r\n" (startNodes |> List.length)
printf "Stop nodes: %A\r\n" (stopNodes |> List.length)

let rec navigateAll nodes index =
  let finishedCount = nodes |> List.filter (fun n -> n.Stop) |> List.length
  if finishedCount = List.length nodes then
    index
  else
    let dir = instructions.[index % instructions.Length]
    let nextNodeIds = nodes |> List.map (fun n -> if dir = 'L' then n.Left else n.Right)
    let nextNodes = nextNodeIds |> List.map (fun id -> findNode id)

    printf "%A: %A %A\r\n" index dir nextNodeIds
    navigateAll nextNodes (index + 1)

let count2 = navigateAll startNodes 0

printf "Number of steps 2: %A\r\n" count2