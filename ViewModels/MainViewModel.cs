using System;
using PronunDLWPF;
using System.Threading.Tasks;
using System.Windows;

namespace Menutest.ViewModels
{

    public class MainViewModel: NotificationObject
    {
        private string status;
        private string progress;
        private int barProgress;
        private string fn;
        private string dir;
        private int isCont; // 1:キャンセル無し　0: キャンセル　ボタン　-1:Exitメニューからの場合
        private Boolean isActiveDone;
        public MainViewModel()
        {
            dir = @"C:\Users\naobaby\Desktop\test\";
            isCont = 1;
            isActiveDone = true;
        }

        public string Fn
        { 
            get { return this.fn; }
            set { SetProperty(ref this.fn, value); }
        }

        public string Status
        {
            get { return this.status; }
            set { SetProperty(ref this.status, value); }
        }

        public string Progress
        {
            get { return this.progress; }
            set { SetProperty(ref this.progress, value); }
        }

        public int BarProgress
        {
            get { return this.barProgress; }
            set { SetProperty(ref this.barProgress, value); }
        }

        public string Dir
        {
            get { return this.dir; }
            set { SetProperty(ref this.dir, value); }
        }


        public int IsCont
        {
            get { return this.isCont; }
            set { SetProperty(ref this.isCont, value); }
        }

        public bool IsActiveDone
        {
            get { return this.isActiveDone; }
            set { SetProperty(ref this.isActiveDone, value); }
        }



        private DelegateCommand _openFileCommand;
        private DelegateCommand _selectDirectory;
        private DelegateCommand _readprocess;
        private DelegateCommand _cancel;
        private DelegateCommand _exitApplication;

        public DelegateCommand ExitApplication
        { 
            get 
            {
                return this._exitApplication ?? (this._exitApplication = new DelegateCommand
                    (_ => { 
                        OnExit(); 
                    }));
            } 
        }

        private void OnExit()
        {
            App.Current.Shutdown();
        }




        public DelegateCommand SelectDirectory
        {
            get
            {
                return this._selectDirectory ?? (_selectDirectory = new DelegateCommand(
                    _ =>
                    {
                        this.DialogDirCallback = OnDirDialogCallback;

                    }));
            }
        }




        public DelegateCommand OpenFileCommand
        {
            get
            {
                return this._openFileCommand ?? (_openFileCommand = new DelegateCommand(
                    _ =>
                    {    
                        this.DialogCallback = OnDialogCallback;
                    }));
            }
        }

        private Action<bool, string> _dialogDirCallback;
        public Action<bool, string> DialogDirCallback
        {
            get { return this._dialogDirCallback; }
            private set { SetProperty(ref this._dialogDirCallback, value); }
        }

        private Action<bool, string> _dialogCallback;
        public Action<bool, string> DialogCallback
        {
            get{ return this._dialogCallback; }
            private set { SetProperty(ref this._dialogCallback, value); } 
        }

        private void OnDialogCallback(bool isOk, string filePath)
        {
            this.DialogCallback = null;

            Fn= filePath;
        }

        private void OnDirDialogCallback(bool isOk, string filePath)
        {
            this.DialogCallback = null;

            Dir = filePath;
        }



        public DelegateCommand Readprocess
        {
            get
            {
                if (this._readprocess == null)
                {
                    this._readprocess = new DelegateCommand(_ =>
                    {
                        if (fn != null)
                        {
                            this.IsActiveDone = false;
                            Fntreat();
                        }
                        else
                        {
                            MessageBox.Show("Select a target file !!!");
                        }
                    });
                }
                return this._readprocess;
            }
        }


        public DelegateCommand Cancel
        {
            get
            {
                if (this._cancel == null)
                {
                    this._cancel = new DelegateCommand(_ =>
                    {
                        isCont = 0;
                    });
                }
                return this._cancel;
            }
        }







        private async void Fntreat()
        {
            this.Status = "Processing";
            int num_treat;
            var LoadFileData = new FileData(fn, dir);
            var num_gross = LoadFileData.Count;

            var t1 = LoadFileData.TreatData();

            while (!t1.IsCompleted)
            {
                if (isCont==0 | isCont==-1)
                {
                    break;
                }
                num_treat = LoadFileData.Progress;
                BarProgress = num_treat * 100 / num_gross;
                Progress = $"{BarProgress}%";
                await Task.Delay(100);
            }
            Status = "Writing data in file";
            LoadFileData.WriteData();

            if (isCont == -1)
            {
                App.Current.Shutdown();
            }
            else
            {
                if(isCont==0)
                {
                    Status = "Canceled";
                }
                else
                {
                    Status = "Complete";

                }
            }
            this.isCont = 1;
            this.IsActiveDone = true;
        }
    }
}
