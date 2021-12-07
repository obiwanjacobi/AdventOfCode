import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")

groupAnsers = []
answer = ""
for line in file.readlines():
    line = line.rstrip("\n")

    if len(line) == 0:
        groupAnsers.append(answer)
        answer = ""
        continue
    
    for c in line:
        if c not in answer:
            answer += c

if len(answer) > 0:
    groupAnsers.append(answer)

file.close()

total = 0
for code in groupAnsers:
    total += len(code)

print(total)