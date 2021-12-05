import os
import math
import re
import pathlib

############################ Day 1 ############################

def exercise_1_1():
    input_file = os.path.join(pathlib.Path(__file__).parent.resolve(), "input-1.txt")
    measurements = [int(x.strip()) for x in read_file(input_file)]
    depth_increases = 0
    for i in range(0, len(measurements)):
        if i == len(measurements) - 1:
            break
        if measurements[i] < measurements[i + 1]:
            depth_increases += 1
    return depth_increases

def exercise_1_2():
    input_file = os.path.join(pathlib.Path(__file__).parent.resolve(), "input-1.txt")
    measurements = [int(x.strip()) for x in read_file(input_file)]
    depth_increases = 0
    last_three_sum_measurement = 0
    for i in range(0, len(measurements)):
        # we break the loop when we reach i equals to the last but one 
        if i == len(measurements) - 2:
            break
        if i == 0:
            last_three_sum_measurement = measurements[i] + measurements[i + 1] + measurements[i + 2]
            continue
        new_three_sum_measurement = measurements[i] + measurements[i + 1] + measurements[i + 2]
        if last_three_sum_measurement < new_three_sum_measurement:
            depth_increases += 1
        last_three_sum_measurement = new_three_sum_measurement
    return depth_increases

############################ End of day 1 ############################

############################ Day 2 ############################

class Move:
    def __init__(self, line):
        parts = line.split(' ')
        self.direction = parts[0]
        self.count = int(parts[1])

    def __str__(self):
        return self.direction + " " + str(self.count)

class Point2D:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def __str__(self):
        return "(" + str(self.x) + ", " + str(self.y) + ")"

    def move(self, move):
        if move.direction == 'forward':
            self.x += move.count
        elif move.direction == 'down':
            self.y += move.count
        elif move.direction == 'up':
            self.y -= move.count

class Point3D:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

    def __str__(self):
        return "(" + str(self.x) + ", " + str(self.y) + ", " + str(self.z) + ")"

    def move(self, move):
        if move.direction == 'forward':
            self.x += move.count
            self.y += self.z * move.count
        elif move.direction == 'down':
            self.z += move.count
        elif move.direction == 'up':
            self.z -= move.count

def exercise_2_1():
    input_file = os.path.join(pathlib.Path(__file__).parent.resolve(), "input-2.txt")
    moves = [Move(line) for line in read_file(input_file)]
    point = Point2D(0, 0)
    for move in moves:
        point.move(move)
    return point.x * point.y

def exercise_2_2():
    input_file = os.path.join(pathlib.Path(__file__).parent.resolve(), "input-2.txt")
    moves = [Move(line) for line in read_file(input_file)]
    point = Point3D(0, 0, 0)
    for move in moves:
        point.move(move)
    return point.x * point.y

############################ End of day 2 ############################

############################ Day 3 ############################

def exercise_3_1():
    input_file = os.path.join(pathlib.Path(__file__).parent.resolve(), "input-3.txt")
    bits_representation_str = [line.strip() for line in read_file(input_file)]

    gamma_rate_bits = [0] * len(bits_representation_str[0]) 
    epsilon_rate_bits = [0] * len(bits_representation_str[0])

    for i in range(0, len(bits_representation_str[0])):
        bit_counts = [0] * 2
        for reprensentation in bits_representation_str:
            bit_counts[int(reprensentation[i])] += 1

        if (bit_counts[0] > bit_counts[1]):
            gamma_rate_bits[i] = 0
            epsilon_rate_bits[i] = 1
        else:
            gamma_rate_bits[i] = 1
            epsilon_rate_bits[i] = 0
    gamma_rate = int("".join([str(x) for x in gamma_rate_bits]))
    epsilon_rate = int("".join([str(x) for x in epsilon_rate_bits]))

    gamma_rate_decimal = binary_to_decimal(gamma_rate)
    epsilon_rate_decimal = binary_to_decimal(epsilon_rate)

    return gamma_rate_decimal * epsilon_rate_decimal

def binary_to_decimal(n):
    power_count = 0
    value = 0
    while(n != 0):
        power_value = n % 10
        if (power_value == 1):
            value += math.pow(2, power_count)
        n //= 10
        power_count += 1
    return value

def exercise_3_2():
    input_file = os.path.join(pathlib.Path(__file__).parent.resolve(), "input-3.txt")
    bits_representation_list = [line.strip() for line in read_file(input_file)]
    oxygen_generator_input = bits_representation_list
    co2_scrubber_input = bits_representation_list

    oxygen_generator_binary = filter_by_bit_criteria(oxygen_generator_input, 'most_common_bit', 0)
    co2_scrubber_binary = filter_by_bit_criteria(co2_scrubber_input, 'least_common_bit', 0)

    oxygen_generator_decimal = binary_to_decimal(int(oxygen_generator_binary))
    co2_scrubber_decimal = binary_to_decimal(int(co2_scrubber_binary))

    return oxygen_generator_decimal * co2_scrubber_decimal

