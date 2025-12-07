from pathlib import Path
import math

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample.txt"

lines = []
with input_path.open("r") as f:
    lines = f.read().splitlines()

grid1 = []
for l in range(0, len(lines) - 1):
    grid1.append(list(map(lambda v: int(v), lines[l].split())))
operators = list(lines[-1].split())

# print(grid)
# print(operators)

l = len(grid1[0])
total1 = 0
for i in range(0, l):
    if operators[i] == '*':
        acc = 1
        for cell in grid1:
            acc *= cell[i]
    else:
        acc = 0
        for cell in grid1:
            acc += cell[i]
    total1 += acc

print(f"Total1: {total1}")

grid2 = []
col = []
startMarker = 0 # use operator as start marker of col
for o in range(0, len(lines[-1])):
    if lines[-1][o] != ' ':     # operator
        startMarker = o
    
    str = ''
    for l in range(0, len(lines) - 1):
        str += lines[l][o]
    
    # column separators are spaces
    if not str.isspace():
        v = int(str)
        col.append(v)
        # print(v)
    else:
        grid2.append(list(col))
        col.clear()

# add last one (input is not terminated with col-del spaces)
grid2.append(list(col))
# print(grid2)

l = len(grid2)
total2 = 0
for c in range(0, l):
    if operators[c] == '*':
        total2 += math.prod(grid2[c])
    else:
        total2 += sum(grid2[c])

print(f"Total2: {total2}")