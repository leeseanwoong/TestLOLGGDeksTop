using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using testlol.API;
using testlol.Managers;
using testlol.Models.DTOs;
using testlol.Models.DTOs.Sumonner_V4;
using testlol.Scripts;
using testlol.Utills;

namespace testlol.ViewModels
{
    internal class MenuViewModel : ViewModelBase
    {
        private ObservableCollection<MenuItemViewModel> _items;

        public ObservableCollection<MenuItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private MenuItemViewModel _selectedType;

        public MenuItemViewModel SelectedType
        {
            get => _selectedType;
            set
            {
                if (SetProperty(ref _selectedType, value))
                {
                    OnChangedSelectedType();
                }
            }
        }

        public EventHandler ChangedSelectedType;

        private void OnChangedSelectedType()
        {
            ChangedSelectedType?.Invoke(this, EventArgs.Empty);
        }
    }
}
