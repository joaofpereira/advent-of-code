using AdventOfCode;

public static class Program
{
    public static void Main(string[] args)
    {
        // Day1();
        // Day2();
        // Day3();
        // Day4();
        Day5();
    }

    #region Day 1
    public static void Day1()
    {
        var input1 = Utils.ReadInput("./2022/1.txt");
        var elvesCalories = new List<int>();
        var currentElfCalories = 0;
        for (var i = 0; i < input1.Length; i++)
        {
            if (string.IsNullOrEmpty(input1[i]))
            {
                elvesCalories.Add(currentElfCalories);
                currentElfCalories = 0;
                continue;
            }

            currentElfCalories += int.Parse(input1[i]);
        }
        Console.WriteLine("Output 1.1: {0}", elvesCalories.Max());
        Console.WriteLine("Output 1.2: {0}", elvesCalories.OrderByDescending(v => v).Take(3).Sum());
    }
    #endregion

    #region Day 2
    public static void Day2()
    {
        var input2 = Utils.ReadInput("./2022/2.txt");
        const int lossScore = 0;
        const int drawScore = 3;
        const int winScore = 6;
        var gameScores = new Dictionary<string, int>()
        {
            { "A", 1 },
            { "X", 1 },
            { "B", 2 },
            { "Y", 2 },
            { "C", 3 },
            { "Z", 3 },
        };

        bool IsSamePlay(string himChoice, string myChoice)
        {
            if (himChoice == "A" && myChoice == "X") return true;
            else if (himChoice == "B" && myChoice == "Y") return true;
            else if (himChoice == "C" && myChoice == "Z") return true;
            else return false;
        }

        int RockPaperScissorCalculator(string himChoice, string myChoice, Dictionary<string, int> gameScores)
        {
            var myRoundScore = gameScores[myChoice];
            if (IsSamePlay(himChoice, myChoice))
                return myRoundScore + drawScore;

            if (himChoice == "A")
            {
                if (myChoice == "Y")
                    return myRoundScore + winScore;
                else
                    return myRoundScore + lossScore;
            }
            else if (himChoice == "B")
            {
                if (myChoice == "X")
                    return myRoundScore + lossScore;
                else
                    return myRoundScore + winScore;
            }
            else
            {
                if (myChoice == "X")
                    return myRoundScore + winScore;
                else
                    return myRoundScore + lossScore;
            }
        }

        var myScore = 0;
        for (var i = 0; i < input2.Length; i++)
        {
            var parts = input2[i].Split(' ');
            myScore += RockPaperScissorCalculator(parts[0], parts[1], gameScores);
        }

        Console.WriteLine("Output 2.1: {0}", myScore);

        string RockPaperScissorAutoFiller(string himChoice, string expectedResult)
        {
            // in case the final result is a draw we need to get the equivalent move
            if (expectedResult == "Y")
            {
                if (himChoice == "A") // rock vs rock
                    return "X";
                else if (himChoice == "B") // paper vs paper
                    return "Y";
                else return "Z"; // scissor vs scissor
            }
            else if (expectedResult == "X")
            {
                if (himChoice == "A") // rock vs scissor
                    return "Z";
                else if (himChoice == "B") // paper vs rock
                    return "X";
                else return "Y"; // scissor vs paper 
            }
            else
            {
                if (himChoice == "A") // rock vs paper
                    return "Y";
                else if (himChoice == "B") // paper vs scissor
                    return "Z";
                else return "X"; // scissor vs rock
            }
        }

        myScore = 0;
        for (var i = 0; i < input2.Length; i++)
        {
            var parts = input2[i].Split(' ');
            var myMove = RockPaperScissorAutoFiller(parts[0], parts[1]);
            myScore += RockPaperScissorCalculator(parts[0], myMove, gameScores);
        }

        Console.WriteLine("Output 2.2: {0}", myScore);
    }
    #endregion

