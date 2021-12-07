import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")

class Bag:
    def __init__(self):
        self.Name = ""
        self.Containing = {}    # Bag -> num
        self.ContainedBy = {}   # name -> Bag

# name -> Bag
bags = {}

def GetOrAddBag(bagName: str):
    bag = Bag()
    if bagName in bags.keys():
        bag = bags[bagName]
    else:
        bag.Name = bagName
        bags[bag.Name] = bag
    return bag

def ParseAddContaining(bag: Bag, numBag: str):
    numBag = numBag.strip(" ")
    if numBag != "no other":
        num = int(numBag[0:1])
        name = numBag[2:]
        contBag = GetOrAddBag(name)
        bag.Containing[contBag] = num
        contBag.ContainedBy[bag.Name] = bag

for line in file.readlines():
    #light red bags contain 1 bright white bag, 2 muted yellow bags.\n
    line = line.rstrip("\n")
    #light red bags contain 1 bright white bag, 2 muted yellow bags.
    line = line.rstrip(".")
    #light red bags contain 1 bright white bag, 2 muted yellow bags
    line = line.replace("bags", "")
    #light red contain 1 bright white bag, 2 muted yellow
    line = line.replace("bag", "")
    #light red contain 1 bright white , 2 muted yellow

    parts = line.split(",")
    #[light red contain 1 bright white , 2 muted yellow]
    src, cont = parts[0].split("contain")
    #[light red , 1 bright white]

    bagName = src.strip(" ")
    bag = GetOrAddBag(bagName)

    ParseAddContaining(bag, cont)
    for p in parts[1:]:
        ParseAddContaining(bag, p)

file.close()

def GetContainedBy(bag: Bag):
    contBy = bag.ContainedBy.copy()

    for contBag in bag.ContainedBy.values():
        contBy.update(GetContainedBy(contBag))
    return contBy

def GetContaining(bag: Bag):
    total = 0
    for contBag in bag.Containing.keys():
        sub = GetContaining(contBag)
        if sub > 0:
            total += bag.Containing[contBag] * sub
        total += bag.Containing[contBag]
    return total

bag = bags["shiny gold"]

#part 1
containedByBags = GetContainedBy(bag)
print(len(containedByBags))

# part 2
containingBags = GetContaining(bag)
print(containingBags)