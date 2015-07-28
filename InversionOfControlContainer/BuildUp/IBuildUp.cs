using System;

namespace InversionOfControlContainer.BuildUp
{
    public interface IBuildUp
    {
        /// <summary>
        /// Attempts to create an instance of the given type
        /// </summary>
        /// <param name="type">The type of object to create</param>
        /// <returns>A new instance of type T</returns>
        object Build(Type type);
    }
}