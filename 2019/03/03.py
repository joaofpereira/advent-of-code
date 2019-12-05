import sys
import math

def read_input():
	with open('./input.txt', 'r') as input:
		return [i.split(',') for i in input.readlines()]

def merge_wires_path(collection, wire_collection):
	for wire_pair in wire_collection:
		if wire_pair in collection:
			collection[wire_pair] = collection[wire_pair] + 1
		else:
			collection[wire_pair] = 1

def calculate_manhattan_distance(initial_x, initial_y, final_x, final_y):
	 return abs(initial_x - final_x) + abs(initial_y - final_y)

def calculate_euclidean_distance(initial_x, initial_y, final_x, final_y):
	return math.sqrt(pow(final_x - initial_x, 2) + pow(final_y - initial_y, 2))

def exercise_1(wires):
	coordinates_collection = dict()
	current_x = 0
	current_y = 0
	for wire in wires:
		wire_coordinate_collection = dict()

		for i in wire:
			if i[0] == 'U':
				for i in range(0, int(i[1:].strip())):
					current_y += 1
					wire_coordinate_collection[(current_x, current_y)] = 1
			elif i[0] == 'D':
				for i in range(0, int(i[1:].strip())):
					current_y -= 1
					wire_coordinate_collection[(current_x, current_y)] = 1
			elif i[0] == 'L':
				for i in range(0, int(i[1:].strip())):
					current_x -= 1
					wire_coordinate_collection[(current_x, current_y)] = 1
			elif i[0] == 'R':
				for i in range(0, int(i[1:].strip())):
					current_x += 1
					wire_coordinate_collection[(current_x, current_y)] = 1

		merge_wires_path(coordinates_collection, wire_coordinate_collection)
		current_x = 0
		current_y = 0
	
	min_manhattan_distance = sys.maxsize

	closest_coordinate = None

	"""for coordinate in coordinates_collection:
		if coordinates_collection[coordinate] > 1:
			distance = calculate_euclidean_distance(0, 0, coordinate[0], coordinate[1])
			if distance < min_euclidean_distance:
				min_euclidean_distance = distance
				closest_coordinate = coordinate"""
	for coordinate in coordinates_collection:
		if coordinates_collection[coordinate] > 1:
			distance = calculate_manhattan_distance(0, 0, coordinate[0], coordinate[1])
			if distance < min_manhattan_distance:
				min_manhattan_distance = distance
				closest_coordinate = coordinate
	print(closest_coordinate)
	print("Distance: " + str(min_manhattan_distance))
	print("Distance: " + str(calculate_manhattan_distance(0, 0, coordinate[0], coordinate[1])))
	
if __name__ == "__main__":
	wires = read_input()
	exercise_1(wires)