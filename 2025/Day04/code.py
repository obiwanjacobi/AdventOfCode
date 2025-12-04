from pathlib import Path

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample.txt"

lines = []
with input_path.open("r") as f:
    lines = f.read().splitlines()

grid = list(map(lambda l: list(l), lines))
maxRows = len(grid)
maxCols = len(lines[0])

def isOccupied(r, c):
    if r >= 0 and r < maxRows and c >=0  and c < maxCols:
        return grid[r][c] != "."
    return False

def CountAdjacent(r, c):
    cnt = 0
    if isOccupied(r-1, c-1):
        cnt += 1
    if isOccupied(r-1, c):
        cnt += 1
    if isOccupied(r-1, c+1):
        cnt += 1
    if isOccupied(r, c-1):
        cnt += 1
    # if isOccupied(r, c):
    #     cnt += 1
    if isOccupied(r, c+1):
        cnt += 1
    if isOccupied(r+1, c-1):
        cnt += 1
    if isOccupied(r+1, c):
        cnt += 1
    if isOccupied(r+1, c+1):
        cnt += 1
    return cnt

def MarkRolls(m):
    total = 0
    for r in range(0, maxRows):
        for c in range(0, maxCols):
            if isOccupied(r, c):
                cnt = CountAdjacent(r, c)
                if cnt < 4:
                    grid[r][c] = m
                    total += 1
    return total

print(f"Total1: {MarkRolls("@")}")

def RemoveRolls():
    return MarkRolls(".")

total2 = 0
removeCount = RemoveRolls()
while removeCount > 0:
    total2 += removeCount
    removeCount = RemoveRolls()

print(f"Total2: {total2}")