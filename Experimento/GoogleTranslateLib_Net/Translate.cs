using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoogleTranslateLib
{
    //Original: https://github.com/TiepHoangDev/GoogleTranslateLib
    //Just change from .netFramework to .Net core
    public class Translate
    {
        public static async Task<TranslateResponse> TranslateText(string input, Languages lang_input = Languages.en, Languages lang_output = Languages.vi)
        {
            var _res = new TranslateResponse();
            try
            {
                HttpClient client = new HttpClient();
                string sl = "auto";
                string tl = lang_output.ToString().Replace("_", "-");
                string hl = lang_input.ToString().Replace("_", "-");
                string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sl}&tl={tl}&hl={hl}&dt=t&dt=bd&dj=1&source=input&tk=501776.501776&q={input}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var text = await response.Content.ReadAsStringAsync();
                    var TranslateResult = JsonConvert.DeserializeObject<TranslateResult>(text);
                    _res.IsSuccess = true;
                    _res.Result = TranslateResult;
                    _res.Response = text;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GoogleTranslateLib.Translate [error] = " + ex);
                _res.IsSuccess = false;
                _res.Exception = ex;
                _res.MessageError = ex.Message;
            }
            return _res;
        }

        public static Dictionary<Languages, string> GetLanguages()
        {
            Dictionary<Languages, string> _res = new Dictionary<Languages, string>();
            var type = typeof(Languages);
            foreach (Languages item in Enum.GetValues(type))
            {
                string description = type.GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>().First().Description;
                _res.Add(item, description);
            }
            return _res;
        }
        static public Languages GetLanguageFromISO(string descripcion)
        {
            foreach (var field in typeof(Languages).GetFields())
            {
                if (field.Name==descripcion)
                {
                    return (Languages)field.GetValue(null);
                }
            }
            throw new Exception($"No language suported ({descripcion})");
        }
    }


}
