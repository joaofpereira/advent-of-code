using AdventOfCode;
using System.Numerics;
using System.Text;

public static class Program
{
    public static void Main(string[] args)
    {
        // Day1();
        // Day2();
        // Day3();
        // Day4();
        // Day5();
        // Day6();
        // Day7();
        // Day8();
        // Day9();
        // Day10();
        Day11();
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

    private static LinkedList<char>[] CreateEmptyCrateStacks(int numberOfCrates)
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
        foreach (var line in input5)
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

        var crateStacks = CreateEmptyCrateStacks(maxCrateStack);
        InitializeCrateStacks(crateStacksInput, crateStacks);

        foreach (var moveInput in movesInput)
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

        crateStacks = CreateEmptyCrateStacks(maxCrateStack);
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

            for (var i = 0; i < numberOfCrates; i++)
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

    #region Day 6

    private static int CalculateMarker(string message, int markerLength)
    {
        var hashSet = new HashSet<char>();
        for (var i = 0; i < message.Length; i++)
        {
            // when surpassing the minimum required length to perform the marker
            if (i == message.Length - (markerLength + 1))
                return -1;

            var charGroup = message.Substring(i, markerLength);

            foreach (var c in charGroup)
            {
                if (hashSet.Contains(c))
                {
                    hashSet.Clear();
                    break;
                }
                else hashSet.Add(c);
            }
            if (hashSet.Count == markerLength)
            {
                return i + markerLength;
            }
        }

        return -1;
    }

    public static void Day6()
    {
        var input6 = Utils.ReadInput("./2022/6.txt")[0];

        Console.WriteLine("Output 6.1: {0}", CalculateMarker(input6, 4));
        Console.WriteLine("Output 6.2: {0}", CalculateMarker(input6, 14));
    }
    #endregion

    #region Day 7

    private abstract class Item
    {
        public string Name;
        public int Size;

        protected Item(string name, int size)
        {
            Name = name;
            Size = size;
        }

        public abstract int GetSize();
        public abstract string ToString(int level);
    }

    private class File : Item
    {
        public File(string name, int size) : base(name, size) { }
        public override int GetSize() { return Size; }

        public override string ToString(int level)
        {
            return $"{Tabs(level)}- {Name} (file, size={Size})";
        }
    }

    private class Folder : Item
    {
        public List<Item> Items { get; set; }
        public Folder Parent { get; set; }

        public Folder(string name, int size = 0) : base(name, size)
        {
            Items = new List<Item>();
        }

        public Folder(Folder parent, string name, int size = 0) : base(name, size)
        {
            Parent = parent;
            Items = new List<Item>();
        }

        public override int GetSize()
        {
            var totalSize = 0;
            foreach (var item in Items)
            {
                totalSize += item.GetSize();
            }
            return totalSize;
        }

        public Folder CreateFolder(string folderName)
        {
            var folder = new Folder(this, folderName);
            Items.Add(folder);
            return folder;
        }

        public void CreateFile(string fileName, int size)
        {
            Items.Add(new File(fileName, size));
        }

        public override string ToString(int level)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine($"{Tabs(level)}- {Name} (dir)");
            var innerLevel = level + 1;
            foreach(var item in Items)
            {
                strBuilder.AppendLine($"{item.ToString(innerLevel)}");
            }
            return strBuilder.ToString().TrimEnd();
        }

        public IEnumerable<(string FolderName, int Size)> GetFolderAndInnerFolderInfo()
        {
            yield return (Name, GetSize());
            // even if we dont return the current one, there might be some child folder that obeys the rule
            foreach(var item in Items)
            {
                if (item is Folder innerFolder)
                {
                    foreach(var innerFolderItem in innerFolder.GetFolderAndInnerFolderInfo())
                        yield return innerFolderItem;
                }
            }
        }

        public IEnumerable<int> GetInnerFolderSizes()
        {
            foreach (var item in Items)
            {
                if (item is Folder innerFolder)
                {
                    foreach(var innerSize in innerFolder.GetInnerFolderSizes())
                        yield return innerSize;
                }
            }
            yield return GetSize();
        }
    }

