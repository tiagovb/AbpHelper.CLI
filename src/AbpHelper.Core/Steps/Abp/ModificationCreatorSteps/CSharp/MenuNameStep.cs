using System.Collections.Generic;
using System.Linq;
using AbpTools.AbpHelper.Core.Extensions;
using AbpTools.AbpHelper.Core.Generator;
using AbpTools.AbpHelper.Core.Models;
using AbpTools.AbpHelper.Core.Workflow;
using Elsa.Services.Models;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AbpTools.AbpHelper.Core.Steps.Abp.ModificationCreatorSteps.CSharp
{
    public class MenuNameStep : CSharpModificationCreatorStep
    {
        public MenuNameStep([NotNull] TextGenerator textGenerator) : base(textGenerator)
        {
        }

        protected override IList<ModificationBuilder<CSharpSyntaxNode>> CreateModifications(WorkflowExecutionContext context, CompilationUnitSyntax rootUnit)
        {
            var model = context.GetVariable<object>("Model");
            string templateDir = context.GetVariable<string>(VariableNames.TemplateDirectory);
            string addMenuNameText = TextGenerator.GenerateByTemplateName(templateDir, "Menus_AddMenuName", model);
            
            return new List<ModificationBuilder<CSharpSyntaxNode>>
            {
                new InsertionBuilder<CSharpSyntaxNode>(
                    root => root.Descendants<ClassDeclarationSyntax>().Single().GetEndLine(),
                    addMenuNameText,
                    modifyCondition: root => root.DescendantsNotContain<ClassDeclarationSyntax>(addMenuNameText)
                ),
            };
        }
    }
}