using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe.Utility.ServiceLocator
{
    /// <summary>
    /// Simple service locator for <see cref="IGameService"/> instances.
    /// </summary>
    public class _ServiceManager : MonoBehaviour
    {
        readonly Dictionary<Type, object> services = new Dictionary<Type, object> ();
        public IEnumerable<object> RegisteredServices => services.Values;


        public bool TryGet<T>(out T service) where T : class {
            Type serviceType = typeof(T);
            if (services.TryGetValue(serviceType, out object serviceObject))
            {
                service = serviceObject as T;
                return true;
            }

            service = null;
            return false;
        }

        public T Get<T>() where T : class {
            Type serviceType = typeof (T);
            if (services.TryGetValue (serviceType, out object service))
            {
                return service as T;
            }

            throw new ArgumentException(message: $"_ServiceLocator.Get: Service of type {serviceType.FullName} not registered");
        }

        public _ServiceManager Register<T>(T service) {
            Type serviceType = typeof(T);

            if (!services.TryAdd(serviceType, service))
            {
                Debug.LogError(message: $"_ServiceLocator.Register: Service of type {serviceType.FullName} already registered");
            }

            return this;
        }

        public _ServiceManager Register(Type serviceType, object service)
        {
            if (!serviceType.IsInstanceOfType(service)) 
            {
                throw new ArgumentException(message: "Type of service does not match type of service interface", nameof(service));
            }

            if (!services.TryAdd(serviceType, service))
            {
                Debug.LogError(message: $"_ServiceLocator.Register: Service of type {serviceType.FullName} already registered");
            }

            return this;
        }
    }

}