    private class FileSystem
    {
        public const int TotalDiskSize = 70000000;

        public List<Item> Items;
        public Folder CurrentFolder;

        public FileSystem()
        {
            Items = new List<Item>();
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            foreach(var item in Items)
            {
                strBuilder.AppendLine(item.ToString(0));
            }
            return strBuilder.ToString();
        }

        public IEnumerable<(string Folder, int Size)> GetFoldersOfAtMost(int? capSize)
        {
            foreach (var i in Items)
            {
                if (i is Folder innerFolder)
                {
                    foreach (var info in innerFolder.GetFolderAndInnerFolderInfo())
                    {
                        // if no cap size is passed we just return everything
                        if (!capSize.HasValue || info.Size <= capSize)
                        {
                            // Console.WriteLine($"Folder: {info.FolderName}\nSize: {info.Size}");
                            yield return info;
                        }
                    }
                }
            }
        }

        public int GetFolderSizes(int capSize)
        {
            var totalSize = 0;
            foreach(var folderInfo in GetFoldersOfAtMost(capSize))
            {
                totalSize += folderInfo.Size;
            }
            return totalSize;
        }

        public int GetSize()
        {
            var size = 0;
            foreach(var item in Items)
            {
                size += item.GetSize();
            }
            return size;
        }

        public int FindSmallestDirectoryToDelete(int requiredSpace)
        {
            var minDirectorySize = int.MaxValue;
            var currentOccupiedSpace = GetSize();

            foreach (var info in GetFoldersOfAtMost(null))
            {
                var newFreeSpace = TotalDiskSize - (currentOccupiedSpace - info.Size);
                if (newFreeSpace >= requiredSpace && minDirectorySize > info.Size)
                    minDirectorySize = info.Size;
            }

            return minDirectorySize;
        }
    }

    static void ProcessCommand(FileSystem fileSystem, string command)
    {
        var parts = command.Split(' ');
        switch(parts[0])
        {
            case "$":
                ProcessCommand(fileSystem, string.Join(" ", parts.Where((v, i) => i > 0)));
                break;
            case "cd":
                if (parts[1] == "..")
                {
                    if (fileSystem.CurrentFolder.Parent != null)
                        fileSystem.CurrentFolder = fileSystem.CurrentFolder.Parent;
                }
                    
                else if (fileSystem.CurrentFolder == null)
                {
                    var folder = new Folder(parts[1]);
                    fileSystem.Items.Add(folder);
                    fileSystem.CurrentFolder = folder;
                }
                else
                {
                    if (fileSystem.CurrentFolder.Items.FirstOrDefault(f => f.Name == parts[1]) is Folder folder)
                        fileSystem.CurrentFolder = folder;
                }
                break;
            case "dir":
                fileSystem.CurrentFolder.CreateFolder(parts[1]);
                break;
            case "ls":
                break;
            default:
                fileSystem.CurrentFolder.CreateFile(parts[1], int.Parse(parts[0]));
                break;
        }
    }

    static string Tabs(int n)
    {
        return new string('\t', n);
    }

    public static void Day7()
    {
        var input7 = Utils.ReadInput("./2022/7.txt");
        var fileSystem = new FileSystem();
        foreach(var command in input7)
        {
            ProcessCommand(fileSystem, command);
        }
        // Console.WriteLine(fileSystem.ToString());
        Console.WriteLine("Output 7.1: {0}", fileSystem.GetFolderSizes(100000));
        Console.WriteLine("Output 7.2: {0}", fileSystem.FindSmallestDirectoryToDelete(30000000));
    }

    #endregion

    #region Day 8

    private class ForestGrid
    {
        private int[][] _grid;
        public ForestGrid(int length)
        {
            _grid = new int[length][];
            for (var i = 0; i < length; i++)
            {
                _grid[i] = new int[length];
            }
        }

