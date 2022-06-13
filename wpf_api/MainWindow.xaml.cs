using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wpf_api.Models;
using wpf_api.ViewModels;

namespace wpf_api
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainViewModel vm;
        private MainWindow()
        {          
            InitializeComponent();
            vm = (MainViewModel)DataContext;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm.ReloadCommandFranchise.Execute(null);
            vm.ReloadCommand.Execute(null);
        }
    }


}
