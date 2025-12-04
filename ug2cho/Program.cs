using System.Text.RegularExpressions;

namespace ug2cho;
class Program
{



    static bool LooksLikeAChord(string str)
    {
        // root, potential sharp/flag, modifier
        string chordRegex=@"[A-G](#|b)?(m|min|7|maj7|-7)?(/[A-G])?";
        return Regex.Match(str, chordRegex, RegexOptions.Compiled).Success;
    }

    static void TestChordRegex()
    {
        string[] chords = { "A", "G", "A#", "Gb", "Am", "Gmin", "Amaj7", "G-7", "A/B", "Abmaj7/C" };
        string[] nonChords = { "Q", "A#b", "Ammin", "A/BC" };
        foreach(string currChord in chords)
        {
            if(!LooksLikeAChord(currChord))
            {
                Console.Error.WriteLine($"Valid chord not recognized: {currChord}");
            }
        }
        foreach(String nonChord in nonChords)
        {
            if(LooksLikeAChord(nonChord))
            {
                Console.Error.WriteLine($"Non chord recognized: {nonChord}");
            }
        }
    }

    static bool LooksLikeAChordLine(string line)
    {

        string[] toks = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        foreach(string currTok in toks)
        {
            if(!LooksLikeAChord(currTok))
            {
                return false;
            }
        }
        return toks.Length > 0;
    }

    static List<int> GetChordStarts(string line)
    {
        bool inChord = false;
        List<int> ret = new();
        for(int i=0; i < line.Length; ++i)
        {
            if(!char.IsWhiteSpace(line[i]))
            {
                if(!(inChord))
                {
                    ret.Add(i);
                    inChord=true;
                }
            }
            else
            {
                inChord=false;
            }
        }
        return ret;
    }

    public static void TestChordStarts()
    {
        string testMe = "                            Em          A";
        var starts = GetChordStarts(testMe);
        if(starts.Count != 2 || starts[0] != 28 || starts[1] != 40)
        {
            Console.WriteLine("Got bad starts, expected 28 and 40");
        }
    }

    public static string Splice(string original, string insertMe, int offset)
    {
        string prefix = original.Substring(0, offset);
        string suffix = original.Substring(offset);
        return $"{prefix}{insertMe}{suffix}";
    }


    // Start at the back of the string so the forward-computed starts remain valid
    public static string CombineWordsAndChords(string wordLine, List<int> chordStarts, List<string> chords, int line)
    {
        string ret = wordLine;
        chordStarts.Reverse();
        chords.Reverse();
        //int[] revStarts = chordStarts..Reverse();
        //string[] revChords = chords.Reverse();
        if(chordStarts.Count != chords.Count)
        {
            throw new Exception($"chord starts and toks must have same length at line {line}");
        }

        for(int i=0; i < chordStarts.Count; ++i)
        {

            int currStart = chordStarts[i];

            if(currStart < 0)
            {
                throw new Exception($"chord offset > length of line at line {line}");
            }
            else if (wordLine.Length < currStart || currStart < 0)
            {
                ret = ret + $"[{chords[i]}] ";
            }
            else
            {
                ret = Splice(ret, $"[{chords[i]}]", chordStarts[i]);
            }
        }

        chordStarts.Reverse();
        chords.Reverse();

        return ret;
    }


    static void Main(string[] args)
    {
        TestChordRegex();
        TestChordStarts();
        List<int> currChordStarts=null;
        List<string> currChords=null;

        string[] lines = File.ReadAllLines(args[0]).Where(x => !(x.StartsWith("[") && x.EndsWith("]"))).ToArray();
        for(int i=0; i < lines.Length; ++i)
        {
            string currLine = lines[i];
            if(LooksLikeAChordLine(currLine))
            {
                if(currChordStarts != null)
                {
                    throw new Exception($"Got two chord lines in a row at line {i}");
                }

                string[] toks = currLine.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var starts = GetChordStarts(currLine);
                if(starts.Count != toks.Length)
                {
                    throw new Exception($"Chord and tok count mismatch at line {currLine}");
                }
                currChordStarts = starts;
                currChords = new List<string>(toks);
            }
            // If we have chords to write out, write them and reset. 
            else if(!string.IsNullOrWhiteSpace(currLine))
            {
                if(currChordStarts != null)
                {
                    Console.WriteLine(CombineWordsAndChords(currLine, currChordStarts, currChords, i));
                    currChordStarts = null;
                    currChords = null;
                }
                else
                {
                    Console.WriteLine(currLine);
                }
            }
        }
    }
}
