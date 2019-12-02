import itertools

def get_number_of_letters(word):
    counts = {}
    for letter in word.strip():
        if letter in counts.keys():
            counts[letter] += 1
        else:
            counts[letter] = 1
    return counts

def levenshtein(s1, s2):
    if len(s1) < len(s2):
        return levenshtein(s2, s1)

    # len(s1) >= len(s2)
    if len(s2) == 0:
        return len(s1)

    previous_row = range(len(s2) + 1)
    for i, c1 in enumerate(s1):
        current_row = [i + 1]
        for j, c2 in enumerate(s2):
            insertions = previous_row[j + 1] + 1
            deletions = current_row[j] + 1
            substitutions = previous_row[j] + (c1 != c2)
            current_row.append(min(insertions, deletions, substitutions))
        previous_row = current_row
    
    return previous_row[-1]


def get_diff_strings(entry):
    words = entry.split(',')

    print(words)

    result = []

    for idx, val in enumerate(words[0]):
        if val == words[1][idx]:
            result.append(val)
        
    return result

if __name__ == "__main__":
    '''counts = {}

    for line in open("input.txt"):
        counts_per_word = get_number_of_letters(line)
        has_2 = False
        has_3 = False
        for letter in counts_per_word.keys():
            if counts_per_word[letter] == 2 and has_2 == False:
                if 2 in counts.keys():
                    counts[2] += 1
                else:
                    counts[2] = 1
                has_2 = True
            elif counts_per_word[letter] == 3 and has_3 == False:
                if 3 in counts.keys():
                    counts[3] += 1
                else:
                    counts[3] = 1
                has_3 = True
    
    print(str(counts[2] * counts[3]))'''

    words = []
    most_similar_words = {}

    for line in open("input.txt"):
        words.append(line.strip())

    for a, b in itertools.combinations(words, 2):
        distance = levenshtein(a, b)
        most_similar_words[a + "," + b] = distance

    minimum = None
    word = None
    for entry in most_similar_words.keys():
        if minimum == None or most_similar_words[entry] < minimum:
            word = entry
            minimum = most_similar_words[entry]

    print(word)
    print(minimum)
        
    result = get_diff_strings(word)

    print(''.join(result))

    