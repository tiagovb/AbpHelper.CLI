using AbpTools.AbpHelper.Core.Steps.Common;
using AbpTools.AbpHelper.Core.Workflow.Common;
using Elsa.Scripting.JavaScript;
using Elsa.Services;

namespace AbpTools.AbpHelper.Core.Workflow.Generate.Crud
{
    public static class MigrationAndUpdateDatabaseWorkflow
    {
        public static IActivityBuilder AddMigrationAndUpdateDatabaseWorkflow(this IOutcomeBuilder builder)
        {
            return builder
                    .Then<EmptyStep>()
                    .AddConfigureMigrationProjectsWorkflow(ActivityNames.AddMigration)
                    /* Add migration */
                    .Then<RunCommandStep>(
                        step => step.Command = new JavaScriptExpression<string>("`dotnet ef migrations add Added${EntityInfo.Name} -p \"${MigrationProjectFile}\" -s \"${StartupProjectFile}\"`")
                    ).WithName(ActivityNames.AddMigration)
                    /* Update database */
                    .Then<RunCommandStep>(
                        step => step.Command = new JavaScriptExpression<string>("`dotnet ef database update -p \"${MigrationProjectFile}\" -s \"${StartupProjectFile}\"`")
                    )
                ;
        }
    }
}