    #region Day 3
    public static void Day3()
    {
        var input3 = Utils.ReadInput("./2022/3.txt");
        var firstCompartiments = new List<string>();
        var secondCompartiments = new List<string>();
        var sharedItemSum = 0;
        for (var i = 0; i < input3.Length; i++)
        {
            var compartimentLength = input3[i].Length / 2;
            var firstCompartiment = input3[i].Substring(0, compartimentLength);
            var secondCompartiment = input3[i].Substring(compartimentLength, compartimentLength);
            firstCompartiments.Add(firstCompartiment);
            secondCompartiments.Add(secondCompartiment);
            var sharedItem = GetSharedItem(firstCompartiment, secondCompartiment);
            if (sharedItem.HasValue)
            {
                var sharedItemPriority = GetSharedItemPriority(sharedItem.Value);
                // Console.WriteLine(sharedItemPriority);
                sharedItemSum += sharedItemPriority;
            }
        }

        Console.WriteLine("Output 3.1: {0}", sharedItemSum);

        int GetSharedItemPriority(char item)
        {
            const int fixedLowerCasePriority = 1;
            const int fixedUpperCasePriority = 27;
            var charValue = (int)item;

            // 'a' is 97 so less than that is uppercase ('A' starts at 65)
            if (charValue < 97)
                return fixedUpperCasePriority + (charValue - (int)'A');
            else
                return fixedLowerCasePriority + (charValue - (int)'a');
        }

        char? GetSharedItem(string str1, string str2)
        {
            for (var i = 0; i < str1.Length; i++)
                for (var j = 0; j < str2.Length; j++)
                    if (str1[i] == str2[j])
                        return str1[i];
            return null;
        }

        char? GetCommonBadge(string[] elvesGroup)
        {
            for (var i = 0; i < elvesGroup[0].Length; i++)
                for (var j = 0; j < elvesGroup[1].Length; j++)
                    for (var z = 0; z < elvesGroup[2].Length; z++)
                        if (elvesGroup[0][i] == elvesGroup[1][j] && elvesGroup[1][j] == elvesGroup[2][z])
                            return elvesGroup[0][i];
            return null;
        }

        var commonBadgeSum = 0;
        for (var i = 0; i < input3.Length; i += 3)
        {
            var group = new string[] { input3[i], input3[i + 1], input3[i + 2] };
            var commonBadge = GetCommonBadge(group);
            // Console.WriteLine("Common Badge: {0}", commonBadge);
            if (commonBadge.HasValue)
            {
                var sharedItemPriority = GetSharedItemPriority(commonBadge.Value);
                // Console.WriteLine("Common Badge Priority: {0}", sharedItemPriority);
                commonBadgeSum += sharedItemPriority;
            }
        }

        Console.WriteLine("Output 3.2: {0}", commonBadgeSum);
    }
    #endregion

    #region Day 4
    struct ElfAssignedSection
    {
        public int LowerSection;
        public int HigherSection;

        public ElfAssignedSection(int lowerSection, int higherSection)
        {
            LowerSection = lowerSection;
            HigherSection = higherSection;
        }

