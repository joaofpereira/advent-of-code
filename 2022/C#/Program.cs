using AdventOfCode;
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
        Day10();
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

    public static void Day10()
    {
        var input10 = Utils.ReadInput("./2022/10.txt");
    }

    #endregion
}