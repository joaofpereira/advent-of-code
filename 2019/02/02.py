import sys

def read_input(fileName):
	lines = []
	with open(fileName, 'r') as input:
		lines = input.readline().split(",")
	return [int(i) for i in lines]

def opcode_1(value_1, value_2):
	return value_1 + value_2

def opcode_2(value_1, value_2):
	return value_1 * value_2

def transform_input(noun, verb, input):
	input[1] = noun
	input[2] = verb
	return [int(i) for i in input]

def exercise_1():
	input = read_input('./input.txt')
	for i in range(0, len(input), 4):
		if input[i] == 1:
			input[input[i+3]] = opcode_1(input[input[i+1]], input[input[i+2]])
		elif input[i] == 2:
			input[input[i+3]] = opcode_2(input[input[i+1]], input[input[i+2]])
		elif input[i] == 99:
			break
	print("Exercise 1: " + str(input[0]))

def exercise_2(target):
	input = read_input('./input.txt')
	
	for i in range(0, 99):
		for z in range(0, 99):
			copied = transform_input(i, z, input.copy())

			for i_idx in range(0, len(copied), 4):
				if copied[i_idx] == 1:
					copied[copied[i_idx + 3]] = opcode_1(copied[copied[i_idx + 1]], copied[copied[i_idx + 2]])
				elif copied[i_idx] == 2:
					copied[copied[i_idx + 3]] = opcode_2(copied[copied[i_idx + 1]], copied[copied[i_idx + 2]])
				elif copied[i_idx] == 99:
					if copied[0] == target:
						#print("Noun: " + str(copied[1]) + " Verb:" + str(copied[2]))
						print("Exercise 2: " + str(100 * copied[1] + copied[2]))
						exit(1)
					else:
						continue
			z+=1
		i+=1

if __name__ == "__main__":
	exercise_1()
	exercise_2(19690720)
		

