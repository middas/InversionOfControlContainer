using InversionOfControlContainer.LifeCycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InversionOfControlContainer
{
    /// <summary>
    /// An Inversion of Control container class that allows you to register types
    /// </summary>
    public class IoCContainer : IDisposable
    {
        private Dictionary<Type, ILifeCycle> Registrants = new Dictionary<Type, ILifeCycle>();

        /// <summary>
        /// Disposes this object
        /// </summary>
        public void Dispose()
        {
            Registrants = null;
        }

        /// <summary>
        /// Registers a given type with a transient life style
        /// </summary>
        /// <typeparam name="I">The type being registered</typeparam>
        /// <typeparam name="T">The implementation of type I</typeparam>
        /// <exception cref="InvalidOperationException">Exception for duplicate registrants</exception>
        public void Register<I, T>()
            where I : class
            where T : I
        {
            Register<I, T>(new TransientLifeCycle());
        }

        /// <summary>
        /// Registers a given type
        /// </summary>
        /// <typeparam name="I">The type being registered</typeparam>
        /// <typeparam name="T">The implementation of type I</typeparam>
        /// <param name="lifeCycle">The desired <see cref="ILifeCycle"/> handler</param>
        /// <exception cref="InvalidOperationException">Exception for duplicate registrants</exception>
        public void Register<I, T>(ILifeCycle lifeCycle)
            where I : class
            where T : I
        {
            Type baseType = typeof(I);
            Type classType = typeof(T);

            // check if the type has already been registered
            if (Registrants.ContainsKey(baseType))
            {
                throw new InvalidOperationException(string.Format("'{0}' has already been registered.", baseType.Name));
            }

            lifeCycle.ItemType = classType;

            // TODO: create a buildup factory
            if (lifeCycle.BuildUp == null)
            {
                // use the default buildup
                lifeCycle.BuildUp = CreateInstance;
            }

            Registrants.Add(baseType, lifeCycle);
        }

        /// <summary>
        /// Attempts to resolve the given type from the registered types
        /// </summary>
        /// <typeparam name="I">The type to be resolved</typeparam>
        /// <returns>The implementation of the given type</returns>
        /// <exception cref="TypeNotRegisteredException">Exception for an unregistered type</exception>
        /// <exception cref="InvalidOperationException">Exception for when the constructor can not be called</exception>
        public I Resolve<I>() where I : class
        {
            Type t = typeof(I);
            I value = Resolve(t) as I;

            return value;
        }

        /// <summary>
        /// Attempts to resolve the given type from the registered types
        /// </summary>
        /// <param name="type">The type to be resolved</param>
        /// <returns>The implementation of the given type</returns>
        /// <exception cref="TypeNotRegisteredException">Exception for an unregistered type</exception>
        /// <exception cref="InvalidOperationException">Exception for when the constructor can not be called</exception>
        public object Resolve(Type type)
        {
            if (!Registrants.ContainsKey(type))
            {
                throw new TypeNotRegisteredException(type);
            }

            return Registrants[type].Value;
        }

        /// <summary>
        /// Attempts to create an instance of the given type
        /// </summary>
        /// <param name="type">The type of object to create</param>
        /// <returns>A new instance of type T</returns>
        private object CreateInstance(Type type)
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
                            createdParams.Add(Resolve(p.ParameterType));
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