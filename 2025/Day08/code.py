from pathlib import Path
from typing import Optional
import math

root = Path(__file__).parent
#input_path = root / "input.txt"
input_path = root / "sample.txt"

lines = []
with input_path.open("r") as f:
    lines = f.read().splitlines()

class JunctionBox:
    def __init__ (self, x, y, z):
        self.x = x
        self.y = y
        self.z = z
        self.parent : Optional[JunctionBox] = None
        self.children = []
    
    def __repr__(self) -> str:
        return f"{{{self.x}, {self.y}, {self.z}}}"
    
    def distanceTo(self, jb):
        # https://en.wikipedia.org/wiki/Euclidean_distance
        dx = self.x - jb.x
        dy = self.y - jb.y
        dz = self.z - jb.z

        return math.sqrt(math.pow(dx, 2) + math.pow(dy, 2) + math.pow(dz, 2))
    
    def root(self):
        if self.parent is None:
            return self
        return self.parent.root()
    
    def connect(self, jb):
        if jb.parent is not None and self.parent is not None:
            print(f"ILLEGAL CONNECTION: {self}=>{jb}")
            return
        
        if jb.parent is not None:
            self.parent = jb
            jb.children.append(self)
        else:
            jb.parent = self
            self.children.append(jb)
    
    def isConnectedTo(self, jb):
        root = self.root()
        return root.isConnected(jb)

    def isConnected(self, jb):
        for c in self.children:
            if c is jb:
                return True
            if c.isConnected(jb):
                return True

        return False
    
boxes = []
for l in lines:
    coords = l.split(",")
    boxes.append(JunctionBox(int(coords[0]), int(coords[1]), int(coords[2])))

def ConnectShortest():
    shortest = 999999999
    shortestJB1 = None
    shortestJB2 = None

    for begin in range(0, len(boxes)):
        for end in range(begin + 1, len(boxes)):
            box1 = boxes[begin]
            box2 = boxes[end]
            if box1 is box2:
                continue
            if box1.isConnectedTo(box2):
                continue

            d = box1.distanceTo(box2)
            #print(f"{box1}=>{box2}={d}")
            if d < shortest:
                shortest = d
                shortestJB1 = box1
                shortestJB2 = box2
            
    if shortestJB1 is not None and shortestJB2 is not None:
        c1 = shortestJB1.isConnectedTo(shortestJB2)
        c2 = shortestJB2.isConnectedTo(shortestJB1)
        shortestJB1.connect(shortestJB2)
        print(f"{shortestJB1}=>{shortestJB2}={shortest}")
        return True
    
    return False

while ConnectShortest():
    pass
