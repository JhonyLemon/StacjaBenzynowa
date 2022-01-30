using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StacjaBenzynowaMVVM.EventModels
{
    class DeleteClientOnEventModel
    {
        public int State { get; set; }
        public DeleteClientOnEventModel(int state)
        {
            State = state;
        }
    }
}
