using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InversionOfControlContainer.BuildUp
{
    public class DependencyInjectionBuildUp : IBuildUp
    {
        private IoCContainer Container;

        public DependencyInjectionBuildUp(IoCContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Attempts to create an instance of the given type
        /// </summary>
        /// <param name="type">The type of object to create</param>
        /// <returns>A new instance of type T</returns>
        public object Build(Type type)
        {
            object value = null;

            // get all public constructors ordered by parameter count descending
            var ctors = type.GetConstructors().OrderByDescending(c => c.GetParameters().Count()).ToList();

            if (ctors == null || ctors.Count == 0)
            {
                try
                {
                    // attempt to create the instance with the default constructor
                    value = Activator.CreateInstance(type);
                }
                catch
                {
                    throw new InvalidOperationException("No public constructor has been defined.");
                }
            }

            foreach (ConstructorInfo c in ctors)
            {
                ParameterInfo[] parameters = c.GetParameters();

                if (parameters != null && parameters.Length > 0)
                {
                    try
                    {
                        List<object> createdParams = new List<object>();

                        foreach (ParameterInfo p in parameters)
                        {
                            createdParams.Add(Container.Resolve(p.ParameterType));
                        }

                        // the constructor can safely be called because no execption was thrown
                        // meaning that all types have been registered and can be injected
                        value = Activator.CreateInstance(type, createdParams.ToArray());
                    }
                    catch (TypeNotRegisteredException) { } // do nothing with this exception, let other float up
                }
                else
                {
                    // no parameters, create the default instance
                    value = Activator.CreateInstance(type);
                }

                // if the value has been set break the loop
                if (value != null)
                {
                    break;
                }
            }

            if (value == null)
            {
                // if the type couldn't be created an exception is thrown
                throw new NullReferenceException(string.Format("No constructor for type '{0}' could be invoked.", type.Name));
            }

            return value;
        }
    }
}