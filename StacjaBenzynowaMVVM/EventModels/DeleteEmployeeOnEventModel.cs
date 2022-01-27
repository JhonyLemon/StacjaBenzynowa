using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.EventModels
{
    class DeleteEmployeeOnEventModel
    {
        public int state { get; set; }
        public DeleteEmployeeOnEventModel(int state)
        {
            this.state = state;
        }
    }
}
