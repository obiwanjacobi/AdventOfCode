import os

rootDir = os.path.dirname(__file__)
values = []

validCount1: int = 0
validCount2: int = 0

file = open(rootDir + "\\input.txt", "r")
for line in file:
    # parse line content
    validator, password = line.split(":")
    rng, char = validator.split(" ")
    min, max = rng.split("-")
    
    # part 1
    charCount: int = 0
    for c in password:
        if (c == char):
            charCount = charCount + 1
    
    if (int(min) <= charCount and charCount <= int(max)):
        validCount1 = validCount1 + 1
    
    # part 2
    firstIndex = int(min)   # password starts with a space so no need to compensate to zero-based
    secondIndex = int(max)

    charCount = 0
    if (password[firstIndex] == char):
        charCount = charCount + 1
    if (password[secondIndex] == char):
        charCount = charCount + 1
    if (charCount == 1):
        validCount2 = validCount2 + 1

file.close()

print(validCount1)
print(validCount2)