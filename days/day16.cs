using System.Globalization;
using AOC.util;

namespace AOC.days;

internal class Day16 : Day
{
    protected override void SetSequence()
    {
        AddRun("Test 1", () => RunPart(1, "16_test01.txt"), 16);
        AddRun("Test 2", () => RunPart(1, "16_test02.txt"), 12);
        AddRun("Test 3", () => RunPart(1, "16_test03.txt"), 23);
        AddRun("Test 4", () => RunPart(1, "16_test04.txt"), 31);
        AddRun("Part 1", () => RunPart(1, "16.txt"));
        AddRun("Test 5", () => RunPart(2, "16_test05.txt"), 3);
        AddRun("Test 6", () => RunPart(2, "16_test06.txt"), 54);
        AddRun("Test 7", () => RunPart(2, "16_test07.txt"), 7);
        AddRun("Test 8", () => RunPart(2, "16_test08.txt"), 9);
        AddRun("Test 9", () => RunPart(2, "16_test09.txt"), 1);
        AddRun("Test 10", () => RunPart(2, "16_test10.txt"), 0);
        AddRun("Test 11", () => RunPart(2, "16_test11.txt"), 0);
        AddRun("Test 12", () => RunPart(2, "16_test12.txt"), 1);
        AddRun("Part 2", () => RunPart(2, "16.txt"));
    }

    public override long RunPart(int part, string inputName)
    {
        var line = GetListOfLines(inputName).Single();
        var bits = new Queue<char>(
            line.Select(x => int.Parse($"{x}", NumberStyles.HexNumber))
            .SelectMany(x => Convert.ToString(x, 2).PadLeft(4, '0'))
            );
        return Run(part, bits);
    }

    private long GetLong(Queue<char> bits, int size)
    {
        var word = String.Concat(bits.DequeueChunk(size));
        return Convert.ToInt64(word, 2);
    }

    private long GetLongBlocks5(Queue<char> bits)
    {
        long result = 0;
        bool notLast;
        do
        {
            notLast = bits.Dequeue() == '1';
            result = result * 16 + GetLong(bits, 4);
        } while (notLast);

        return result;
    }

    private long Run(int part, Queue<char> bits)
    {
        var version = GetLong(bits, 3);
        var type = GetLong(bits, 3);
        long total = 0;
        
        switch (type)
        {
            case 4:
                total = GetLongBlocks5(bits);
                if (part == 1)
                    total = 0;
                break;
            default:
                var lengthTypeId = bits.Dequeue();
                var subPackets = new List<long>();
                switch (lengthTypeId)
                {
                    case '0':
                        var packets = GetLong(bits, 15);
                        var subBits = new Queue<char>(bits.DequeueChunk(packets));
                        while (subBits.Count > 0)
                            subPackets.Add(Run(part, subBits));
                        break;
                    case '1':
                        var number = GetLong(bits, 11);
                        for (var i = 0; i < number; i++)
                            subPackets.Add(Run(part, bits));
                        break;
                }

                total += part == 1
                    ? subPackets.Sum()
                    : type switch
                    {
                        0 => subPackets.Sum(),
                        1 => subPackets.Aggregate((aggregate, next) => aggregate * next),
                        2 => subPackets.Min(),
                        3 => subPackets.Max(),
                        5 => subPackets[0] > subPackets[1] ? 1 : 0,
                        6 => subPackets[0] < subPackets[1] ? 1 : 0,
                        7 => subPackets[0] == subPackets[1] ? 1 : 0,
                        _ => total
                    };
                break;
        }
        return part == 1 ? version + total : total;
    }
}