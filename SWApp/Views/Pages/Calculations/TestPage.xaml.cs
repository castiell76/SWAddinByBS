﻿using SWApp.ControlsLookup;
using SWApp.Viewmodels.Pages.Calculations;
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
using Wpf.Ui.Controls;

namespace SWApp.Views.Pages.Calculations
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    /// 
    [SWAppPage("TestPage.", SymbolRegular.ControlButton24)]
    public partial class TestPage : Page
    {
        public TestPageViewModel ViewModel;
        public TestPage()
        {
            ViewModel = new TestPageViewModel();
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}
