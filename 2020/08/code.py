import os

rootDir = os.path.dirname(__file__)
file = open(rootDir + "\\input.txt", "r")
#file = open(rootDir + "\\example.txt", "r")

class Instruction:
    Opcode: str
    Operand: int

program = []    # Instruction[]

for line in file.readlines():
    parts = line.split(" ")
    instr = Instruction()
    instr.Opcode = parts[0]
    instr.Operand = int(parts[1])
    program.append(instr)
file.close()

# for instruction in program:
#     print(instruction.Opcode + " " + str(instruction.Operand))

visited = []

def run(prog):
    acc = 0
    ip = 0

    visited.clear()
    while ip >= 0 and ip < len(program):
        if ip in visited:
            break;

        instr = prog[ip]
        visited.append(ip)

        if instr.Opcode == "acc":
            acc += instr.Operand
        if instr.Opcode == "jmp":
            ip += instr.Operand
        else:
            ip += 1

    return (acc, ip)

# part 1
acc, ip = run(program)
print (str(acc) + ": " + str(ip))

# part 2

# back track through visited
# brute force rerun with changed opcode until ip >= len(program)
executionPath = visited.copy()

def sampleRuns():
    for sampleIp in executionPath:
        sampleProg = program.copy()
        inst = sampleProg[sampleIp]
        if inst.Opcode == "nop" and inst.Operand != 0:
            newInst = Instruction()
            newInst.Opcode = "jmp"
            newInst.Operand = inst.Operand
            sampleProg[sampleIp] = newInst
        elif inst.Opcode == "jmp":
            newInst = Instruction()
            newInst.Opcode = "nop"
            newInst.Operand = inst.Operand
            sampleProg[sampleIp] = newInst

        acc, ip = run(sampleProg)
        print (str(sampleIp) + " => " + str(acc) + ": " + str(ip))

        if ip >= len(sampleProg):
            return (acc, ip)
    
    return( -1, -1)


acc, ip = sampleRuns()
print (str(acc) + ": " + str(ip))
