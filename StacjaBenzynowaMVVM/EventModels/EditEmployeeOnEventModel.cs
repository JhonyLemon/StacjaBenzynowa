using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.EventModels
{
    class EditEmployeeOnEventModel
    {
        public int State { get; set; }
        public EditEmployeeOnEventModel(int state)
        {
            State = state;
        }
    }
}
