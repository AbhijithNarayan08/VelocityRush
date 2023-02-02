
using System.Linq.Expressions;
using System;
namespace DependencyHero
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DependencyInjector 
    {
        private static DependencyInjector _instance;
        public static DependencyInjector Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DependencyInjector();
                return _instance;
            }
        }
        private DependencyInjector() { }

        ~ DependencyInjector() { }

        private Dictionary<Type, object> dependencies = new Dictionary<Type, object>();

        public void Register(Type _t)
        {
            
            object dependency = null;
            dependency = Activator.CreateInstance(_t);
            dependencies[_t] = dependency;
        }



        public T Get<T>()
        {
            return (T)dependencies[typeof(T)];
        }

        public void InjectDependencies(object target)
        {
            var type = target.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            foreach (MethodInfo method in methods)
            {
                object[] injectAttributes = method.GetCustomAttributes(typeof(Inject), true);
                if (injectAttributes.Length == 0)
                    continue;
                
                ParameterInfo[] parameters = method.GetParameters();
                object[] parameterValues = new object[parameters.Length];
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var parameterType = parameter.ParameterType;
                    if (dependencies.ContainsKey(parameterType))
                    {
                        parameterValues[i] = dependencies[parameterType];
                    }
                    else
                    {
                        throw new Exception($"Dependency of type {parameterType} not found.");
                    }
                }
                method.Invoke(target, parameterValues);
            }
        }
    }

}
