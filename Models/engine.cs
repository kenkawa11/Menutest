using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Menutest.ViewModels.Notification;


namespace PronunDLWPF
{
    public class ModelStatus: NotificationObject
    {
        private string status;
        private int progress;
        private string rpt;
        public string Status
        {
            get { return this.status; }
            set { SetProperty(ref this.status, value,"status"); }
        }

        public int Progress
        {
            get { return this.progress; }
            set { SetProperty(ref this.progress, value,"progress"); }
        }
        public string Rpt
        {
            get { return this.rpt; }
            set { SetProperty(ref this.rpt, value,"rpt"); }
        }
    }
    public class PronounceDownloader
    {
        private string rfn;
        private string dir;
        public int DownloadNum { get; set; }
        public int TargetNum { get; set; }
        public int TargetSymbolNum { get; set; }
        public int SymbolNum { get; set; }
        public int Count { get;set; }


        private readonly List<List<string>> fdata = new List<List<string>>();

        public ModelStatus Ret = new ModelStatus();




        private void ReadData()
        {
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
        }

        private void WriteData()
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



        public async void TreatData(string a, string b)
        {
            //await Task.Delay(10);
            Ret.Progress = 0;
            Ret.Status = " ";
            Ret.Rpt = " ";
            rfn = a;
            dir = b;
            DownloadNum = 0;
            TargetNum = 0;
            TargetSymbolNum = 0;
            SymbolNum = 0;
            fdata.Clear();


            ReadData();
            Ret.Status = "Reading";
            Count = fdata.Count;



            Ret.Status = "Processing";
            int treatNum = 0;
            foreach (var values in fdata)
            {
                /*TreatSentence(values);:*/
                TreatMp3(values);
                TreatSym(values);
                treatNum++;
                Ret.Progress = treatNum * 100 / Count;
                Ret.Rpt = $"mp3:{DownloadNum}/{TargetNum},Symbol:{SymbolNum}/{TargetSymbolNum}";
                if (Ret.Status=="Canceled")
                {
                    break;
                }
                await Task.Delay(1000);
            }

            WriteData();

            var mp3 = $"{DownloadNum} mp3 files in {TargetNum} were downloaded \n";
            var sym = $"{SymbolNum} symbols in {TargetSymbolNum} were gotten"; 

            if(DownloadNum<2)
            {
                mp3 = $"{DownloadNum} mp3 file in {TargetNum} was downloaded \n"; ;
            }
            if (SymbolNum<2)
            {
                sym = $"{SymbolNum} symbol in {TargetSymbolNum} was gotten";
            }

            Ret.Rpt = mp3 + sym;

            if (Ret.Status!="Canceled")
            {
                Ret.Status = "Completed";
            }


        }
        public void TreatMp3(List<string> line)
        {
            var target_word = line[2];
            target_word = target_word.Trim().Replace(" ", "+");
            line[0] = "TRKW-" + target_word;
            if (line[3] == "n")
            {
                TargetNum++;
                var outpath = dir + line[0];
                foreach (var dic in Dict_all.allmp3)
                {
                    if (dic.DownLoadMp3(target_word, outpath) != "0")
                    {
                        line[3] = "A";
                        DownloadNum++;
                        return;
                    }
                }
            }
            return;
        }

        public void TreatSentence(List<string> line)
        {
            var target_word = line[2];
            target_word = target_word.Trim().Replace(" ", "+");
            Dict_all.longman.GetEaxampleSentence(target_word);
        }

        public void TreatSym(List<string> line)
        {
            if (line[4] == "n")
            {
                TargetSymbolNum++;
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
                    SymbolNum++;
                }
            }
            return;

        }
    }
}
