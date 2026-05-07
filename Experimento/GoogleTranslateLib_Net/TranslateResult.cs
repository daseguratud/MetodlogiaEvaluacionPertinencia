using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleTranslateLib
{
    public class TranslateResponse
    {
        public bool IsSuccess { get; set; }
        public Exception Exception { get; set; }
        public string MessageError { get; set; }
        public TranslateResult Result { get; set; }
        public string Response { get; internal set; }

        public override string ToString()
        {
            if (IsSuccess)
            {
                return string.Join(", ", Result.sentences);
            }
            return $"[ERROR] = {MessageError}";
        }
    }

    public class TranslateResult
    {
        [JsonProperty("confidence")]
        public float confidence { get; set; }

        [JsonProperty("dict")]
        public object[] dict { get; set; }

        [JsonProperty("ld_result")]
        public object ld_result { get; set; }

        [JsonProperty("sentences")]
        public IList<Sentence> sentences { get; set; }

        [JsonProperty("src")]
        public string src { get; set; }
    }

    public class Sentence
    {
        [JsonProperty("backend")]
        public int backend { get; set; }

        [JsonProperty("orig")]
        public string orig { get; set; }

        [JsonProperty("trans")]
        public string trans { get; set; }

        public override string ToString()
        {
            return $"{orig}: {trans}";
        }
    }
}
