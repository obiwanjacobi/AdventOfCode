from pathlib import Path

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample.txt"

lines = []
with input_path.open("r") as f:
    lines = f.read().splitlines()

# remove 'blank' lines
manifold = list(filter(lambda l: not all(c == '.' for c in l), lines))

#print(manifold)

indexes = []
indexes.append([manifold[0].find("S")])
width = len(manifold[0])
total1 = 0
# skip first row with S
for l in range(1, len(manifold)):
    prevBeams = indexes[l - 1]
    newBeams = []
    for c in range(0, width):
        if manifold[l][c] == '^' and c in prevBeams:
            total1 += 1
            if c - 1 not in newBeams:
                newBeams.append(c - 1)
            newBeams.append(c + 1)
        elif c in prevBeams:
            newBeams.append(c)

    indexes.append(newBeams)

#print(indexes)

# 1236 too low (forgot to promote prevBeams that weren't plit to next layer)
print (f"Total1: {total1}")