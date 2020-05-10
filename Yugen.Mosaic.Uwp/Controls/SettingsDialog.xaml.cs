﻿using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Yugen.Mosaic.Uwp.ViewModels;
using Yugen.Toolkit.Standard.Commands;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Yugen.Mosaic.Uwp.Controls
{
    public sealed partial class SettingsDialog : ContentDialog
    {
        private ICommand _hideCommand;
        public SettingsViewModel ViewModel => DataContext as SettingsViewModel;

        public SettingsDialog()
        {
            InitializeComponent();
            //DataContext = new SettingsViewModel();
        }

        public ICommand HideCommand => _hideCommand ?? (_hideCommand = new RelayCommand(() => Hide()));
    }
}