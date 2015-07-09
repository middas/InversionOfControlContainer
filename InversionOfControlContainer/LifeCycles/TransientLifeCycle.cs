using System;

namespace InversionOfControlContainer.LifeCycles
{
    /// <summary>
    /// Manages the life cycle of a transient object
    /// </summary>
    public class TransientLifeCycle : ILifeCycle
    {
        /// <summary>
        /// How to build up the item
        /// </summary>
        public Func<Type, object> BuildUp { get; set; }

        /// <summary>
        /// The type of item
        /// </summary>
        public Type ItemType { get; set; }

        /// <summary>
        /// The value of the item
        /// </summary>
        public object Value
        {
            get { return BuildUp(ItemType); }
        }
    }
}