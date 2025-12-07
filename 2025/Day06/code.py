from pathlib import Path

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample.txt"

lines = []
with input_path.open("r") as f:
    lines = f.read().splitlines()


grid = []
for l in range(0, len(lines) - 1):
    grid.append(list(map(lambda v: int(v), lines[l].split())))
operators = list(lines[-1].split())

# print(grid)
# print(operators)

l = len(grid[0])
total1 = 0
for i in range(0, l):
    if operators[i] == '*':
        acc = 1
        for cell in grid:
            acc *= cell[i]
    else:
        acc = 0
        for cell in grid:
            acc += cell[i]
    total1 += acc

print(f"Total1: {total1}")