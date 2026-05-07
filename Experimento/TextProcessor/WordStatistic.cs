using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextProcessorTools
{
    public class WordStatistic
    {
        public string Word { get; set; } = "";
        public int Count { get; set; } 
        [DisplayName("Relative Frecuency")]
        public float RelativeFrecuency { get; set; }
    }
}
