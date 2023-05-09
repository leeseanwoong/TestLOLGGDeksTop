using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testlol.Types;

namespace testlol.Models.Datas
{
    public class FunctionMenuItemData
    {
        public FunctionMenuItemData(FunctionType type, string name)
        {
            this.Type = type;
            this.Name = name;
        }

        public FunctionType Type { get; }

        public string Name { get; }

    }
}
