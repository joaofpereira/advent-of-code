import sys

def calculate_fuel(input):
	return int(int(input) / 3 - 2)

def read_input():
	input_list = []
	with open('./input.txt', 'r') as input:
		line = input.readline()
		while line:
			input_list.append(int(line))
			line = input.readline()

	return input_list

def exercise_1():
	result = 0
	with open('./result_1.txt', 'w') as output:
		for entry in read_input():
			result += calculate_fuel(entry)
		output.write(str(result))

def exercise_2():
	result = 0
	with open('./result_2.txt', 'w') as output:
		for entry in read_input():
			fuel = calculate_fuel(entry)
			while fuel >= 0:
				result += fuel
				fuel = calculate_fuel(fuel)
		output.write(str(result))

if __name__ == "__main__":
	exercise_1()
	exercise_2()
