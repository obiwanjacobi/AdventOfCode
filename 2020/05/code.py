import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")

#128 rows
#convert FFBFBFB (7) to binary = row number
# LRL (3) to binary = col number
# seat id = row * 8 + col
# highest seat id?

def decodeBinary(s, one):
    mask = 1 << len(s) -1
    val = 0
    for c in s:
        if c == one:
            val |= mask
        mask >>= 1
    return val

seats = []
highestSeat = 0

for line in file.readlines():
    line = line.rstrip("\n")

    r = line[:-3]
    c = line[-3:]

    row = decodeBinary(r, "B")
    col = decodeBinary(c, "R")

    seatNo = row * 8 + col
    #print (str(seatNo) + " = " + str(row) + " * 8 + " + str(col))

    if seatNo > highestSeat:
        highestSeat = seatNo

    seats.append(seatNo)

file.close()

seats.sort()

missingSeat = 0
lastSeat = 0
for seat in seats:
    if lastSeat == 0:
        lastSeat = seat
    elif lastSeat + 1 != seat:
        missingSeat = lastSeat + 1
    lastSeat = seat


print(highestSeat)
print(missingSeat)
