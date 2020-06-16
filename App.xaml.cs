using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Menutest.ViewModels;
using Menutest.Views;

namespace Menutest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // ウィンドウをインスタンス化します
            var w = new MainView();
            // ウィンドウに対する ViewModel をインスタンス化します
            var vm = new MainViewModel();
            // ウィンドウに対する ViewModel をデータコンテキストに指定します
            w.DataContext = vm;
            // ウィンドウを表示します 
            w.Show();
        }

    }
}
