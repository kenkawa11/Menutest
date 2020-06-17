namespace Menutest.Views.Behaviors
{
    using System;
    using System.Windows;
    using Microsoft.Win32;
    using MSAPI = Microsoft.WindowsAPICodePack;
    public static class CommonDialogBehavior
    {
        public static DependencyProperty CallbackProperty = DependencyProperty.RegisterAttached("Callback", typeof(Action<bool, string>),
            typeof(CommonDialogBehavior), new PropertyMetadata(null, OnCallbackPropertyChanged));

        public static  Action<bool, string> GetCallback(DependencyObject target)
        {
            return (Action<bool, string>)target.GetValue(CallbackProperty);
        }

        public static void SetCallback(DependencyObject target, Action<bool, string> value)
        {
            target.SetValue(CallbackProperty, value);
        }

        private static void OnCallbackPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var callback = GetCallback(sender);
            if (callback != null)
            {
                var dlg = new OpenFileDialog()
                {
                    FileName = "",
                    Filter="CSV file|*.csv",
                    Multiselect=false,
                    Title="Open Anki data file"
                };
                var owner = Window.GetWindow(sender); 
                var result = dlg.ShowDialog(owner); 
                callback(result.Value, dlg.FileName);
            }
        }

        public static DependencyProperty DirCallbackProperty = DependencyProperty.RegisterAttached("DirCallback", typeof(Action<bool, string>),
            typeof(CommonDialogBehavior), new PropertyMetadata(null, OnDirCallbackPropertyChanged));

        public static Action<bool, string> GetDirCallback(DependencyObject target)
        {
            return (Action<bool, string>)target.GetValue(DirCallbackProperty);
        }

        public static void SetDirCallback(DependencyObject target, Action<bool, string> value)
        {
            target.SetValue(DirCallbackProperty, value);
        }

        private static void OnDirCallbackPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var dircallback = GetDirCallback(sender);
            if(dircallback!=null)
            {
                var dlg = new MSAPI::Dialogs.CommonOpenFileDialog
                {

                    // フォルダ選択ダイアログ（falseにするとファイル選択ダイアログ）
                    IsFolderPicker = true,
                    // タイトル
                    Title = "Select Anki Media Folder",
                    // 初期ディレクトリ
                    InitialDirectory = @"C:\Work"
                };

                var owner = Window.GetWindow(sender);
                if (dlg.ShowDialog(owner) == MSAPI::Dialogs.CommonFileDialogResult.Ok)
                {
                    var dir = dlg.FileName + @"\";
                    dircallback(true, dir);
                }
            }
        }
    }

}