        public void PopulateGrid(string[] input)
        {
            var currentIndex = 0;
            foreach (var inputLine in input)
            {
                for (var i = 0; i < inputLine.Length; i++)
                {
                    _grid[currentIndex][i] = inputLine[i] - '0';
                }
                currentIndex++;
            }
        }

        public string Print()
        {
            var strBuilder = new StringBuilder();
            foreach (var line in _grid)
            {
                strBuilder.AppendLine(string.Join(" ", line));
            }
            return strBuilder.ToString();
        }

        public int CalculateTreesVisibleFromOutside(out int highestScenicScore)
        {
            var edgeTrees = _grid.Length * 2 + (_grid.Length - 2) * 2;
            var visibleTreesHashset = new HashSet<(int, int)>();
            highestScenicScore = 0;

            for (var i = 1; i < _grid.Length - 1; i++)
            {
                for (var j = 1; j < _grid.Length - 1; j++)
                {
                    var isVisableFromWest = IsVisableFromWest(i, j, out var westScenicScore);
                    var isVisableFromEast = IsVisableFromEast(i, j, out var eastScenicScore);
                    var isVisableFromNorth = IsVisableFromNorth(j, i, out var northScenicScore);
                    var isVisableFromSouth = IsVisableFromSouth(j, i, out var southScenicScore);

                    if (isVisableFromWest || isVisableFromEast || isVisableFromNorth || isVisableFromSouth)
                    {
                        // Console.WriteLine("Added: {0},{1}", i, j);
                        visibleTreesHashset.Add((i, j));
                    }

                    var calculatedScenicScore = westScenicScore * eastScenicScore * northScenicScore * southScenicScore;
                    // Console.WriteLine($"({i},{j}) N: {northScenicScore}, S: {southScenicScore}, E: {eastScenicScore}, W: {westScenicScore}, Score: {calculatedScenicScore}");
                    if (calculatedScenicScore > highestScenicScore)
                        highestScenicScore = calculatedScenicScore;
                }
            }
            // Console.WriteLine("HashSet Count: {0}", visibleTreesHashset.Count);
            return edgeTrees + visibleTreesHashset.Count;
        }

        private bool IsVisableFromWest(int rowIndex, int endIndex, out int scenicScore)
        {
            scenicScore = 0;
            for (var i = endIndex - 1; i >= 0; i--)
            {
                scenicScore++;
                // Console.WriteLine($"West [{rowIndex}{endIndex}] {_grid[rowIndex][i]} >= {_grid[rowIndex][endIndex]}");
                if (_grid[rowIndex][i] >= _grid[rowIndex][endIndex])
                    return false;
            }
            return true;
        }

        private bool IsVisableFromEast(int rowIndex, int startIndex, out int scenicScore)
        {
            scenicScore = 0;
            for (var i = startIndex + 1; i < _grid.Length; i++)
            {
                scenicScore++;
                // Console.WriteLine($"East [{rowIndex}{startIndex}] {_grid[rowIndex][startIndex]} =< {_grid[rowIndex][i]}");
                if (_grid[rowIndex][startIndex] <= _grid[rowIndex][i])
                    return false;
            }
            return true;
        }

        private bool IsVisableFromNorth(int columnIndex, int endIndex, out int scenicScore)
        {
            scenicScore = 0;
            for (var i = endIndex - 1; i >= 0; i--)
            {
                scenicScore++;
                // Console.WriteLine($"North [{endIndex}{columnIndex}] {_grid[i][columnIndex]} >= {_grid[endIndex][columnIndex]}");
                if (_grid[i][columnIndex] >= _grid[endIndex][columnIndex])
                    return false;
            }
            return true;
        }

        private bool IsVisableFromSouth(int columnIndex, int startIndex, out int scenicScore)
        {
            scenicScore = 0;
            for (var i = startIndex + 1; i < _grid.Length; i++)
            {
                scenicScore++;
                // Console.WriteLine($"South [{startIndex}{columnIndex}] {_grid[startIndex][columnIndex]} <= {_grid[i][columnIndex]}");
                if (_grid[startIndex][columnIndex] <= _grid[i][columnIndex])
                    return false;
            }
            return true;
        }
    }

