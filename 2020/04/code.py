import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")

passports = []
currentPass = dict()

newline = 0
field = ""
buf = ""
while True:
    c = file.read(1)
    if not c:
        if len(buf) > 0:
            currentPass[field] = buf
        break
    if c == "\n":
        newline += 1
    if newline == 1 and c != "\n":
        newline = 0
    if newline > 1:
        newline = 0
        if len(currentPass) > 0:
            passports.append(currentPass)
            currentPass = dict()
        continue

    if c == " " or c == "\n":
        if len(buf) > 0:
            currentPass[field] = buf
            field = ""
            buf = ""
        continue

    if c == ":":
        field = buf
        buf = ""
    else:
        buf += c

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

def isValid(pp):
    for key in fields:
        if not key in pp.keys():
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