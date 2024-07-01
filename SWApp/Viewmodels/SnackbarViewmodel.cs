using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Controls;
using Wpf.Ui;

namespace SWApp.Viewmodels
{
    public partial class SnackbarViewModel(ISnackbarService snackbarService) : ObservableObject
    {
        private ControlAppearance _snackbarAppearance = ControlAppearance.Secondary;

        [ObservableProperty]
        private int _snackbarTimeout = 2;

        private int _snackbarAppearanceComboBoxSelectedIndex = 1;

        public int SnackbarAppearanceComboBoxSelectedIndex
        {
            get => _snackbarAppearanceComboBoxSelectedIndex;
            set
            {
                _ = SetProperty(ref _snackbarAppearanceComboBoxSelectedIndex, value);
                UpdateSnackbarAppearance(value);
            }
        }

        //[RelayCommand]
        public void OnOpenSnackbar(string title, string message)
        {
            snackbarService.Show(title, message,_snackbarAppearance,
                new SymbolIcon(SymbolRegular.Fluent24),
                TimeSpan.FromSeconds(SnackbarTimeout)
            );
        }

        private void UpdateSnackbarAppearance(int appearanceIndex)
        {
            _snackbarAppearance = appearanceIndex switch
            {
                1 => ControlAppearance.Secondary,
                2 => ControlAppearance.Info,
                3 => ControlAppearance.Success,
                4 => ControlAppearance.Caution,
                5 => ControlAppearance.Danger,
                6 => ControlAppearance.Light,
                7 => ControlAppearance.Dark,
                8 => ControlAppearance.Transparent,
                _ => ControlAppearance.Primary
            };
        }
    }
}
