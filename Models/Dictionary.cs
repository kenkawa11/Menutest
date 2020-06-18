using System.Text.RegularExpressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using AngleSharp;
using AngleSharp.Html.Parser;
using System.Linq;

namespace PronunDLWPF
{
    public static class Dict_all
    {
        private static readonly Ox oxford = new Ox();
        public static  Ldo longman = new Ldo();
        private static readonly Webl weblio = new Webl();
        public static Eiji eijiro = new Eiji();

        public static List<BaseDic> allmp3 = new List<BaseDic>
        {
            longman,oxford,weblio
        };
    }

    public class BaseDic
    {
        public static HttpClient client = new HttpClient();
        public string Url { get; set; }
        public string Ptn { get; set; }

        public virtual string DownLoadMp3(string w, string outpath)
        {
            var bodyUrl = Url + w;
            var url_mp3 = Get_body(bodyUrl, Ptn).Result;
            if (url_mp3 == "0")
            {
                return "0";
            }
            Get_mp3(url_mp3, outpath);
            return "1";
        }
        public static async Task<string>GetHtml(string url)
        {
            try
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                return await client.GetStringAsync(url).ConfigureAwait(false);

                //return client.GetStringAsync(url);

            }
            catch (HttpRequestException)
            {
                return "0";

            }
        }

        public static async Task<string> Get_body(string url, string ptn)//マッチパターンから直接mp3を取り出せう場合はこれで良い
        {
            try
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                var html = await client.GetStringAsync(url).ConfigureAwait(false);
                var reg = new Regex(ptn);
                var m = reg.Match(html);
                if (m.Success)
                {
                    return m.Groups[0].Value;
                }
                return "0";
            }
            catch (HttpRequestException)
            {
                return "0";
            }
        }
        public static async void Get_mp3(string url_mp3, string outpath)
        {
            HttpResponseMessage res = await client.GetAsync(url_mp3);
            var outputPath = outpath + ".mp3";
            using var fileStream = File.Create(outputPath);
            using var httpStream = await res.Content.ReadAsStreamAsync();
            httpStream.CopyTo(fileStream);
            fileStream.Flush();
        }
        public string DownLoadSymbol(string w)
        {
            var bodyUrl = Url + w;
            var url_symbol = Get_body(bodyUrl, Ptn).Result;
            if (url_symbol == "0")
            {
                return "0";
            }
            var pos = url_symbol.IndexOf("】");

            return url_symbol.Substring(pos + 1, url_symbol.Length - pos - 5);
        }
    }


    public class Ox : BaseDic
    {
        public Ox()
        {
            Url = "https://www.oxfordlearnersdictionaries.com/definition/english/";
            Ptn = "https://www.oxfordlearnersdictionaries.com/media/english/us_pron/.+?mp3";
        }
    }

    public class Ldo : BaseDic
    {
        public Ldo()
        {
            Url = "https://www.ldoceonline.com/jp/dictionary/";
            //Ptn = "https://d27ucmmhxk51xv.cloudfront.net/media/english/exaProns/.+?mp3";

            Ptn = "https://.+/ameProns/.+?mp3";
        }

        public Ldo(string ptn1)
        {
            Url = "https://www.ldoceonline.com/jp/dictionary/";
            Ptn = ptn1;
            //Ptn = "https://d27ucmmhxk51xv.cloudfront.net/media/english/exaProns/.+?mp3";
        }

        public string GetEaxampleSentence(string w)
        {
            var config = Configuration.Default;
            var bodyUrl = Url + w;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();
            var html = GetHtml(bodyUrl);
            var doc = parser.ParseDocument(html.Result);
            var elemnts = doc.GetElementsByClassName("EXAMPLE");



            return "1";

        }











    }

    public class Webl : BaseDic
    {
        public Webl()
        {
            Url = "https://ejje.weblio.jp/content/";
            Ptn = "https://weblio.hs.llnwd.net/.+?mp3\"";
        }
        public override string DownLoadMp3(string w, string outpath)
        {
            var bodyUrl = Url + w;
            var url_mp3 = Get_body(bodyUrl, Ptn).Result;
            if (url_mp3 == "0")
            {
                return "0";
            }
            url_mp3 = url_mp3[0..^1];
            //url_mp3 = url_mp3.Substring(0, url_mp3.Length - 1);
            Get_mp3(url_mp3, outpath);
            return "1";
        }
    }

    public class Eiji : BaseDic
    {
        public Eiji()
        {
            Url = "https://eow.alc.co.jp/";
            Ptn = "【発音】.+?【カナ】|【発音.+?】.+?【カナ】";
        }
    }
}
