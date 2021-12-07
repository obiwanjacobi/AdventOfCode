import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")

preamble = 25
#preamble = 5
xmasNumbers = []

for line in file:
    xmasNumbers.append(int(line))

file.close()

def isValid(xmasIndex):
    i = xmasIndex - preamble
    while i < xmasIndex:
        j = xmasIndex - preamble
        while j < xmasIndex:
            if i != j and xmasNumbers[i] + xmasNumbers[j] == xmasNumbers[xmasIndex]:
                return True
            j += 1
        i += 1
    return False

# part 1
xmasIndex = preamble
while xmasIndex < len(xmasNumbers):
    if not isValid(xmasIndex):
        break
    xmasIndex += 1

print(xmasNumbers[xmasIndex])
print (xmasIndex)

# part 2

def contiguousRange(xmasIndex):
    i = 0
    while i < xmasIndex:
        acc = xmasNumbers[i]
        j = i + 1
        while j < xmasIndex:
            acc += xmasNumbers[j]
            if acc == xmasNumbers[xmasIndex]:
                rng = xmasNumbers[i:j + 1]
                rng.sort()
                return rng[0] + rng[-1]
            if acc > xmasNumbers[xmasIndex]:
                break;
            j += 1
        i += 1
    return -1

print(contiguousRange(xmasIndex))