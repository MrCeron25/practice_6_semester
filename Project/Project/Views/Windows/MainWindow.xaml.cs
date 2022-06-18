﻿using Project.Views.Pages;
using System.Windows;

namespace Project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Manager.Instance.Context = new Entities();

            Manager.Instance.MainFrame = MainFrame;
            Manager.Instance.MainFrame.Navigate(new LoginPage());

            Manager.Instance.MenuFrame = MenuFrame;
            Manager.Instance.MenuFrame.Navigate(new MainMenuPage());
        }
    }
}