using AbpTools.AbpHelper.Core.Steps.Abp.ModificationCreatorSteps.CSharp;
using AbpTools.AbpHelper.Core.Steps.Common;
using Elsa.Services;

namespace AbpTools.AbpHelper.Core.Workflow.Generate.Crud
{
    public static class EntityConstructorsGenerationWorkflow
    {
        public static IActivityBuilder AddEntityConstructorsGenerationWorkflow(this IOutcomeBuilder builder)
        {
            return builder
                    .Then<EntityConstructorsStep>()
                    .Then<FileModifierStep>()
                ;
        }
    }
}