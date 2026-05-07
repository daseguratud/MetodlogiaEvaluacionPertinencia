using System.Drawing;
using System.Text;
using TextProcessorTools;
namespace Experimento
{
    internal class Program
    {
        private static string WorkingDirectory { get; set; }
        private static string rawTargetCorpusFile = "rawTargetCorpus.txt";
        private static string rawReferenceCorpusFile = "rawReferenceCorpus.txt";

        private static string targetCorpusFile = "targetCorpus.txt";
        private static string referenceCorpusFile = "referenceCorpus.txt";
        
        private static string normalizationRulesFile = "_normalizationRules.txt";
        private static string stopWordsFile = "_stopwords.csv";

        private static string crossTermsFile = "crossTerms.csv";
        static void Main(string[] args)
        {
            WorkingDirectory = Directory.GetCurrentDirectory()
                .Replace(@"Experimento\Experimento\bin\Debug\net8.0-windows", "");
            try
            {
                ConsoleUI.ShowTitle("Start process");
                //Stage2();
                //Stage3();
                //Stage4();
                Stage5();
                ConsoleUI.ShowTitle("End process");
            }
            catch (Exception ex) { 
                ConsoleUI.ShowError(ex);
            }
            ConsoleUI.WaitKey();
        }
        static void Stage2()
        {
            ConsoleUI.ShowStageTitle("Stage 2. Target corpus contruction");
            string stagePath = Path.Combine(WorkingDirectory, "Etapa_02");
            string[] syllabusFiles = Directory.GetFiles(stagePath);
            StringBuilder targetCorpusContent = new StringBuilder();
            foreach (string file in syllabusFiles)
            {
                string fileName = Path.GetFileName(file);
                if (fileName == rawTargetCorpusFile) continue;
                ConsoleUI.ShowMessage($"Reading {fileName}");
                string content = File.ReadAllText(file)+ '\n';
                targetCorpusContent.AppendLine(content);
            }
            ConsoleUI.ShowMessage($"Saving {rawTargetCorpusFile}");
            File.WriteAllText(Path.Combine(stagePath,rawTargetCorpusFile),targetCorpusContent.ToString());
            ConsoleUI.ShowStageTitle("Stage 2. Ended");
        }
        static void Stage3()
        {
            ConsoleUI.ShowStageTitle("Stage 3. Reference corpus contruction");
            string stagePath = Path.Combine(WorkingDirectory, "Etapa_03");
            string[] referenceFiles = Directory.GetFiles(stagePath);
            StringBuilder referenceCorpusContent = new StringBuilder();
            foreach (string file in referenceFiles)
            {
                string fileName = Path.GetFileName(file);
                if (fileName == rawReferenceCorpusFile) continue;
                ConsoleUI.ShowMessage($"Reading {fileName}");
                string content = File.ReadAllText(file) ;
                referenceCorpusContent.AppendLine(content);
            }
            ConsoleUI.ShowMessage($"Saving {rawReferenceCorpusFile}");
            File.WriteAllText(Path.Combine(stagePath, rawReferenceCorpusFile), referenceCorpusContent.ToString());
            ConsoleUI.ShowStageTitle("Stage 3. Ended");
        }
        static void Stage4()
        {
            ConsoleUI.ShowStageTitle("Stage 4. Linguistic processing");
            string targetPath = Path.Combine(WorkingDirectory, "Etapa_02");
            string referencePath = Path.Combine(WorkingDirectory, "Etapa_03");
            string stagePath = Path.Combine(WorkingDirectory, "Etapa_04");

            string targetRawCorpusFile = Path.Combine(targetPath, rawTargetCorpusFile);
            string referenceRawCorpusFile = Path.Combine(referencePath, rawReferenceCorpusFile);

            string targetCorpus_File = Path.Combine(stagePath, targetCorpusFile);
            string referenceCorpus_File = Path.Combine(stagePath, referenceCorpusFile);
            
            ConsoleUI.ShowMessage("Processing target corpus...");
            LinguisticProcessing(targetRawCorpusFile, targetCorpus_File,stagePath);
            
            ConsoleUI.ShowMessage("Processing reference corpus...");
            LinguisticProcessing(referenceRawCorpusFile, referenceCorpus_File,stagePath);

            ConsoleUI.ShowStageTitle("Stage 4. Ended");
        }
        static void LinguisticProcessing(string inputFile, string outputFile, string statagePath)
        {
            string rules_file = Path.Combine(statagePath, normalizationRulesFile);
            string stopword_file = Path.Combine(statagePath, stopWordsFile);
            var corpus = File.ReadAllText(inputFile);
            TextProcessor processor = new TextProcessor(corpus);

            //ConsoleUI.ShowMessage("Calculating statistics before processing...");
            //processor.SaveStatistics(outputFile.Replace(".txt", "_prev.csv"));
            //ConsoleUI.ShowMessage("Creating word cloud before processing...");
            //processor.CreateWordCloud(outputFile.Replace(".txt", "_prev.csv"), 100);

            ConsoleUI.ShowMessage("Converting to lower case...");
            processor.ToLowerCase();
            ConsoleUI.ShowMessage("Removing numbers...");
            processor.RemoveNumbers();
            ConsoleUI.ShowMessage("Removing punctuation marks...");
            processor.RemovePunctuationMarks();
            ConsoleUI.ShowMessage("Removing extra spaces...");
            processor.RemoveExtraSpaces();
            ConsoleUI.ShowMessage("Translating...");
            processor.TranslateToEnglish();
            ConsoleUI.ShowMessage("Applying Normalization rules...");
            processor.ApplyNormalizationRules(rules_file);
            ConsoleUI.ShowMessage("Removing stop words...");
            processor.RemoveStopWords(stopword_file);
            ConsoleUI.ShowMessage("Calculating statistics...");
            processor.SaveStatistics(outputFile.Replace(".txt", ".csv"));
            ConsoleUI.ShowMessage("Creating word cloud...");
            processor.CreateWordCloud(outputFile.Replace(".txt", ".csv"),100);
            File.WriteAllText(outputFile, processor.WorkText);
        }
        static void Stage5()
        {
            ConsoleUI.ShowStageTitle("Stage 5. Cross-term visualization and coverage index");
            string corpusPath = Path.Combine(WorkingDirectory, "Etapa_04");
            string stagePath = Path.Combine(WorkingDirectory, "Etapa_05");

            string targetCorpus_File = Path.Combine(corpusPath, targetCorpusFile.Replace(".txt", ".csv"));
            string referenceCorpus_File = Path.Combine(corpusPath, referenceCorpusFile.Replace(".txt", ".csv"));
            string crossTerms_File = Path.Combine(stagePath, crossTermsFile);
            ConsoleUI.ShowMessage("Creating cross-term visualization terms list...");
            TextProcessor.Compare(referenceCorpus_File, targetCorpus_File, crossTerms_File, 15);
            ConsoleUI.ShowMessage("Creating cross-term visualization terms chart...");
            TextProcessor.CreateCrossVisualization(crossTerms_File,true);
            CoverageIndexResult coverageIndexResult = new TextProcessor().CalculateCoverageIndex(crossTerms_File);
            ConsoleUI.ShowMessage($"Coverage Index: {coverageIndexResult.CoverageIndex} (B: {coverageIndexResult.B}, AB: {coverageIndexResult.AB})");
            ConsoleUI.ShowStageTitle("Stage 5. Ended");
        }     
    }
}
