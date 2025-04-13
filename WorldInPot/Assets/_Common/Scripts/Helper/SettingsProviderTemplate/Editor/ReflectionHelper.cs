using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public class ReflectionHelper
{
    public static Type[] GetAllTypeInCurrentDomain()
    {
        List<Type> result = new List<Type>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (Assembly assembly in assemblies)
        {
            result.AddRange(assembly.GetTypes());
        }
        return result.ToArray();
    }
    public static Type[] GetTypesWithAttribute<T>() where T : Attribute
    {
        List<Type> result = new List<Type>();

        Type[] types = GetAllTypeInCurrentDomain();
        foreach (Type type in types)
        {
            if(type.IsDefined(typeof(T), false))
            {
                result.Add(type);
            }
        }
        return result.ToArray();
    }

    public static MemberInfo[] GetMembersWithAttribute<T>() where T : Attribute
    {
        List<MemberInfo> result = new List<MemberInfo>();

        Type[] types = GetAllTypeInCurrentDomain();

        foreach (Type type in types)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            MemberInfo[] members = type.GetMembers(flags);

            foreach (MemberInfo member in members)
            {
                if (member.CustomAttributes.ToArray().Any())
                {
                    T attribute = member.GetCustomAttribute<T>();
                    if (attribute != null)
                    {
                        result.Add(member);
                    }
                }
            }
        }

        return result.ToArray();
    }
}