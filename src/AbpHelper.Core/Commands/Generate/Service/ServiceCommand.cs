using System;
using System.Threading.Tasks;
using AbpTools.AbpHelper.Core.Extensions;
using AbpTools.AbpHelper.Core.Steps.Abp;
using AbpTools.AbpHelper.Core.Steps.Common;
using AbpTools.AbpHelper.Core.Workflow;
using AbpTools.AbpHelper.Core.Workflow.Generate;
using Elsa.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;
using Humanizer;

namespace AbpTools.AbpHelper.Core.Commands.Generate.Service
{
    public class ServiceCommand : CommandWithOption<ServiceCommandOption>
    {
        public ServiceCommand(IServiceProvider serviceProvider) 
            : base(serviceProvider, "service", "Generate service interface and class files according to the specified name")
        {
        }

        public override Task RunCommand(ServiceCommandOption option)
        {
            if (option.Folder.IsNullOrEmpty())
            {
                option.Folder = option.Name.Pluralize();
            }
            option.Folder = option.Folder.NormalizePath();
            return base.RunCommand(option);
        }

        protected override IActivityBuilder ConfigureBuild(ServiceCommandOption option, IActivityBuilder activityBuilder)
        {
            return base.ConfigureBuild(option, activityBuilder)
                .AddOverwriteWorkflow()
                .Then<SetVariable>(
                    step =>
                    {
                        step.VariableName = VariableNames.TemplateDirectory;
                        step.ValueExpression = new LiteralExpression<string>("/Templates/Service");
                    })
                .Then<SetModelVariableStep>()
                .Then<GroupGenerationStep>(
                    step =>
                    {
                        step.GroupName = "Service";
                        step.TargetDirectory = new JavaScriptExpression<string>(VariableNames.AspNetCoreDir);
                    });
        }
    }
}