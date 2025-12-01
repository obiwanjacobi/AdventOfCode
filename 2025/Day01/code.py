from pathlib import Path

root = Path(__file__).parent
input_path = root / "input.txt"
#input_path = root / "sample.txt"

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

total2 = 50
count2 = 0

index = 0
for l in lines:
    prevTot = total2
    prevCnt = count2

    val = int(l[1:])
    if l[0] == 'L':
        total2 -= val
        count2 += int(val / 100)
        if prevTot > 0 and total2 <= 0:
            count2 += 1
    else:
        total2 += val
        count2 += int(val / 100)
        if total2 >= 100:
            count2 += 1

    total2 = total2 % 100

    if (prevCnt != count2):
        print(f"{l}({index}):[{total2}] bounds +{count2 - prevCnt} | {count2}")
    else:
        print(f"{l}({index}):[{total2}] bounds")

    # if total2 <= 0 or total2 >= 100:
    #     prevCnt = count2

    #     count2 += int(val / 100)
    #     if prevTot != 0:
    #         count2 += 1
    #     total2 = total2 % 100

    #     if (prevCnt != count2):
    #         print(f"{l}({index}):[{total2}] bounds +{count2 - prevCnt} | {count2}")
    #     else:
    #         print(f"{l}({index}):[{total2}] bounds")
    # else:
    #     print(f"{l}({index}):[{total2}] in-bounds")
    
    index += 1

#R50 => 0 (1)
#R50 => 50
#L50 => 0 (2)
#L50 => 50
#R75 => 25 (3)
#L50 => 75 (4)
#L25 => 50
#L75 => 75 (5)
#R50 => 25 (6)

# to 0: increment
# from 0: don't increment
# ..with > 100: add / 100
# exceed lower bound: adjust
# exceed upper bound: adjust

# 5981 too low
# 6410 too high
print("Count2:", count2)