    public static void Day8()
    {
        var input8 = Utils.ReadInput("./2022/8.txt");
        var forestGrid = new ForestGrid(input8[0].Length);
        forestGrid.PopulateGrid(input8);
        // Console.WriteLine(forestGrid.Print());
        Console.WriteLine("Output 8.1: {0}", forestGrid.CalculateTreesVisibleFromOutside(out var highestScenicScore));
        Console.WriteLine("Output 8.2: {0}", highestScenicScore);
    }

    #endregion

    #region Day 9

    private class Knot
    {
        public Position Head;
        public Position Tail;

        public Knot(Position head)
        {
            Head = head;
            Tail = new Position(0, 0);
        }
    }

    private class Position
    {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void MoveUp()
        {
            X += 1;
        }

        public void MoveDown()
        {
            X -= 1;
        }

        public void MoveRight()
        {
            Y += 1;
        }

        public void MoveLeft()
        {
            Y -= 1;
        }

        public int GetHightestCoordinate()
        {
            var x = Math.Abs(X);
            var y = Math.Abs(Y);
            return x > y ? x : y;
        }
    }

    private static void MoveTail(Position tail, Position head, HashSet<(int, int)>? tailVisitedPositions = null)
    {
        var deltaX = head.X - tail.X;
        var deltaY = head.Y - tail.Y;

        if (deltaX == 0)
        {
            if (deltaY == 2)
                tail.MoveRight();
            else if (deltaY == -2)
                tail.MoveLeft();
        }
        else if (deltaY == 0)
        {
            if (deltaX == 2)
                tail.MoveUp();
            else if (deltaX == -2)
                tail.MoveDown();
        }
        else if (deltaX == 2)
        {
            tail.MoveUp();
            if (deltaY > 0)
                tail.MoveRight();
            else
                tail.MoveLeft();
        }
        else if (deltaX == -2)
        {
            tail.MoveDown();
            if (deltaY > 0)
                tail.MoveRight();
            else
                tail.MoveLeft();
        }
        else if (deltaY == 2)
        {
            tail.MoveRight();
            if (deltaX > 0)
                tail.MoveUp();
            else
                tail.MoveDown();
        }
        else if (deltaY == -2)
        {
            tail.MoveLeft();
            if (deltaX > 0)
                tail.MoveUp();
            else
                tail.MoveDown();
        }
        tailVisitedPositions?.Add((tail.X, tail.Y));
    }

    private static void PrintKnots(IList<Knot> knots)
    {
        var grid = new char[MaxGridSize][];
        for (var i = 0; i < grid.Length; i++)
        {
            grid[i] = Enumerable.Repeat('o', MaxGridSize).ToArray();
        }

        var deltaX = knots.Min(knot =>
        {
            return knot.Head.X < knot.Tail.X ? knot.Head.X : knot.Tail.X;
        });
        var deltaY = knots.Min(knot =>
        {
            return knot.Head.Y < knot.Tail.Y ? knot.Head.Y : knot.Tail.Y;
        });

        bool negativeIndexes = false;
        if (deltaX < 0)
        {
            deltaX = Math.Abs(deltaX);
            negativeIndexes = true;
        }
        if (deltaY < 0)
        {
            deltaY = Math.Abs(deltaY);
            negativeIndexes = true;
        }

        for (var i = knots.Count - 1; i >= 0; i--)
        {
            if (negativeIndexes)
            {
                if (i != knots.Count - 1)
                    grid[knots[i].Tail.X + deltaX][knots[i].Tail.Y + deltaY] = Convert.ToChar(48 + i + 1);
                grid[knots[i].Head.X + deltaX][knots[i].Head.Y + deltaY] = Convert.ToChar(48 + i);
            }
            else
            {
                if (i != knots.Count - 1)
                    grid[knots[i].Tail.X][knots[i].Tail.Y] = Convert.ToChar(48 + i + 1);
                grid[knots[i].Head.X][knots[i].Head.Y] = Convert.ToChar(48 + i);
            }
        }

        var strBuilder = new StringBuilder();
        for (var i = MaxGridSize - 1; i >= 0; i--)
        {
            var format = string.Join(' ', grid[i]);
            strBuilder.AppendLine(format);
        }
        Console.WriteLine(strBuilder.ToString());
    }

