﻿using System;
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
using tesla_wpf.Route.ViewModel;

namespace tesla_wpf.Route.View {
    /// <summary>
    /// TypingGame.xaml 的交互逻辑
    /// </summary>
    public partial class TypingGame : UserControl {
        public TypingGame() {
            InitializeComponent();
            DataContext = new TypingGameViewModel();
        }
    }
}
