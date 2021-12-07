import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")

commonAnsers = []
answer = ""
inGroup = False
for line in file.readlines():
    line = line.rstrip("\n")

    if len(line) == 0:
        commonAnsers.append(answer)
        answer = ""
        inGroup = False
        continue
    
    if len(answer) == 0 and inGroup == False:
        answer = line
        inGroup = True
    else:
        for c in line:
            if c not in answer:
                answer = answer.replace(c, "")
        for c in answer:
            if c not in line:
                answer = answer.replace(c, "")

if len(answer) > 0:
    commonAnsers.append(answer)

file.close()

print(commonAnsers)

total = 0
for code in commonAnsers:
    total += len(code)

print(total)