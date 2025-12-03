from pathlib import Path

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample.txt"

banks = []
with input_path.open("r") as f:
    banks = f.read().splitlines()

joltages = []

for b in banks:
    options = []
    for i in range(len(b)):
        highest = 0
        for j in range(i + 1, len(b)):
            v = int(f"{b[i]}{b[j]}")
            if v > highest:
                highest = v
        
        options.append(highest)
    joltages.append(max(options))

print(f"Sum1: {sum(joltages)}")

joltages.clear()

for b in banks:
    l = len(b)
    additionals = l - 12
    digits = []
    i = 0
    while i < l:
        h = 0
        ih =  -1
        for p in range(i, i + additionals + 1):
            v = int(b[p])
            if v > h:
                h = v
                ih = p
        digits.append(h)
        additionals -= ih - i
        i = ih + 1

        if len(digits) == 12:
            break
    
    #print(digits)
    j = int("".join(map(str, digits)))
    joltages.append(j)

print(f"Sum2: {sum(joltages)}")