    private static void ProcessMove(string move, IList<Knot> knots, HashSet<(int, int)> tailVisitedPositions)
    {
        var moveDirection = move[0];
        var movesCount = int.Parse(move.Substring(2, move.Length - 2));
        for (var i = 0; i < movesCount; i++)
        {
            switch (moveDirection)
            {
                case 'U':
                    knots[0].Head.MoveUp();
                    break;
                case 'D':
                    knots[0].Head.MoveDown();
                    break;
                case 'L':
                    knots[0].Head.MoveLeft();
                    break;
                case 'R':
                    knots[0].Head.MoveRight();
                    break;
            }
            for(var j = 0; j < knots.Count; j++)
            {
                if (j == knots.Count - 2)
                    MoveTail(knots[j].Tail, knots[j].Head, tailVisitedPositions);
                else
                    MoveTail(knots[j].Tail, knots[j].Head, null);
            }
        }
    }

    private static int CalculateTailVisitedPositions(int numKnots, string[] input)
    {
        var tailVisitedPositions = new HashSet<(int, int)>();
        var knots = new List<Knot>();
        var currentHead = new Position(0, 0);
        for (var i = 0; i < numKnots; i++)
        {
            knots.Add(new Knot(currentHead));
            currentHead = knots[i].Tail;
        }
        foreach (var line in input)
        {
            ProcessMove(line, knots, tailVisitedPositions);
            // Only use the grid print on example, it throws IndexOutOfBounds on the puzzle because the MaxGridSize is too low.
            // PrintKnots(knots);
        }

        return tailVisitedPositions.Count;
    }

    static int MaxGridSize = 30;
    public static void Day9()
    {
        var input9 = Utils.ReadInput("./2022/9.txt");
        
        Console.WriteLine("Output 9.1: {0}", CalculateTailVisitedPositions(1, input9));
        Console.WriteLine("Output 9.2: {0}", CalculateTailVisitedPositions(10, input9));
    }

    #endregion

    #region Day 10

    private class CrtRow
    {
        public int MinCycleNumber;
        public int MaxCycleNumber;

        public char[] PixelsRow = Enumerable.Repeat(' ', 40).ToArray();

        public CrtRow(int minCycleNumber, int maxCycleNumber)
        {
            MinCycleNumber = minCycleNumber;
            MaxCycleNumber = maxCycleNumber;
        }

        public void DrawPixel(int pixelIndex, char charToDraw)
        {
            PixelsRow[pixelIndex] = charToDraw;
        }

        public override string ToString()
        {
            return string.Join("", PixelsRow);
        }
    }

