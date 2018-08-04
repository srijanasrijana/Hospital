using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public class OptionalField
    {
        public int NoOfFields { get; set; }
        public string First { get; set; }
        public string Second { get; set; }
        public string Third { get; set; }
        public string Fourth { get; set; }
        public string Fifth { get; set; }
        public bool IsField1Required { get; set; }
        public bool IsField2Required { get; set; }
        public bool IsField3Required { get; set; }
        public bool IsField4Required { get; set; }
        public bool IsField5Required { get; set; }
    }
}