        public bool IsOverlappedTotally(in ElfAssignedSection other)
        {
            if (LowerSection >= other.LowerSection)
            {
                if (HigherSection <= other.HigherSection)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOverlapped(in ElfAssignedSection other)
        {
            if (LowerSection == other.LowerSection || HigherSection == other.HigherSection ||
                LowerSection == other.HigherSection || HigherSection == other.LowerSection) return true;
            if (LowerSection < other.LowerSection && HigherSection >= other.LowerSection) return true;
            if (LowerSection > other.LowerSection && LowerSection <= other.HigherSection) return true;

            return false;
        }

        public override string ToString()
        {
            return $"Lower Section: {LowerSection}\tHigher Section: {HigherSection}";
        }
    }

    public static void Day4()
    {
        var input4 = Utils.ReadInput("./2022/4.txt");
        var elfPairs = input4.Select(line =>
        {
            var parts = line.Split(',');
            var elf1Sections = parts[0].Split('-').Select(e => int.Parse(e)).ToArray();
            var elf2Sections = parts[1].Split('-').Select(e => int.Parse(e)).ToArray();
            return (new ElfAssignedSection(elf1Sections[0], elf1Sections[1]), new ElfAssignedSection(elf2Sections[0], elf2Sections[1]));
        });
        var overlappedElves = 0;
        foreach (var elfPair in elfPairs)
        {
            // Console.WriteLine("Elf1:\n\t{0},\nElf2:\n\t{1}\n\n", elfPair.Item1, elfPair.Item2);
            if (elfPair.Item1.IsOverlappedTotally(elfPair.Item2) || elfPair.Item2.IsOverlappedTotally(elfPair.Item1))
            {
                overlappedElves++;
                continue;
            }
        }

        Console.WriteLine("Output 4.1: {0}", overlappedElves);

        overlappedElves = 0;
        foreach (var elfPair in elfPairs)
        {
            // Console.WriteLine("Elf1:\n\t{0},\nElf2:\n\t{1}\n\n", elfPair.Item1, elfPair.Item2);
            if (elfPair.Item1.IsOverlapped(elfPair.Item2))
            {
                overlappedElves++;
                continue;
            }
        }
        Console.WriteLine("Output 4.2: {0}", overlappedElves);
    }
    #endregion

    #region Day 5

    private static LinkedList<char>[] CreateEmptyCrates(int numberOfCrates)
    {
        var crateStacks = new LinkedList<char>[numberOfCrates];
        for (var i = 0; i < crateStacks.Length; i++)
        {
            crateStacks[i] = new LinkedList<char>();
        }
        return crateStacks;
    }

    private static void InitializeCrateStacks(IReadOnlyList<string> input, LinkedList<char>[] crateStacks)
    {
        var currentCrateStackIndex = 0;
        foreach (var crateInputLine in input)
        {
            var lineIndex = 0;
            while (lineIndex < crateInputLine.Length)
            {
                // whenever we see an empty space we move forward in the line to fill the next crate stack
                if (crateInputLine[lineIndex] == ' ')
                {
                    lineIndex += 4;
                    currentCrateStackIndex++;
                    continue;
                }
                // if there's value we put the value of the next cell
                crateStacks[currentCrateStackIndex].AddFirst(crateInputLine[lineIndex + 1]);
                lineIndex += 4;
                currentCrateStackIndex++;
            }
            currentCrateStackIndex = 0;
        }
    }

    public static void Day5()
    {
        var input5 = Utils.ReadInput("./2022/5.txt");
        var crateStacksInput = new List<string>();
        var movesInput = new List<string>();
        var wasEmptyLineRead = false;
        var previousLine = string.Empty;
        var maxCrateStack = 0;
        foreach(var line in input5)
        {
            // here we reach the empty line and we can use the previous line to get the max stack crate in the input
            if (string.IsNullOrWhiteSpace(line))
            {
                wasEmptyLineRead = true;
                maxCrateStack = previousLine.Split(' ').Where(c => string.IsNullOrWhiteSpace(c) == false).Select(c => int.Parse(c)).Max();
                continue;
            }
                
            if (!wasEmptyLineRead)
                crateStacksInput.Add(line);
            else
                movesInput.Add(line);

            previousLine = line;
        }

        var crateStacks = CreateEmptyCrates(maxCrateStack);
        InitializeCrateStacks(crateStacksInput, crateStacks);

        foreach(var moveInput in movesInput)
        {
            var parts = moveInput.Split(' ');
            var numberOfCrates = int.Parse(parts[1]);
            var fromStack = int.Parse(parts[3]) - 1;
            var toStack = int.Parse(parts[5]) - 1;

            for (var i = 0; i < numberOfCrates; i++)
            {
                var lastCrateOnStack = crateStacks[fromStack].Last;
                if (lastCrateOnStack != null)
                {
                    crateStacks[fromStack].RemoveLast();
                    crateStacks[toStack].AddLast(lastCrateOnStack);
                }
            }
        }

        Console.WriteLine("Output 5.1: {0}", string.Join("", crateStacks.Select(c => c.Last.Value)));

        crateStacks = CreateEmptyCrates(maxCrateStack);
        InitializeCrateStacks(crateStacksInput, crateStacks);

        foreach (var moveInput in movesInput)
        {
            var parts = moveInput.Split(' ');
            var numberOfCrates = int.Parse(parts[1]);
            var fromStack = int.Parse(parts[3]) - 1;
            var toStack = int.Parse(parts[5]) - 1;

            var tempCrateStack = new LinkedList<char>();

            for (var i = 0; i < numberOfCrates; i++)
            {
                var lastCrateOnStack = crateStacks[fromStack].Last;
                if (lastCrateOnStack != null)
                {
                    crateStacks[fromStack].RemoveLast();
                    tempCrateStack.AddFirst(lastCrateOnStack);
                }
            }

            for(var i = 0; i < numberOfCrates; i++)
            {
                var firstCrateOnStack = tempCrateStack.First;
                if (firstCrateOnStack != null)
                {
                    tempCrateStack.RemoveFirst();
                    crateStacks[toStack].AddLast(firstCrateOnStack);
                }
            }
        }

        Console.WriteLine("Output 5.2: {0}", string.Join("", crateStacks.Select(c => c.Last.Value)));
    }
    #endregion
}