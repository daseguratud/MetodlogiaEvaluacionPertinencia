using LifaeTools;

namespace TextProcessorTools
{
    internal class TranslationCache :ICacheable
    {
        public string OriginalText { get; set; } = "";
        public string TranslatedText { get; set; } = "";
        public void FromStringArray(string[] textFields)
        {
            OriginalText= textFields[0];
            TranslatedText= textFields[1];
        }

        public string[] ToStringArray()
        {
            return new string[] { 
                OriginalText, TranslatedText
            };
        }
    }
}
