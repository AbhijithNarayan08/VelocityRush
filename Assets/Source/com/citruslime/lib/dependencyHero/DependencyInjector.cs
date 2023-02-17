
using System.Linq.Expressions;
using System;
namespace com.citruslime.lib.dependencyHero
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class DependencyInjector 
    {
        private static DependencyInjector _instance = null;
        
        public static DependencyInjector Instance => _instance ?? 
                                                        (_instance = new DependencyInjector());

        private Dictionary<Type, object> dependencies = null;


        private DependencyInjector() 
        { 
            dependencies = new Dictionary<Type, object>();
        }

        ~ DependencyInjector() { }


        public void Register(Type _t)
        {
            
            object dependency = null;
            dependency = Activator.CreateInstance(_t); // Ive heard the faster way is to create through the LamdaExpressions Class ask AMAN about it 
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
                    if (parameterType.IsInterface)
                    {
                        Type implementationType = dependencies.Keys.FirstOrDefault(k => parameterType.IsAssignableFrom(k));
                        if (implementationType != null)
                        {
                            parameterValues[i] = dependencies[implementationType];
                        }
                        else
                        {
                            throw new Exception($"Dependency implementing {parameterType} not found.");
                        }
                    }
                    else
                    {
                        if (dependencies.ContainsKey(parameterType))
                        {
                            parameterValues[i] = dependencies[parameterType];
                        }
                        else
                        {
                            throw new Exception($"Dependency of type {parameterType} not found.");
                        }
                    }
                }
                method.Invoke(target, parameterValues);
            }
        }

    }

}
