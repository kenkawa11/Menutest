using System;
using PronunDLWPF;
using System.Windows;
using Menutest.ViewModels.Notification;
using System.ComponentModel;

namespace Menutest.ViewModels
{


    public class MainViewModel: NotificationObject
    {
        private string status;
        private string progress;
        private int barProgress;
        private string fn;
        private string dir;
        private bool isActiveDone;
        private string rpt;
        private readonly PronounceDownloader DownLoder = new PronounceDownloader();

        public MainViewModel()
        {
            dir = @"C:\Users\naobaby\Desktop\test\";
            isActiveDone = true;
            DownLoder.Ret.PropertyChanged += OnModelChanged;
        }

        public string Rpt
        {
            get { return this.rpt; }
            set { SetProperty(ref this.rpt, value); }
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


        public bool IsActiveDone
        {
            get { return this.isActiveDone; }
            set { SetProperty(ref this.isActiveDone, value); }
        }



        private DelegateCommand _openFileCommand;
        private DelegateCommand _selectDirectory;
        private DelegateCommand _done;
        private DelegateCommand _cancel;
        private DelegateCommand _exitApplication;

        public DelegateCommand ExitApplication
        { 
            get 
            {
                return this._exitApplication ??= new DelegateCommand
                    (_ => { 
                        OnExit(); 
                    });
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



        public DelegateCommand Done
        {
            get
            {
                if (this._done == null)
                {
                    this._done= new DelegateCommand(_ =>
                    {
                        if (fn != null)
                        {
                            Status = "Initialization";
                            IsActiveDone = false;
                            DownLoder.TreatData(fn, dir);
                        }
                        else
                        {
                            MessageBox.Show("Select a target file !!!");
                        }
                    });
                }
                return this._done;
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
                        DownLoder.Ret.Status = "Canceled";
                    });
                }
                return this._cancel;
            }
        }




        private void OnModelChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "progress":
                    BarProgress = DownLoder.Ret.Progress;
                    Progress = $"{BarProgress}%";
                    break;
                case "rpt":
                    Rpt = DownLoder.Ret.Rpt;
                    break;
                case "status":
                    Status = DownLoder.Ret.Status;
                    if (Status == "Completed" || Status == "Canceled")
                    {
                        this.IsActiveDone = true;
                    }
                    break;

            }
        }
    }
}
