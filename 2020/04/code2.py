import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")

passports = []
currentPass = {}

for line in file.readlines():
    line = line.rstrip("\n")

    if len(line) == 0:
        passports.append(currentPass)
        currentPass = {}
        continue
    
    for fieldVal in line.split(" "):
        parts = fieldVal.split(":")
        currentPass[parts[0]] = parts[1]

if len(currentPass) > 0:
    passports.append(currentPass)

file.close()

# valid passports have all these fields
fields = [
    "byr",
    "iyr",
    "eyr",
    "hgt",
    "hcl",
    "ecl",
    "pid"]

def isNumber(s, min, max):
    v = int(s)
    return min <= v and v <= max

def isHeight(s):
    post = s[-2:]
    if post != "cm" and post != "in":
        return False

    val = s[0: len(s) - 2]
    if post == "cm":
        return isNumber(val, 150, 193)

    return isNumber(val, 59, 76)
    
hexChars = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f"]

def isHairColor(s):
    if s[0] != "#":
        return False
    val = s[1: len(s)]
    if len(val) != 6:
        return False

    for c in val:
        if c not in hexChars:
            return False
    return True

eyeColors = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"]
def isEyeColor(s):
    if not s in eyeColors:
        return False
    return True

def isId(s):
    if len(s) != 9:
        return False
    return int(s) > 0

def isValid(pp):
    if len(pp.keys()) < 7:
        return False

    for key in fields:
        if not key in pp.keys():
            return False

    #part 2
    val = pp["byr"]
    if not isNumber(val, 1920, 2002):
        return False
    val = pp["iyr"]
    if not isNumber(val, 2010, 2020):
        return False
    val = pp["eyr"]
    if not isNumber(val, 2020, 2030):
        return False
    val = pp["hgt"]
    if not isHeight(val):
        return False
    val = pp["hcl"]
    if not isHairColor(val):
        return False
    val = pp["ecl"]
    if not isEyeColor(val):
        return False
    val = pp["pid"]
    if not isId(val):
        return False
    return True

validCount = 0
for passport in passports:
    if isValid(passport):
        validCount += 1
    #     print ("+valid (" + str(len(passport)) + ") " + str(passport))
    # else:
    #     print ("-invalid (" + str(len(passport)) + ") " + str(passport))

print ("Valid: " + str(validCount) + " of " + str(len(passports)))

#print (passports)