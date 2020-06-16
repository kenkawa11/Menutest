using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace PronunDLWPF
{
    public class FileData
    {
        private readonly string rfn;
        private readonly string dir;
        public int Count { get; set; }
        public int Progress { get; set; }
        private readonly List<List<string>> fdata = new List<List<string>>();
        public FileData(string a, string b)
        {
            rfn=a;
            dir=b;
            string line;
            _ = new List<string>();
            using StreamReader sr = new StreamReader(rfn);
            while (!sr.EndOfStream) //csvファイルをリストに読み込む
            {
                // CSVファイルの一行を読み込む
                line = sr.ReadLine();
                // 読み込んだ一行をカンマ毎に分けて配列に格納する
                List<string> stringList = line.Split(',').ToList();
                fdata.Add(stringList);
            }
            Count = fdata.Count;
        }

        public void WriteData()
        {
            string line = null;
            using StreamWriter file = new StreamWriter(rfn, false, Encoding.UTF8);
            foreach (var v in fdata)
            {
                foreach (var u in v)
                {
                    line += u;
                    line += ",";
                }
                line = line[0..^1];
                //line = line.Substring(0, line.Length - 1);
                file.WriteLine(line);
                line = null;
            }
        }

        public async Task<bool> TreatData()
        {
            Progress = 0;
            foreach (var values in fdata)
            {
                TreatMp3(values);
                TreatSym(values);
                Progress++;
                await Task.Delay(1000);
            }
            return true;

        }
        public void TreatMp3(List<string> line)
        {
            var target_word = line[2];
            target_word = target_word.Trim().Replace(" ", "+");
            line[0] = "TRKW-" + target_word;
            if (line[3] == "n")
            {
                var outpath = dir + line[0];
                foreach (var dic in Dict_all.allmp3)
                {
                    if (dic.DownLoadMp3(target_word, outpath) != "0")
                    {
                        line[3] = "A";
                        return;
                    }
                }
            }
            return;
        }

        public void TreatSym(List<string> line)
        {
            if (line[4] == "n")
            {
                var target_word = line[2];
                target_word = target_word.Trim().Replace(" ", "+");
                target_word = "search?q=" + target_word;
                var temp = Dict_all.eijiro.DownLoadSymbol(target_word);
                if (temp == "0")
                {
                    return;

                }
                else
                {
                    line[4] = temp;
                }
            }
            return;

        }
    }
}
