﻿using System.Collections.Generic;
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
    public class ApplicationAutoMapperProfileStep : CSharpModificationCreatorStep
    {
        protected override IList<ModificationBuilder<CSharpSyntaxNode>> CreateModifications(WorkflowExecutionContext context, CompilationUnitSyntax rootUnit)
        {
            var model = context.GetVariable<object>("Model");
            string entityUsingText = context.GetVariable<string>("EntityUsingText");
            string entityDtoUsingText = context.GetVariable<string>("EntityDtoUsingText");
            string templateDir = context.GetVariable<string>(VariableNames.TemplateDirectory);
            string contents = TextGenerator.GenerateByTemplateName(templateDir, "ApplicationAutoMapperProfile_CreateMap", model);
            
            return new List<ModificationBuilder<CSharpSyntaxNode>>
            {
                new InsertionBuilder<CSharpSyntaxNode>(
                    root => root.Descendants<UsingDirectiveSyntax>().Last().GetEndLine(),
                    entityUsingText,
                    modifyCondition: root => root.DescendantsNotContain<UsingDirectiveSyntax>(entityUsingText)
                ),
                new InsertionBuilder<CSharpSyntaxNode>(
                    root => root.Descendants<UsingDirectiveSyntax>().Last().GetEndLine(),
                    entityDtoUsingText,
                    modifyCondition: root => root.DescendantsNotContain<UsingDirectiveSyntax>(entityDtoUsingText)
                ),
                new InsertionBuilder<CSharpSyntaxNode>(
                    root => root.Descendants<ConstructorDeclarationSyntax>().Single().GetEndLine(),
                    contents,
                    modifyCondition: root => root.Descendants<ConstructorDeclarationSyntax>().Single().NotContains(contents)
                )
            };
        }

        public ApplicationAutoMapperProfileStep([NotNull] TextGenerator textGenerator) : base(textGenerator)
        {
        }
    }
}