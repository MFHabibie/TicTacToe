using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TicTacToe.Utility.ServiceLocator
{
    public class _ServiceLocator : MonoBehaviour
    {
        static _ServiceLocator global;
        static Dictionary<Scene, _ServiceLocator> sceneContainers;
        static List<GameObject> tmpSceneList;

        readonly _ServiceManager serviceManager = new _ServiceManager();

        const string k_globalServiceLocatorName = "Service Locator [Global]";
        const string k_sceneServiceLocatorName = "Service Locator [Scene]";

        internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
        {
            if (global == this)
            {
                Debug.LogWarning(message:"_ServiceLocator.ConfigureAsGlobal: Already configurred as global", this);
            }
            else if (global != null)
            {
                Debug.LogError(message: "_ServiceLocator.ConfigureAsGlobal: Another ServiceLocator is already configured as global", this);
            }
            else
            {
                global = this;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(this);
                }
            }
        }

        internal void ConfigureForScene()
        {
            Scene scene = gameObject.scene;

            if (sceneContainers.ContainsKey(scene))
            {
                Debug.LogError(message: "_ServiceLocator.ConfigureForScene: Another ServiceLocator is already configured this scene", this);
                return;
            }
        }

        public static _ServiceLocator Global
        {
            get
            {
                if (global != null) 
                { 
                    return global;
                } 

                if (FindFirstObjectByType<_ServiceLocatorGlobalBootstrapper>() is { } found) 
                {
                    found.BootstrapOnDemand();
                    return global;
                }

                var container = new GameObject(k_globalServiceLocatorName, typeof(_ServiceLocator));
                container.AddComponent<_ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();

                return global;
            }
        }

        public static _ServiceLocator For(MonoBehaviour monoBehaviour)
        {
            return monoBehaviour.GetComponent<_ServiceLocator>() ? monoBehaviour.GetComponent<_ServiceLocator>() : null ?? ForSceneOf(monoBehaviour) ?? Global;
        }

        public static _ServiceLocator ForSceneOf(MonoBehaviour monoBehaviour)
        {
            Scene scene = monoBehaviour.gameObject.scene;

            if (sceneContainers.TryGetValue(scene, out _ServiceLocator container) && container != monoBehaviour)
            {
                return container;
            }

            tmpSceneList.Clear();
            scene.GetRootGameObjects(tmpSceneList);

            foreach(GameObject gameObject in tmpSceneList.Where(gameObject => gameObject.GetComponent<_ServiceLocatorSceneBootstrapper>() != null)) 
            {
                if (gameObject.TryGetComponent(out _ServiceLocatorSceneBootstrapper bootstrapper) && bootstrapper.Container != monoBehaviour)
                {
                    bootstrapper.BootstrapOnDemand();
                    return bootstrapper.Container;
                }
            }

            return Global;
        }

        public _ServiceLocator Register<T>(T service)
        {
            serviceManager.Register(service);
            return this;
        }

        public _ServiceLocator Register(Type type, object service)
        {
            serviceManager.Register(type, service);
            return this;
        }

        public _ServiceLocator Get<T>(out T service) where T : class
        {
            if (TryGetService(out service))
            {
                return this;
            }

            if (TryGetNextInHierarchy(out _ServiceLocator serviceLocator))
            {
                serviceLocator.Get(out service);
                return this;
            }

            throw new ArgumentException(message: $"_ServiceLocator.Get: Service of type {typeof(T).FullName} not registered");
        }

        bool TryGetService<T>(out T service) where T : class
        {
            return serviceManager.TryGet(out service);
        }

        bool TryGetNextInHierarchy(out _ServiceLocator serviceLocator)
        {
            if (this == global)
            {
                serviceLocator = null;
                return false;
            }

            serviceLocator = (transform.parent ? transform.parent : null)?.GetComponentInParent<_ServiceLocator>() ? (transform.parent ? transform.parent : null)?.GetComponentInParent<_ServiceLocator>() : null ?? ForSceneOf(this);
            return serviceLocator != null;
        }

        private void OnDestroy()
        {
            if (this == global)
            {
                global = null;
            }
            else if (sceneContainers.ContainsValue(this))
            {
                sceneContainers.Remove(gameObject.scene);
            }
        }

        //make sure unity clear the static object and variable
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            global = null;
            sceneContainers = new Dictionary<Scene, _ServiceLocator>();
            tmpSceneList = new List<GameObject>();
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/ServiceLocator/Add Global")]
        static void AddGlobal()
        {
            var gameObject = new GameObject(k_globalServiceLocatorName, typeof(_ServiceLocatorGlobalBootstrapper));
        }

        [MenuItem("GameObject/ServiceLocator/Add Scene")]
        static void AddScene()
        {
            var gameObject = new GameObject(k_sceneServiceLocatorName, typeof(_ServiceLocatorSceneBootstrapper));
        }
#endif

    }
}
