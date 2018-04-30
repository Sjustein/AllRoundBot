using System.Collections.Generic;

namespace AllRound.Resources.Datatypes
{
    public class Setting
    {
        public string token { get; set; }
        public ulong owner { get; set; }
        public List<ulong> log { get; set; }
        public string version { get; set; }
        public List<ulong> banned { get; set; }
    }
}
