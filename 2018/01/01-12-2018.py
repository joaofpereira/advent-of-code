import sys

if __name__ == "__main__":
    frequency = 0
    resulting_frequencies = {}

    has_found = False
    resulting_frequencies[0] = 1

    while has_found == False:
        for line in open("input.txt"):
            frequency += int(line)
            print("Frequency: " + str(frequency))

            if frequency in resulting_frequencies.keys():
                resulting_frequencies[frequency] += 1
                if resulting_frequencies[frequency] > 1:
                    has_found = True
                    print(frequency)
                    sys.exit()
            else:
                resulting_frequencies[frequency] = 1

            print("Result: " + str(resulting_frequencies[frequency]))

    print(str(frequency))