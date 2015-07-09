using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversionOfControlContainer.LifeCycles
{
    public interface ILifeCycle
    {
        Type ItemType { get; set; }
        object Value { get; }
        Func<Type, object> BuildUp { get; set; }
    }
}