    public static void Day10()
    {
        void CheckIfCycleWasReached(List<int> cyclesPositions, List<int> cyclesStrengths, int currentCycle, int currentRegisterXValue)
        {
            if (cyclesPositions.Count == 0) return;

            var minCycleRequired = cyclesPositions[0];
            if (currentCycle == minCycleRequired)
            {
                var cycleStrength = minCycleRequired * currentRegisterXValue;
                // Console.WriteLine("Cycle {0}, RegisterX: {1}, Strength: {2}", minCycleRequired, currentRegisterXValue, cycleStrength);
                cyclesPositions.RemoveAt(0);
                cyclesStrengths.Add(cycleStrength);
            }
        }

        CrtRow? GetCorrectCrtRowBasedOnCycleNumber(CrtRow[] crtRows, int cycleNumber)
        {
            foreach (var crtRow in crtRows)
            {
                if (crtRow.MinCycleNumber <= cycleNumber && crtRow.MaxCycleNumber >= cycleNumber)
                    return crtRow;
            }
            return null;
        }

        char GetCorrectCharBasedOnPosition(int position, (int Min, int Max) spritePosition)
        {
            if (position >= spritePosition.Min && position <= spritePosition.Max)
                return '#';
            return '.';
        }

        var input10 = Utils.ReadInput("./2022/10.txt");
        var registerX = 1;
        var cycleNumber = 0;

        var cyclesPositions = new List<int>() { 20, 60, 100, 140, 180, 220 };
        var cyclesStrengths = new List<int>();

        (int MinSpritePos, int MaxSpritePos) spritePos = (1, 3);

        var crtRows = new CrtRow[6];
        for (var i = 0; i < 6; i++)
        {
            crtRows[i] = new CrtRow((i * 40) + 1, (i * 40) + 40);
            // Console.WriteLine("CRT {0}: {1}-{2}", i, crtRows[i].MinCycleNumber, crtRows[i].MaxCycleNumber);
        }

        var checkForCycleStrengths = true;
        foreach(var line in input10)
        {
            int position;
            var parts = line.Split(" ");
            switch (parts[0])
            {
                case "noop":
                    cycleNumber++;
                    position = (cycleNumber % 40) == 0 ? 40 : (cycleNumber % 40);
                    GetCorrectCrtRowBasedOnCycleNumber(crtRows, cycleNumber)?.DrawPixel(position - 1, GetCorrectCharBasedOnPosition(position, spritePos));
                    if (checkForCycleStrengths)
                        CheckIfCycleWasReached(cyclesPositions, cyclesStrengths, cycleNumber, registerX);
                    break;
                case "addx":
                    // first cycle of addx
                    cycleNumber++;
                    position = (cycleNumber % 40) == 0 ? 40 : (cycleNumber % 40);
                    GetCorrectCrtRowBasedOnCycleNumber(crtRows, cycleNumber)?.DrawPixel(position - 1, GetCorrectCharBasedOnPosition(position, spritePos));
                    if (checkForCycleStrengths)
                        CheckIfCycleWasReached(cyclesPositions, cyclesStrengths, cycleNumber, registerX);
                    
                    // second cycle of addx
                    cycleNumber++;
                    position = (cycleNumber % 40) == 0 ? 40 : (cycleNumber % 40);
                    GetCorrectCrtRowBasedOnCycleNumber(crtRows, cycleNumber)?.DrawPixel(position - 1, GetCorrectCharBasedOnPosition(position, spritePos));
                    if (checkForCycleStrengths)
                        CheckIfCycleWasReached(cyclesPositions, cyclesStrengths, cycleNumber, registerX);
                    
                    var addXValue = int.Parse(parts[1]);
                    registerX += addXValue;

                    spritePos.MinSpritePos += addXValue;
                    spritePos.MaxSpritePos += addXValue;
                    break;
            }

            if (cyclesPositions.Count == 0)
            {
                checkForCycleStrengths = false;
            }
        }

        Console.WriteLine("Output 10.1: {0}", cyclesStrengths.Aggregate((first, second) => first + second));

        var strBuilder = new StringBuilder();
        foreach(var crtRow in crtRows)
        {
            strBuilder.AppendLine(crtRow.ToString());
        }

        Console.WriteLine("Output 10.2:\n\n{0}", strBuilder.ToString());
    }

    #endregion

    #region Day 11

    private enum OperatorEnum
    {
        Sum,
        Sub,
        Mul,
        Div
    }

    private class Operation
    {
        public OperatorEnum Operator;
        public string LeftSide;
        public string RightSide;

        public Operation(string leftSide, string rightSide, OperatorEnum operatorEnum)
        {
            LeftSide = leftSide;
            RightSide = rightSide;
            Operator = operatorEnum;
        }

        public BigInteger Calculate(string previous)
        {
            string left = !string.IsNullOrWhiteSpace(LeftSide) ? LeftSide : previous;
            string right = !string.IsNullOrWhiteSpace(RightSide) ? RightSide : previous;
            switch (Operator)
            {
                case OperatorEnum.Sum:
                    return Sum(left, right);
                //case OperatorEnum.Sub:
                //    return Sub(left, right);
                case OperatorEnum.Mul:
                    return Mul(left, right);
                //case OperatorEnum.Div:
                //    return Div(left, right);
                default:
                    return 0;
            }
        }

