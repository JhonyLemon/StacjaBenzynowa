using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.EventModels
{
    class ChangePasswordOnEventModel
    {
        public int State { get; set; }
        public ChangePasswordOnEventModel(int state)
        {
            State = state;
        }
    }
}
