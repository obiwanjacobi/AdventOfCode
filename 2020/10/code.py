import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")
#file = open(rootDir + "\\example2.txt", "r")

adapters = []

for line in file.readlines():
    adapters.append(int(line))

file.close()

adapters.sort()
adapters.append(adapters[-1] + 3)
print(adapters)

oneCount = 0
threeCount = 0
jolt = 0
for a in adapters:
    diff = a - jolt
    if diff == 1:
        oneCount += 1
    elif diff == 3:
        threeCount += 1
    else:
        raise Exception("invalid data")
    jolt = a

#part 1
print("one: " + str(oneCount) + " three: " + str(threeCount))
print(oneCount * threeCount)

#part 2

#cache = 3 => 2 perm 2 ** (len - 2)
#cache = 4 => 4 perm
#cache = 5 => 7 perm (2 ** (len - 2) - 1

#   1, 2, 3
#   1, 3

#   1, 2, 3, 4
#   1, 2, 4
#   1, 3, 4
#   1, 4

#   1, 2, 3, 4, 5
#   1, 2, 3, 5
#   1, 2, 4, 5
#   1, 3, 4, 5
#   1, 2, 5
#   1, 3, 5
#   1, 4, 5
#   1, 5  => invalid so -1

# remove device
#adapters.remove(adapters[-1])

permutations = 1
seqCount = 0
jolt = 0
for a in adapters:
    diff = a - jolt
    if diff < 3:
        if seqCount == 0:
            seqCount += 1
        seqCount += 1
    else:
        if seqCount >= 3:
            factor = pow(2, seqCount - 2)
            if seqCount > 4:
                # cant span from begin to end
                factor -= 1
            permutations *= factor
        seqCount = 0
    jolt = a

print(permutations)
