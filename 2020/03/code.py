import os

rootDir = os.path.dirname(__file__)
treeMap = []

file = open(rootDir + "\\input.txt", "r")
for line in file:
    treeMap.append(line)
file.close()

length = len(treeMap[0]) - 1
count = len(treeMap)

def determineTreeCount(deltaX: int, deltaY: int):
    x = 0
    y = 0
    treeCount = 0

    while y < count:
        if x >= length:
            x = x % length

        if treeMap[y][x] == "#":
            treeCount = treeCount + 1
        #     print ("# at " + str(x) + ", " + str(y))
        # else:
        #     print (". at " + str(x) + ", " + str(y))

        x = x + deltaX
        y = y + deltaY
    
    return treeCount

r3d1 = determineTreeCount(3, 1)
print ("Part1: " + str(r3d1))

r1d1 = determineTreeCount(1, 1)
r5d1 = determineTreeCount(5, 1)
r7d1 = determineTreeCount(7, 1)
r1d2 = determineTreeCount(1, 2)

print ("Part2: " + str(r1d1 * r3d1 * r5d1 * r7d1 * r1d2))
