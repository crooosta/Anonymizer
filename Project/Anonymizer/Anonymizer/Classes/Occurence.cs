using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymizer.Classes
{
    public class Occurence
    {
        public double start { get; set; }
        public double end { get; set; }
        public List<WordClass> words { get; set; }
    }

    public class OccurenceNoWordList
    {
        public double start { get; set; }
        public double end { get; set; }

        public static List<OccurenceNoWordList> Convert(List<Occurence> originalList)
        {
            List<OccurenceNoWordList> result = new();
            foreach (Occurence o in originalList)
            {
                result.Add(new OccurenceNoWordList()
                {
                    start = o.start,
                    end = o.end
                });
            }
            return result;
        }

    }
}