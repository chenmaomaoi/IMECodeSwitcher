// https://www.cnblogs.com/SaoJian/p/17462782.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Extensions.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterAllServices(this IServiceCollection services)
    {
        //获取当前程序集
        var entryAssembly = Assembly.GetEntryAssembly();

        //获取所有类型
        //!. null包容运算符，当你明确知道表达式的值不为null 使用!.（即null包容运算符）可以告知编译器这是预期行为，不应发出警告
        //例: entryAssembly!.GetReferencedAssemblies() 正常
        //entryAssembly.GetReferencedAssemblies() 编译器判断entryAssembly有可能为null，变量下方出现绿色波浪线警告

        var types = entryAssembly!.GetReferencedAssemblies()//获取当前程序集所引用的外部程序集
            .Select(Assembly.Load)//装载
            .Concat(new List<Assembly>() { entryAssembly })//与本程序集合并
            .SelectMany(x => x.GetTypes())//获取所有类
            .Distinct();//排重

        //三种生命周期分别注册
        Register<ITransientService>(types, services.AddTransient, services.AddTransient);
        Register<IScopedService>(types, services.AddScoped, services.AddScoped);
        Register<ISingletonService>(types, services.AddSingleton, services.AddSingleton);

        Register<IHostedService>(types, services.AddSingleton, services.AddSingleton);

        return services;
    }

    /// <summary>
    /// 根据服务标记的生命周期interface，不同生命周期注册到容器里面
    /// </summary>
    /// <typeparam name="TLifetime">注册的生命周期</typeparam>
    /// <param name="types">集合类型</param>
    /// <param name="register">委托：成对注册</param>
    /// <param name="registerDirectly">委托：直接注册服务实现</param>
    private static void Register<TLifetime>(IEnumerable<Type> types, Func<Type, Type, IServiceCollection> register, Func<Type, IServiceCollection> registerDirectly)
    {
        //找到所有标记了Tlifetime生命周期接口的实现类
        var tImplements = types.Where(x => x.IsClass && !x.IsAbstract && x.GetInterfaces().Any(tinterface => tinterface == typeof(TLifetime)));

        //遍历，挨个以其他所有接口为key，当前实现为value注册到容器中
        foreach (var t in tImplements)
        {
            //获取除生命周期接口外的所有其他接口
            var interfaces = t.GetInterfaces();
            if (interfaces.Length != 0)
            {
                foreach (var i in interfaces)
                {
                    if (i == typeof(ITransientService)
                        || i == typeof(IScopedService)
                        || i == typeof(ISingletonService))
                    {
                        continue;
                    }

                    register(i, t);
                }
            }

            //注入基类
            var baseType = t.BaseType;
            if (baseType != null && baseType != typeof(object))
            {
                register(baseType, t);
            }

            //注入实现类本身
            registerDirectly(t);
        }
    }
}
