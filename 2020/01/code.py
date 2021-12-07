
import os

rootDir = os.path.dirname(__file__)
values = []

file = open(rootDir + "\\input.txt", "r")
for line in file:
    val = int(line)
    values.append(val)
file.close()

for first in values:
    for second in values:
        if (first + second == 2020):
            print(str(first) + " * " + str(second) + " = " + str(first * second))

for first in values:
    for second in values:
        for third in values:
            if (first + second + third == 2020):
                print(str(first) + " * " + str(second) + " * " + str(third) + " = " + str(first * second * third))
