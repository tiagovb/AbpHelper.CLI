using System.Reflection;
using AbpTools.AbpHelper.Core;
using AbpTools.AbpHelper.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpTools.AbpHelper
{
    [DependsOn(typeof(AbpHelperCoreModule))]
    public class AbpHelperModule : AbpModule
    {
    }
}