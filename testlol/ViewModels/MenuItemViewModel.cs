using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.Datas;
using testlol.Types;

namespace testlol.ViewModels
{
    internal class MenuItemViewModel : ViewModelBase
    {
        #region Constructors
        public MenuItemViewModel(FunctionType type, string name)
        {
            Type = type;
            Name = name;
        }
        #endregion


        #region Bindable Properties
        private FunctionType _type;

        public FunctionType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private string _name;


        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        

        #endregion

        #region Helpers
        public static MenuItemViewModel From(FunctionMenuItemData data)
        {
            return new MenuItemViewModel
            (
                type: data.Type,
                name: data.Name
            );
        }
        #endregion
    }
}