        private string Sum(string left, string right)
        {
            int len1 = left.Length;
            int len2 = right.Length;
            if (len1 == 0) return right;
            if (len2 == 0) return left;

            // the length of the result will be at least the length of the larger number plus 1
            int resultLength = len1 > len2 ? len1 + 1 : len2 + 1;
            int[] result = new int[resultLength];

            int carry = 0;
            for (var i_n1 = len1 - 1; i_n1 >= 0; i_n1--)
            {
                if (i_n1 > len2 - 1) 
            }
        }
        private BigInteger Sub(BigInteger left, BigInteger right) => left - right;
        private string Mul(string left, string right)
        {
            int len1 = left.Length;
            int len2 = right.Length;
            if (len1 == 0 || len2 == 0)
                return "0";

            // will keep the result number in vector
            // in reverse order
            int[] result = new int[len1 + len2];

            // Below two indexes are used to
            // find positions in result.
            int i_n1 = 0;
            int i;

            // Go from right to left in num1
            for (i = len1 - 1; i >= 0; i--)
            {
                int carry = 0;
                int n1 = left[i] - '0';

                // To shift position to left after every
                // multipliccharAtion of a digit in num2
                var i_n2 = 0;

                // Go from right to left in num2            
                for (int j = len2 - 1; j >= 0; j--)
                {
                    // Take current digit of second number
                    int n2 = right[j] - '0';

                    // Multiply with current digit of first number
                    // and add result to previously stored result
                    // charAt current position.
                    int sum = n1 * n2 + result[i_n1 + i_n2] + carry;

                    // Carry for next itercharAtion
                    carry = sum / 10;

                    // Store result
                    result[i_n1 + i_n2] = sum % 10;

                    i_n2++;
                }

                // store carry in next cell
                if (carry > 0)
                    result[i_n1 + i_n2] += carry;

                // To shift position to left after every
                // multipliccharAtion of a digit in num1.
                i_n1++;
            }

            // ignore '0's from the right
            i = result.Length - 1;
            while (i >= 0 && result[i] == 0)
                i--;

            // If all were '0's - means either both
            // or one of num1 or num2 were '0'
            if (i == -1)
                return "0";

            // genercharAte the result String
            String s = "";

            while (i >= 0)
                s += (result[i--]);

            return s;
        }

        private BigInteger Div(BigInteger left, BigInteger right) => left / right;
    }

    private class Monkey
    {
        public int Number;
        public List<BigInteger> Items;
        public Operation Operation { get; set; }
        public int Test { get; set; }
        public int ThrowToMonkeyTrue { get; set; }
        public int ThrowToMonkeyFalse { get; set; }
        public int NumberOfInspectedItems;

        public Monkey(int number)
        {
            Number = number;
            Items = new List<BigInteger>();
            NumberOfInspectedItems = 0;
        }

        public void AddItem(BigInteger itemNumber)
        {
            Items.Add(itemNumber);
        }

        public void ThrowItem(BigInteger oldItemWorryLevel, BigInteger newItemWorryLevel, Monkey to)
        {
            Items.Remove(oldItemWorryLevel);
            to.AddItem(newItemWorryLevel);
            // Console.WriteLine("Monkey {0} threw item {1} to monkey {2} with new worry level of {3}.", Number, oldItemWorryLevel, to.Number, newItemWorryLevel);
        }

        public void InspectItem(List<Monkey> monkeys, bool ignoreItemDamage)
        {
            if (Items.Count == 0) return;
            NumberOfInspectedItems++;
            var itemWorryLevel = Items.First();
            var operationWorryLevel = Operation.Calculate(itemWorryLevel);
            var monkeyGetsBoredWorryLevel = ignoreItemDamage ? operationWorryLevel : operationWorryLevel / 3;
            if (monkeyGetsBoredWorryLevel % Test == 0)
                ThrowItem(itemWorryLevel, monkeyGetsBoredWorryLevel, monkeys[ThrowToMonkeyTrue]);
            else
                ThrowItem(itemWorryLevel, monkeyGetsBoredWorryLevel, monkeys[ThrowToMonkeyFalse]);
        }
    }

