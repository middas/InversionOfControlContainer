using System;

namespace InversionOfControlContainer.LifeCycle
{
    public class TransientLifeCycle : ILifeCycle
    {
        public Func<Type, object> BuildUp { get; set; }

        public Type ItemType { get; set; }

        public object Value
        {
            get { return BuildUp(ItemType); }
        }
    }
}