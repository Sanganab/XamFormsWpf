using System;
using System.Collections.Generic;

namespace XamFormsWpf.Core.Services
{
    public sealed class SimpleIoC
    {
        private static volatile SimpleIoC _instance;
        private static object _syncRoot = new object();
        private Dictionary<Type, Type> _multiInstance = new Dictionary<Type, Type>();
        private Dictionary<Type, object> _singletons = new Dictionary<Type, object>();

        private SimpleIoC() { }
        public static SimpleIoC Container
        {
            get {
                if(_instance == null) {
                    lock(_syncRoot) {
                        if(_instance == null)
                            _instance = new SimpleIoC();
                    }
                }

                return _instance;
            }
        }

        public void Register<Implementation>()
        {
            RegisterSingleton<Implementation, Implementation>();
        }

        public void RegisterSingleton<Implementation>()
        {
            RegisterSingleton<Implementation, Implementation>();
        }

        public void Register<Interface, Implementation>()
        {
            Validate<Interface, Implementation>();
            _multiInstance.Add(typeof(Interface), typeof(Implementation));
        }

        public void RegisterSingleton<Interface, Implementation>()
        {
            Validate<Interface, Implementation>();
            _singletons.Add(typeof(Interface), Activator.CreateInstance(typeof(Implementation)));
        }

        public Type Resolve<Type>()
        {
            if(_singletons.ContainsKey(typeof(Type))) {
                return (Type)_singletons[typeof(Type)];
            }
            else if(_multiInstance.ContainsKey(typeof(Type))) {
                var theType = _multiInstance[typeof(Type)];
                var obj = Activator.CreateInstance(theType);
                return (Type)obj;
            }
            else
                throw new Exception($"Type {typeof(Type).ToString()} is not registered");
        }

        private void Validate<Interface, Implementation>()
        {
            // This will fail up-front if the class cannot be instantiated correctly
            Activator.CreateInstance<Implementation>();

            if(_multiInstance.ContainsKey(typeof(Interface))) {
                throw new Exception($"Type {_multiInstance[typeof(Interface)].ToString()} is already registered for {typeof(Interface).ToString()}.");
            }
            else if(_singletons.ContainsKey(typeof(Interface))) {
                throw new Exception($"Type {_singletons[typeof(Interface)].ToString()} is already registered for {typeof(Interface).ToString()}.");
            }
        }
    }
}
