﻿using AbpTools.AbpHelper.Core.Attributes;

namespace AbpTools.AbpHelper.Core.Commands.Module.Add
{
    public class AddCommandOption : ModuleCommandOption
    {
        [Option('v', "version", Description = "Specify the version of the package(s) to add")]
        public string Version { get; set; } = null!;
    }
}