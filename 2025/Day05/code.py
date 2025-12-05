from pathlib import Path

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample.txt"

lines = []
with input_path.open("r") as f:
    lines = f.read().splitlines()

ranges = []
ids = []
mode = 0
for l in lines:
    if len(l) == 0:
        mode = 1
        continue
    
    if mode == 1:
        ids.append(int(l))
    else:
        parts = l.split("-")
        ranges.append(list(map(lambda p: int(p), parts)))

def isInRange(id):
    for r in ranges:
        if id >= r[0] and id <= r[1]:
            return True
    return False

# print(ranges)
# print(ids)

fresh = 0
for id in ids:
    if isInRange(id):
        fresh += 1

# 857 too high (forgot to convert to int)
print(f"Total1: {fresh}")

def intersects(r1, r2):
    return max(r1[0], r2[0]) <= min(r1[1], r2[1])

def merge(r1, r2):
    return [min(r1[0], r2[0]), max(r1[1], r2[1])]

def mergeRangesPass(rngs):
    l = len(rngs)
    merged = []
    op = 0
    r = 0
    while r < l - 1:
        rng = rngs[r]
        nxt = rngs[r+1]
        if intersects(rng, nxt):
            merged.append(merge(rng, nxt))
            op = 1
            r += 2
        else:
            merged.append(rng)
            op = 0
            r += 1

    if op == 0:
        last = rngs[l - 1]
        merged.append(last)

    return merged

mergedRanges = list(ranges)
mergedRanges.sort()

#print(mergedRanges)

while True:
    tmp = mergeRangesPass(mergedRanges)
    if len(mergedRanges) == len(tmp):
        break
    mergedRanges = tmp

#print(mergedRanges)

total2 = 0

for r in mergedRanges:
    v = r[1] - r[0] + 1 # inclusive range
    total2 += v
    # print(v)

# 351860214145387 too low
# 442936863489815 too high
print(f"Total2: {total2}")