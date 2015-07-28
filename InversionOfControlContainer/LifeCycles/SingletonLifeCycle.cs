using InversionOfControlContainer.BuildUp;
using System;

namespace InversionOfControlContainer.LifeCycles
{
    /// <summary>
    /// Manages the life cycle of a singleton object
    /// </summary>
    public class SingletonLifeCycle : ILifeCycle
    {
        private object _Value = null;

        /// <summary>
        /// How to build up the item
        /// </summary>
        public IBuildUp BuildUp { get; set; }

        /// <summary>
        /// The type of item
        /// </summary>
        public Type ItemType { get; set; }

        /// <summary>
        /// The value of the item
        /// </summary>
        public object Value
        {
            get
            {
                if (_Value == null)
                {
                    _Value = BuildUp.Build(ItemType);
                }

                return _Value;
            }
        }
    }
}