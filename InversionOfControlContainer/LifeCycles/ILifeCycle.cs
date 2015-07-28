using InversionOfControlContainer.BuildUp;
using System;

namespace InversionOfControlContainer.LifeCycles
{
    /// <summary>
    /// Manages the life cycle of an object
    /// </summary>
    public interface ILifeCycle
    {
        /// <summary>
        /// How to build up the item
        /// </summary>
        IBuildUp BuildUp { get; set; }

        /// <summary>
        /// The type of item
        /// </summary>
        Type ItemType { get; set; }

        /// <summary>
        /// The value of the item
        /// </summary>
        object Value { get; }
    }
}