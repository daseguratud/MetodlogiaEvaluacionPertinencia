using GoogleTranslateLib;
using LifaeTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using WordCloudSharp;
using gT=GoogleTranslateLib;

namespace TextProcessorTools
{
    public class TextProcessor
    {
        public string WorkText {  get; private set; }
        private Cache<TranslationCache> cacheTranslations {  get; set; }
        public TextProcessor(string WorkText="") { 
            this.WorkText = WorkText;
            cacheTranslations = new Cache<TranslationCache>("CacheTranslate.txt");
            cacheTranslations.Load();
        }
        private async Task<string[]> Translate(string text, string originalLanguage = "es", string targetLanguage = "en")
        {
            var cursorPosition = Console.GetCursorPosition();
            var lines = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>();
            var fromLanguaje = gT.Translate.GetLanguageFromISO(originalLanguage);
            var toLanguaje = gT.Translate.GetLanguageFromISO(targetLanguage);
            cacheTranslations.Load();
            foreach (var line in lines)
            {
                var caches = cacheTranslations.StoredCache.Where(t => t.OriginalText == line);
                if (caches.Count() > 0)
                {
                    result.Add(caches.ToArray()[0].TranslatedText);
                }
                else
                {
                    var res = 
                        await gT.Translate.TranslateText(line, fromLanguaje, toLanguaje);
                    var translation = JsonConvert.DeserializeObject<gT.TranslateResult>(res.Response);
                    var newcache = new TranslationCache()
                    {
                        OriginalText = line,
                        TranslatedText = translation.sentences[0].trans
                    };
                    cacheTranslations.Add(newcache);
                    result.Add(newcache.TranslatedText);
                }
                var progressText = $"Translated {result.Count} of {lines.Length}";
                Console.SetCursorPosition(cursorPosition.Left,cursorPosition.Top);
                Console.WriteLine(progressText );
            }
            return result.ToArray();
        }
        public void ToLowerCase()
        {
            WorkText = WorkText.ToLower();
        }
        public void RemoveNewLines()
        {
            WorkText = Regex.Replace(WorkText, @"\r\n|\r|\n", " ");
        }
        public void RemoveNumbers()
        {
            WorkText = Regex.Replace(WorkText, @"\d+", " ");
        }
        public void RemoveExtraSpaces()
        {
            WorkText = Regex.Replace(WorkText, @"[^\S\r\n]+", " ");
            WorkText = Regex.Replace(WorkText, "\r\n ", "\r\n");
            WorkText = Regex.Replace(WorkText, @"(\r\n){2,}", "\r\n");
        }
        public void RemoveSimbols()
        {
            WorkText = Regex.Replace(WorkText, @"[^\w\p{P}\s]", " ");
        }
        public void RemovePunctuationMarks()
        {
            WorkText = Regex.Replace(WorkText, @"\p{P}", " ");
        }
        public void TranslateToEnglish()
        {
            var result = Translate(WorkText,"es", "en").GetAwaiter().GetResult();
            WorkText = string.Join("\n", result);
        }
        public void ApplyNormalizationRules(string rulesFile)
        {
            var rules = NormalizationRule.LoadRulesFromFile(rulesFile);
            foreach (var rule in rules)
            {
                foreach (var variant in rule.LexicalVariants)
                {
                    WorkText = Regex.Replace(WorkText, variant, rule.CanonicalForm);
                }
            }            
        }
        public void RemoveStopWords(string stopwordsFile)
        {
            var stopWords = File.ReadAllLines(stopwordsFile);
            var lines = WorkText.Split('\n');
            StringBuilder result = new StringBuilder();
            foreach (var line in lines)
            {
                var filteredWords = line.Split(' ')
                    .Where(w => !stopWords.Contains(w));
                result.Append(string.Join(" ", filteredWords) + '\n');
            }
            result = result.Remove(result.Length - 1, 1);
            WorkText = result.ToString();
        }
        public void SaveStatistics(string fileName) {
            List<WordStatistic> wordsInfo = WorkText.Split(new char[] { ' ', '\n' })
                        .GroupBy(w => w)
                        .Select(ws => new WordStatistic() { Word = ws.Key.Trim(), Count = ws.Count() })
                        .Where(w => !string.IsNullOrWhiteSpace(w.Word))
                        .OrderByDescending(w => w.Count)
                        .ToList();
            if (wordsInfo.Count > 0)
            {
                int maxfrec = wordsInfo.Max(w => w.Count);
                foreach (var wordInfo in wordsInfo)
                {
                    wordInfo.RelativeFrecuency = (float)wordInfo.Count / maxfrec;
                }
            }
            var lines = wordsInfo
                .Select(wi => wi.Word.Replace(';',' ') + ";" + wi.Count + ";" + wi.RelativeFrecuency);
            File.WriteAllLines(fileName,lines);
        }
        public void CreateWordCloud(string fileName, int limit)
        {
            var wordsStatistics = LoadStatistics(fileName, limit).ToArray();
            var palabras = new List<string>();
            var frecuencias = new List<int>();
            var wc = new WordCloud(800, 600);
            palabras = wordsStatistics.Select(w => w.Word).ToList();
            frecuencias = wordsStatistics.Select(w => w.Count).ToList();
            wc.Draw(
                palabras,
                frecuencias
            ).Save(fileName.Replace(".csv", ".png"));
        }
        private static List<WordStatistic> LoadStatistics(string fileName, int limit)
        {
            var lines = File.ReadAllLines(fileName);
            List<WordStatistic> wordsStatistics =
                lines
                .Select(l => {
                    var parts = l.Split(';');
                    return new WordStatistic() {
                        Word = parts[0],
                        Count = int.Parse(parts[1]),
                        RelativeFrecuency = float.Parse(parts[2])
                    };
                }).ToList();
            if (limit > 0)
            {
                var distinctCounts = wordsStatistics
                    .Select(w => w.Count)
                    .Distinct()
                    .OrderByDescending(c => c)
                    .Take(limit)
                    .ToList();
                wordsStatistics =
                    wordsStatistics
                    .Where(w => distinctCounts.Contains(w.Count))
                    .OrderByDescending(w => w.Count)
                    .ToList();
            }
            return wordsStatistics;
        }
        public static void Compare(string referenceCorpusFile,string targetCorpus, string crossComparisionFile, int limit)
        {
            var reference = LoadStatistics(referenceCorpusFile, limit);
            var target = LoadStatistics(targetCorpus, limit);

            var full_reference = LoadStatistics(referenceCorpusFile, 0);
            var full_target = LoadStatistics(targetCorpus, 0);

            var comparison = new List<TermFrequencyComparison>();
            foreach (var word in reference)
            {
                var targetWord = full_target.FirstOrDefault(i => i.Word == word.Word);
                comparison.Add(new TermFrequencyComparison
                {
                    Term = word.Word,
                    ReferenceFrequency = word.RelativeFrecuency,
                    TargetFrequency = targetWord?.RelativeFrecuency ?? 0
                });
                if (targetWord?.Count is not null)
                {
                    var targetWordToRemove = target.FirstOrDefault(i => i.Word == word.Word);
                    target.Remove(targetWordToRemove);
                }
            }
            foreach (var word in target)
            {
                var referenceWord = full_reference.FirstOrDefault(i => i.Word == word.Word);
                comparison.Add(new TermFrequencyComparison
                {
                    Term = word.Word,
                    ReferenceFrequency = referenceWord?.RelativeFrecuency ?? 0,
                    TargetFrequency = word.RelativeFrecuency 
                });
            }
            comparison = comparison.OrderByDescending(c => c.ReferenceFrequency).ToList();
            var lines = comparison
                .Select(c => $"{c.Term};{c.TargetFrequency};{c.ReferenceFrequency}");
            File.WriteAllLines(crossComparisionFile, lines);
        }
        public static void CreateCrossVisualization(string crossComparisionFile, bool addStripLines)
        {
            var lines = File.ReadAllLines(crossComparisionFile);
            List<TermFrequencyComparison> comparisionList =
                lines.Select(l => {
                    var parts = l.Split(';');
                    return new TermFrequencyComparison() { Term = parts[0], TargetFrequency = float.Parse(parts[1]), ReferenceFrequency = float.Parse(parts[2]) };
                }).ToList();
            Chart chart = CreateChart(addStripLines);
            foreach (var comparision in comparisionList)
            {

                float fontsize =
                    (float)(40 * Math.Sqrt(
                        Math.Pow(comparision.TargetFrequency, 2) + 
                        Math.Pow(comparision.ReferenceFrequency, 2)
                        ));
                var dataPoint = new DataPoint()
                {
                    XValue = comparision.TargetFrequency,
                    YValues = [comparision.ReferenceFrequency],
                    Label = comparision.Term,
                    MarkerSize = 20,
                    MarkerStyle = MarkerStyle.Circle,
                    Font = new Font("Microsoft Sans Serif", fontsize),
                    MarkerColor = Color.FromArgb(
                        0,
                        255-(int)(comparision.TargetFrequency * 255),
                        255-(int)(comparision.ReferenceFrequency * 255)
                        ),
                    ToolTip = $"{comparision.Term}\nReference Frequency: {comparision.ReferenceFrequency:P2}\nTarget Frequency: {comparision.TargetFrequency:P2}",
                };
                chart.Series[0].Points.Add(dataPoint);
            }
            chart.SaveImage(crossComparisionFile.Replace(".csv", ".png"), ChartImageFormat.Png);
        }
        private static Chart CreateChart(bool addStripLines)
        {
            Chart chart = new Chart();
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.Maximum = 1.05;
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Interval = 0.1;
            chartArea.AxisX.Title = "Target Frequency";
            chartArea.AxisX.TitleFont = new Font("Microsoft Sans Serif", 30F);
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new Font("Microsoft Sans Serif", 20F);
            chartArea.AxisY.Maximum = 1.05;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.Interval = 0.1;
            chartArea.AxisY.Title = "Reference Frequency";
            chartArea.AxisY.TitleFont = new Font("Microsoft Sans Serif", 30F);
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisY.LabelStyle.Font = new Font("Microsoft Sans Serif", 20F);
            if(addStripLines)
            {
                chartArea.AxisY.StripLines.Add(CreateStripLine(0.33, Color.Red));
                chartArea.AxisY.StripLines.Add(CreateStripLine(0.66, Color.Orange));
                chartArea.AxisX.StripLines.Add(CreateStripLine(0.33, Color.Red));
                chartArea.AxisX.StripLines.Add(CreateStripLine(0.66, Color.Orange));
            }
            chart.ChartAreas.Add(chartArea);
            chart.Dock = DockStyle.Fill;
            chart.Location = new Point(3, 3);
            chart.Series.Add(new Series() {
                ChartArea = chartArea.Name,
                ChartType = SeriesChartType.Point,
            });
            chart.Size = new Size(1920, 1080);
            return chart;
        }
        // By ChatGPT
        private static StripLine CreateStripLine(double value, Color color)
        {
            var linea = new StripLine();
            linea.IntervalOffset = value; // valor donde quieres la línea
            linea.StripWidth = 0;      // 0 = línea, no banda
            linea.BorderColor = color;
            linea.BorderWidth = 2;
            linea.BorderDashStyle = ChartDashStyle.Dash;
            return linea;
        }
        public CoverageIndexResult CalculateCoverageIndex(string crossComparisionFile)
        {
            var lines = File.ReadAllLines(crossComparisionFile);
            List<TermFrequencyComparison> comparisionList =
                lines.Select(l => {
                    var parts = l.Split(';');
                    return new TermFrequencyComparison() { Term = parts[0], TargetFrequency = float.Parse(parts[1]), ReferenceFrequency = float.Parse(parts[2]) };
                }).ToList();
            int B = comparisionList.Count(c => c.ReferenceFrequency > 0);
            int AB = comparisionList.Count(c => c.TargetFrequency > 0 && c.ReferenceFrequency > 0);
            decimal ICT = ((decimal)AB) / ((decimal)B);
            return new CoverageIndexResult() { CoverageIndex = ICT, B = B, AB = AB };
        }
    }
}
