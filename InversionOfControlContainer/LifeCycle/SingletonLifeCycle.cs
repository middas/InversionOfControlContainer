using System;

namespace InversionOfControlContainer.LifeCycle
{
    public class SingletonLifeCycle : ILifeCycle
    {
        private object _Value = null;

        public Func<Type, object> BuildUp { get; set; }

        public Type ItemType { get; set; }

        public object Value
        {
            get
            {
                if (_Value == null)
                {
                    _Value = BuildUp(ItemType);
                }

                return _Value;
            }
        }
    }
}