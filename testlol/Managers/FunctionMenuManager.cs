using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Models.Datas;
using testlol.Types;

namespace testlol.Managers
{
    public class FunctionMenuManager
    {
        private static FunctionMenuManager _instance;
        public static FunctionMenuManager Instance => _instance ?? (_instance = new FunctionMenuManager());

        private IEnumerable<FunctionMenuItemData> _items = null;

        public IEnumerable<FunctionMenuItemData> GetFunctionListData()
        {
            if (_items != null)
            {
                return _items;
            }

            var items = new List<FunctionMenuItemData>();

            items.Add(new FunctionMenuItemData(FunctionType.Home, "Home"));
            items.Add(new FunctionMenuItemData(FunctionType.Record, "Record"));
            items.Add(new FunctionMenuItemData(FunctionType.Queue, "Queue"));



            return items;
        }
    }
}