def filter_by_bit_criteria(list_bits, filter_criteria, index):
    if len(list_bits) == 1:
        return list_bits[0]
    
    bit_counts = [0] * 2
    for representation in list_bits:
        bit_counts[int(representation[index])] += 1

    common_bit = ''
    if filter_criteria == 'most_common_bit':
        if bit_counts[0] > bit_counts[1]:
            common_bit = '0'
        else:
            common_bit = '1'
    else:
        if bit_counts[0] <= bit_counts[1]:
            common_bit = '0'
        else:
            common_bit = '1'
    
    filtered = list(filter(lambda bits: bits[index] == common_bit, list_bits))
    return filter_by_bit_criteria(filtered, filter_criteria, index + 1)

############################ End of day 3 ############################

############################ Day 4 ############################

class BingoCard:
    def __init__(self, lines):
        self.board = []
        self.marked = []
        for i in range(0, len(lines)):
            line = lines[i].strip()
            line = re.sub('  ', ' ', line)
            line_numbers = line.split(' ')
            self.board.append(line_numbers)
            self.marked.append([' '] * 5)
    
    def __str__(self):
        output = ""
        for line in self.board:
            line_str = ", ".join(number for number in line)
            output += line_str + "\n"

        output += '\n'
    
        for line in self.marked:
            line_str = ", ".join(spot for spot in line)
            output += line_str + "\n"
        return output

    def mark(self, number):
        for i in range(0, len(self.board)):
            for j in range(0, len(self.board[i])):
                if number == self.board[i][j]:
                    self.marked[i][j] = 'X'
    
    def wins(self):
        for line in self.marked:
            if line[0] == 'X' and line[1] == 'X' and line[2] == 'X' and line[3] == 'X' and line[4] == 'X':
                return True
        for i in range(0, len(self.marked)):
            if self.marked[0][i] == 'X' and self.marked[1][i] == 'X' and self.marked[2][i] == 'X' and self.marked[3][i] == 'X' and self.marked[4][i] == 'X':
                return True
        return False

    def sum_unmarked(self):
        count = 0
        for i in range(0, len(self.marked)):
            for j in range(0, len(self.marked)):
                if self.marked[i][j] != 'X':
                    count += int(self.board[i][j])
        return count

def build_boards(input_text):
    lines_on_card = 0
    bingo_cards = []
    bingo_lines = []
    for i in range(1, len(input_text)):
        if (input_text[i] == ""):
            continue
        
        bingo_lines.append(input_text[i])
        lines_on_card += 1

        if (lines_on_card == 5):
            lines_on_card = 0
            bingo_cards.append(BingoCard(bingo_lines))
            bingo_lines = []
            continue
    return bingo_cards

def exercise_4_1():
    input_file = os.path.join(pathlib.Path(__file__).parent.resolve(), "input-4.txt")
    input_text = [line.strip() for line in read_file(input_file)]
    input_plays = input_text[0].strip().split(',')

    bingo_cards = build_boards(input_text)
    for play in input_plays:
        for bingo_card in bingo_cards:
            bingo_card.mark(play)
            if (bingo_card.wins()):
                return int(play) * bingo_card.sum_unmarked()

def exercise_4_2():
    input_file = os.path.join(pathlib.Path(__file__).parent.resolve(), "input-4.txt")
    input_text = [line.strip() for line in read_file(input_file)]
    input_plays = input_text[0].strip().split(',')

    bingo_cards = build_boards(input_text)
    bingos_done = [False] * len(bingo_cards)
    for play in input_plays:
        for i in range(0, len(bingo_cards)):
            bingo_cards[i].mark(play)
            if (bingo_cards[i].wins()):
                bingos_done[i] = True
                if all(result == True for result in bingos_done):
                    return int(play) * bingo_cards[i].sum_unmarked()

############################ End of day 4 ############################

############################ Day 5 ############################

def exercise_5_1():
    return None

def exercise_5_2():
    return None

############################ End of day 5 ############################

############################ Day 6 ############################

def exercise_6_1():
    return None

def exercise_6_2():
    return None

############################ End of day 6 ############################

def read_file(filename):
    with open(filename, encoding='utf-8') as file:
        return file.readlines()

def read_file_as_str(filename):
    result = ""
    with open(filename, encoding='utf-8') as file:
        for x in file.readlines():
            result += str(x)
        return result

def main():
    #print(exercise_1_1())
    #print(exercise_1_2())
    #print(exercise_2_1())
    #print(exercise_2_2())
    #print(exercise_3_1())
    #print(exercise_3_2())
    #print(exercise_4_1())
    #print(exercise_4_2())
    print(exercise_5_1())
    print(exercise_5_2())
    #print(exercise_6_1())
    #print(exercise_6_2())

if __name__ == "__main__":
    main()
