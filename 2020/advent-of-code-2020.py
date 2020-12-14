def read_file(filename):
    with open(filename, encoding='utf-8') as file:
        return file.readlines()

def exercise_1_1():
    numbers = [int(x.strip()) for x in read_file("input-1.txt")]
    for number in numbers[0:len(numbers) - 2]:
        for number2 in numbers[1:len(numbers) - 1]:
            if number + number2 == 2020:
                return number * number2

def exercise_1_2():
    numbers = [int(x.strip()) for x in read_file("input-1.txt")]
    for number in numbers[0:len(numbers) - 3]:
        for number2 in numbers[1:len(numbers) - 2]:
            for number3 in numbers[2:len(numbers) - 1]:
                if number + number2 + number3 == 2020:
                    return number * number2 * number3

class Password:
    def __init__(self, line):
        parts = line.split(' ')
        min_max = parts[0].split('-')
        self.anchor_letter = parts[1][:-1] # remove last char
        self.min = int(min_max[0])
        self.max = int(min_max[1])
        self.password = parts[2]
    
    def __str__(self):
        return "Password: " + self.password + "\tMin: " + str(self.min) + "\tMax: " + str(self.max) + "\tAnchor Letter: " + self.anchor_letter
    
    def is_valid(self):
        counts = {}
        for c in self.password:
            if not c in counts:
                counts[c] = 1
            else:
                counts[c] = counts[c] + 1
        return self.anchor_letter in counts.keys() and counts[self.anchor_letter] <= self.max and counts[self.anchor_letter] >= self.min

    def is_according_corporation(self):
        if self.anchor_letter == self.password[self.min - 1] and self.anchor_letter == self.password[self.max - 1]:
            return False
        elif self.anchor_letter == self.password[self.min - 1]:
            return True
        elif self.anchor_letter == self.password[self.max - 1]:
            return True
        else:
            return False

def exercise_2_1():
    passwords = [Password(x.strip()) for x in read_file("input-2.txt")]
    count = 0
    for password in passwords:
        if password.is_valid():
            count = count + 1
    return count

def exercise_2_2():
    passwords = [Password(x.strip()) for x in read_file("input-2.txt")]
    count = 0
    for password in passwords:
        if password.is_according_corporation():
            count = count + 1
    return count

def main():
    #print(exercise_1_1())
    #print(exercise_1_2())
    #print(exercise_2_1())
    print(exercise_2_2())

if __name__ == "__main__":
    main()