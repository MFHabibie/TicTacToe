using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TicTacToe.Utility.ServiceLocator
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(_ServiceLocator))]
    public abstract class _Bootstrapper : MonoBehaviour
    {
        _ServiceLocator _serviceLocator;
        internal _ServiceLocator Container => _serviceLocator ? _serviceLocator : null ?? (_serviceLocator = GetComponent<_ServiceLocator>());

        bool hasBeenBootstrapped;

        void Awake() => BootstrapOnDemand();

        public void BootstrapOnDemand()
        {
            if (hasBeenBootstrapped) return;
            hasBeenBootstrapped = true;
            Bootstrap();
        }

        protected abstract void Bootstrap();
    }

    [AddComponentMenu("ServiceLocator/ServiceLocator Global")]
    public class _ServiceLocatorGlobalBootstrapper : _Bootstrapper
    {
        [SerializeField]
        bool dontDestroyOnLoad = true;

        protected override void Bootstrap()
        {
            Container.ConfigureAsGlobal(dontDestroyOnLoad);
        }
    }

    [AddComponentMenu("ServiceLocator/ServiceLocator Scene")]
    public class _ServiceLocatorSceneBootstrapper : _Bootstrapper
    {
        protected override void Bootstrap()
        {
            Container.ConfigureForScene();
        }
    }
}

