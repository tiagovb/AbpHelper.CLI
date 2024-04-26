using System;
using AbpTools.AbpHelper.Core.Commands.Ef.Migrations;

namespace AbpTools.AbpHelper.Core.Commands.Ef
{
    public class EfCommand : CommandBase
    {
        public EfCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, "ef", "A shortcut to run 'dotnet ef' commands. See 'abphelper ef --help' for details")
        {
            AddCommand<MigrationsCommand>();
        }
    }
}