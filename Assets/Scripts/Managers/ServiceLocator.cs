using System;
using System.Collections.Generic;
using UnityEngine;

// Простой сервис-локатор для внедрения зависимостей
public static class ServiceLocator 
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void Register<T>(T service) where T : class
    {
        var type = typeof(T);
        if (_services.ContainsKey(type)) _services[type] = service;
        else _services.Add(type, service);
    }

    public static T Resolve<T>() where T : class
    {
         Type type = typeof(T);
    object service; 
    if (_services.TryGetValue(type, out service)) 
    {
        return service as T;
    }
    return null;
    }
}