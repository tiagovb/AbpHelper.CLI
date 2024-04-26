using AbpTools.AbpHelper.Core.Commands;
using AbpTools.AbpHelper.Core.Commands.Generate.Crud;
using AbpTools.AbpHelper.Core.Models;
using AbpTools.AbpHelper.Core.Steps.Common;
using Elsa;
using Elsa.Activities;
using Elsa.Activities.ControlFlow.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;

namespace AbpTools.AbpHelper.Core.Workflow.Common
{
    public static class ConfigureHasDbMigrationsWorkflow
    {
        private const string SearchDbMigrationsResultName = nameof(SearchDbMigrationsResultName);

        public static IActivityBuilder AddConfigureHasDbMigrationsWorkflow(this IActivityBuilder builder, string nextActivityName)
        {
            return builder
                    .Then<FileFinderStep>(
                    step => {
                        step.SearchFileName = new LiteralExpression("*.EntityFrameworkCore.DbMigrations.csproj");
                        step.ErrorIfNotFound = new JavaScriptExpression<bool>("false");
                    })
                    .Then<IfElse>(
                        step => step.ConditionExpression = new JavaScriptExpression<bool>("FileFinderResult != null"),
                        ifElse =>
                        {
                            ifElse.When(OutcomeNames.True)
                                .Then<SetVariable>(step =>
                                {
                                    step.VariableName = VariableNames.HasDbMigrations;
                                    step.ValueExpression = new JavaScriptExpression<bool>("true");
                                })
                                .Then(nextActivityName)
                                ;
                            ifElse.When(OutcomeNames.False)
                                .Then<SetVariable>(step =>
                                {
                                    step.VariableName = VariableNames.HasDbMigrations;
                                    step.ValueExpression = new JavaScriptExpression<bool>("false");
                                })
                                .Then(nextActivityName)
                                ;
                    })
                    ;
        }
    }
}
