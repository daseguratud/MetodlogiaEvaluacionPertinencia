using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifaeTools
{
    public class FileManager
    {
        private string _fullFileName { get; set; }
        public string FullFileName { get => _fullFileName; }
        public FileManager(string FullFileName)
        {
            _fullFileName = FullFileName;
            if (!File.Exists(_fullFileName)) {
                File.Create(_fullFileName).Close();
            }
        }
        public string ReadFile()
        {
            return File.ReadAllText(this.FullFileName);
        }
        public List<string> ReadLines()
        {
            return ReadFile().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        public void WriteFile(string text)
        {
            File.WriteAllText(this.FullFileName, text);
        }

        internal void WriteLines(List<string> lines)
        {
            File.WriteAllLines(this.FullFileName, lines);
        }
    }
}
