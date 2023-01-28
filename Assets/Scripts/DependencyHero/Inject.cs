using System;

namespace DependencyHero
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Inject : Attribute
    {
        public Type DependencyType { get; set; }
        public Inject(Type dependencyType)
        {
            DependencyType = dependencyType;
        }
    }
}

