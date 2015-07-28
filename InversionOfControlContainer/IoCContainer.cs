using InversionOfControlContainer.BuildUp;
using InversionOfControlContainer.LifeCycles;
using System;
using System.Collections.Generic;

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

            if (lifeCycle.BuildUp == null)
            {
                // use the default buildup
                lifeCycle.BuildUp = new DependencyInjectionBuildUp(this);
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
    }
}