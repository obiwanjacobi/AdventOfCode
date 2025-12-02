from pathlib import Path

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample2.txt"

lines = []
with input_path.open("r") as f:
    for line in f:
        lines.append(line[:-1])

total1 = 50
count1 = 0

for l in lines:
    val = int(l[1:])
    if l[0] == 'L':
        total1 -= val
    else:
        total1 += val
    if total1 % 100 == 0:
        count1 += 1

print("Count1:", count1)

dail2 = 50
count2 = 0

print(f"_   : {dail2} => 0")

for l in lines:
    prevDail = dail2
    prevCnt = count2

    val = int(l[1:])
    if l[0] == 'L':
        dail2 -= val % 100
    else:
        dail2 += val % 100

    count2 += int(val / 100)

    if dail2 <= 0 or dail2 >= 100:
        if prevDail != 0:
            count2 += 1
        dail2 = dail2 % 100
    
    print(f"{l}: {dail2} => {count2 - prevCnt} | {count2}")

# to 0: increment
# from 0: don't increment
# ..with > 100: add / 100
# exceed lower bound: adjust
# exceed upper bound: adjust

print("Count2:", count2)
