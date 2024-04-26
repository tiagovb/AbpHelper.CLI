using System;
using AbpTools.AbpHelper.Core.Commands.Generate.Controller;
using AbpTools.AbpHelper.Core.Commands.Generate.Crud;
using AbpTools.AbpHelper.Core.Commands.Generate.Localization;
using AbpTools.AbpHelper.Core.Commands.Generate.Methods;
using AbpTools.AbpHelper.Core.Commands.Generate.Service;

namespace AbpTools.AbpHelper.Core.Commands.Generate
{
    public class GenerateCommand : CommandBase
    {
        public GenerateCommand(IServiceProvider serviceProvider) : base(serviceProvider, "generate", "Generate files for ABP projects. See 'abphelper generate --help' for details")
        {
            AddAlias("gen");

            AddCommand<CrudCommand>();
            AddCommand<ServiceCommand>();
            AddCommand<MethodsCommand>();
            AddCommand<LocalizationCommand>();
            AddCommand<ControllerCommand>();
        }
    }
}