    private static List<Monkey> SetupMonkeys(string[] input)
    {
        var monkeys = new List<Monkey>();
        Monkey currentMonkey = null;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split(':');
            if (string.IsNullOrWhiteSpace(parts[1]))
            {
                var monkeyNumber = parts[0][parts[0].Length - 1] - '0';
                currentMonkey = new Monkey(monkeyNumber);
            }
            else
            {
                switch (parts[0])
                {
                    case "  Starting items":
                        parts[1].Split(',').ToList().ForEach(p => currentMonkey?.AddItem(int.Parse(p.Trim())));
                        break;
                    case "  Operation":
                        var operationElements = parts[1].TrimStart().Split(' ');
                        var left = operationElements[2] == "old" ? default(int?) : int.Parse(operationElements[2]);
                        var right = operationElements[4] == "old" ? default(int?) : int.Parse(operationElements[4]);
                        OperatorEnum op = OperatorEnum.Sum;
                        switch (operationElements[3])
                        {
                            case "+":
                                op = OperatorEnum.Sum;
                                break;
                            case "-":
                                op = OperatorEnum.Sub;
                                break;
                            case "*":
                                op = OperatorEnum.Mul;
                                break;
                            case "/":
                                op = OperatorEnum.Div;
                                break;
                        }
                        currentMonkey.Operation = new Operation(left, right, op);
                        break;
                    case "  Test":
                        var testElements = parts[1].TrimStart().Split(' ');
                        currentMonkey.Test = int.Parse(testElements[2]);
                        break;
                    case "    If true":
                        currentMonkey.ThrowToMonkeyTrue = parts[1][parts[1].Length - 1] - '0';
                        break;
                    case "    If false":
                        currentMonkey.ThrowToMonkeyFalse = parts[1][parts[1].Length - 1] - '0';
                        monkeys.Add(currentMonkey);
                        break;
                }
            }
        }

        return monkeys;
    }

    private static void PrintMonkeysFinalResults(IList<Monkey> monkeys)
    {
        foreach(var monkey in monkeys)
        {
            Console.WriteLine("Monkey {0} inspected items {1} times.", monkey.Number, monkey.NumberOfInspectedItems);
        }
    }

    public static void Day11()
    {
        var input11 = Utils.ReadInput("./2022/11.txt");
        var monkeys = SetupMonkeys(input11);

        var rounds = 0;
        while (rounds < 20)
        {
            foreach(var monkey in monkeys)
            {
                var itemsPossessedByMonkey = monkey.Items.Count;
                for (var i = 0; i < itemsPossessedByMonkey; i++)
                    monkey.InspectItem(monkeys, false);
            }
            rounds++;
        }

        // PrintMonkeysFinalResults(monkeys);
        Console.WriteLine("Output 11.1: {0}", monkeys.OrderByDescending(m => m.NumberOfInspectedItems).Take(2).Select(m => m.NumberOfInspectedItems).Aggregate((a, b) => a * b));

        monkeys = SetupMonkeys(input11);
        rounds = 0;
        while (rounds < 10000)
        {
            foreach (var monkey in monkeys)
            {
                var itemsPossessedByMonkey = monkey.Items.Count;
                for (var i = 0; i < itemsPossessedByMonkey; i++)
                    monkey.InspectItem(monkeys, true);
            }
            rounds++;
            
            if (rounds % 100 == 0)
                Console.WriteLine("Round {0}", rounds);
            //PrintMonkeysFinalResults(monkeys);
        }

        Console.WriteLine("Output 11.2: {0}", monkeys.OrderByDescending(m => m.NumberOfInspectedItems).Take(2).Select(m => m.NumberOfInspectedItems).Aggregate((a, b) => a * b));
    }

    #endregion
}