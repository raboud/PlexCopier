using System;
using System.Collections.Generic;

namespace MediaLib
{
    public class Cutlist : List<Cut>
    {
        public override string ToString()
        {
            List<string> ss = new List<string>();
            foreach (Cut c in this)
            {
                ss.Add($"{TimeSpan.FromSeconds(c.Start)} - {TimeSpan.FromSeconds(c.End)}");
            }
            return string.Join(Environment.NewLine, ss);
        }
    }


}
