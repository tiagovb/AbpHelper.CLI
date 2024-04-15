using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.AbpHelper.Core.Commands.Generate.Crud;
using EasyAbp.AbpHelper.Core.Extensions;
using EasyAbp.AbpHelper.Core.Generator;
using EasyAbp.AbpHelper.Core.Workflow;
using Elsa.Expressions;
using Elsa.Results;
using Elsa.Scripting.JavaScript;
using Elsa.Services.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Scriban;
using Scriban.Runtime;
using Volo.Abp.VirtualFileSystem;

namespace EasyAbp.AbpHelper.Core.Steps.Common
{
    public class GroupGenerationStep : Step
    {
        private readonly TextGenerator _textGenerator;
        private readonly IVirtualFileProvider _virtualFileProvider;
        private const string SkipGenerate = "SKIP_GENERATE";

        public WorkflowExpression<string> TemplateDirectory
        {
            get => GetState<WorkflowExpression<string>>(() => new JavaScriptExpression<string>(VariableNames.TemplateDirectory));
            set => SetState(value);
        }

        public string GroupName
        {
            get => GetState<string>();
            set => SetState(value);
        }

        public WorkflowExpression<string> TargetDirectory
        {
            get => GetState(() => new JavaScriptExpression<string>("BaseDirectory"));
            set => SetState(value);
        }

        public WorkflowExpression<bool> Overwrite
        {
            get => GetState(() => new JavaScriptExpression<bool>("Overwrite"));
            set => SetState(value);
        }

        public WorkflowExpression<object> Model
        {
            get => GetState<WorkflowExpression<object>>(() => new JavaScriptExpression<object>("Model"));
            set => SetState(value);
        }

        public GroupGenerationStep(
            TextGenerator textGenerator,
            IVirtualFileProvider virtualFileProvider)
        {
            _textGenerator = textGenerator;
            _virtualFileProvider = virtualFileProvider;
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            string templateDir = await context.EvaluateAsync(TemplateDirectory, cancellationToken);
            LogInput(() => templateDir);
            LogInput(() => GroupName);
            var targetDirectory = await context.EvaluateAsync(TargetDirectory, cancellationToken);
            LogInput(() => targetDirectory);

            var option = context.GetVariable<object>("Option") as CrudCommandOption;
            bool readOnlyAppServices = option?.ReadOnlyAppServices ?? false;

            string subTargetDirectory = "";
            if (GroupName.Equals("UiRazor", StringComparison.OrdinalIgnoreCase))
            {
                subTargetDirectory = readOnlyAppServices ? "ReadOnly" : "FullCrud";
            }

            var overwrite = await context.EvaluateAsync(Overwrite, cancellationToken);
            LogInput(() => Overwrite);
            var model = await context.EvaluateAsync(Model, cancellationToken);
            LogInput(() => model);

            var groupDir = Path.Combine(templateDir, "Groups", GroupName).NormalizePath();
            await GenerateFile(groupDir, targetDirectory, model, overwrite, subTargetDirectory);

            return Done();
        }

        private async Task GenerateFile(string groupDirectory, string targetDirectory, object model, bool overwrite, string? subTargetDirectory)
        {
            System.Collections.Generic.IEnumerable<(string, IFileInfo)> files = _virtualFileProvider.GetFilesRecursively(groupDirectory)
                .Where(f => string.IsNullOrEmpty(subTargetDirectory) || f.Item1.Contains(subTargetDirectory));

            foreach (var (path, file) in files)
            {
                Logger.LogDebug($"Generating using template file: {path}");
                var targetFilePathNameTemplate = path.Replace(groupDirectory, targetDirectory);
                if (!string.IsNullOrEmpty(subTargetDirectory))
                {
                    targetFilePathNameTemplate = targetFilePathNameTemplate.Replace(subTargetDirectory, string.Empty);
                }
                var targetFilePathName = _textGenerator.GenerateByTemplateText(targetFilePathNameTemplate, model);
                if (File.Exists(targetFilePathName) && !overwrite)
                {
                    Logger.LogInformation($"File {targetFilePathName} already exists, skip generating.");
                    continue;
                }

                var templateText = await file.ReadAsStringAsync();
                var contents = _textGenerator.GenerateByTemplateText(templateText, model, out TemplateContext context);

                context.CurrentGlobal.TryGetValue(SkipGenerate, out object value);
                if (value is bool skipGenerate && skipGenerate)
                {
                    Logger.LogInformation($"Evaluated value of `{SkipGenerate}` is true, skip generating.");
                    continue;
                }

                var dir = Path.GetDirectoryName(targetFilePathName);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                await File.WriteAllTextAsync(targetFilePathName, contents);
                Logger.LogInformation($"File {targetFilePathName} successfully generated.");
            }
        }

    }
}