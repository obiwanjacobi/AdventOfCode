from pathlib import Path

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample.txt"

ranges = []
with input_path.open("r") as f:
    ranges = list(map(lambda r: r.split("-"), f.read().split(",")))

invalidIds = []

for r in ranges:
    lStart = len(r[0])
    lEnd = len(r[1])
    
    if lStart == lEnd and lStart % 2 != 0:
        #print ()
        continue

    start = int(r[0])
    end = int(r[1])

    for x in range(start, end + 1):
        strX = str(x)
        l = len(strX)
        if l % 2 != 0:
            continue
        lHalf = int(l / 2)
        if strX[:lHalf] == strX[lHalf:]:
            invalidIds.append(x)

#print(invalidIds)
print(f"Sum1: {sum(invalidIds)}")

# 5398419734 too low
# 5398419778 correct

invalidIds.clear()

for r in ranges:
    start = int(r[0])
    end = int(r[1])

    for x in range(start, end + 1):
        strX = str(x)
        length = len(strX)
        lEnd = int((length + 2) / 2)

        for l in range(1, lEnd):
            parts = [strX[i:i+l] for i in range(0, length, l)]
            #print(f"{strX}: {parts}")
            if all(p == parts[0] for p in parts):
                invalidIds.append(x)
                break

#print(invalidIds)
print(f"Sum2: {sum(invalidIds)}")
