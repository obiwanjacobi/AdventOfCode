from pathlib import Path

root = Path(__file__).parent
#input_path = root / "input.txt"
input_path = root / "sample.txt"

lines = []
with input_path.open("r") as f:
    lines = f.read().splitlines()

# remove 'blank' lines
manifold = list(filter(lambda l: not all(c == '.' for c in l), lines))

#print(manifold)

indexes1 = []
indexes1.append([manifold[0].find("S")])
width = len(manifold[0])
total1 = 0
# skip first row with S
for l in range(1, len(manifold)):
    prevBeams = indexes1[l - 1]
    newBeams = []
    children2 = []
    for c in prevBeams:
        if manifold[l][c] == '^':
            total1 += 1
            if c - 1 not in newBeams:
                newBeams.append(c - 1)
            newBeams.append(c + 1)
        elif c not in newBeams:
            newBeams.append(c)

    indexes1.append(newBeams)

print(indexes1)

# 1236 too low (forgot to promote prevBeams that weren't split to next layer)
print (f"Total1: {total1}")


total2 = 0

# 3096 too low (just adding the length of indexes1 per level - except start)
print(f"Total2: {total2}")