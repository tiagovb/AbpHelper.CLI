{{- SKIP_GENERATE = Option.SeparateDto || Option.SkipViewModel -}}
using System;
using System.ComponentModel.DataAnnotations;
{{~ if Bag.PagesFolder; pagesNamespace = Bag.PagesFolder + "."; end ~}}

namespace {{ ProjectInfo.FullName }}.Web.Pages.{{ pagesNamespace }}{{ EntityInfo.RelativeNamespace}}.{{ EntityInfo.Name }}.ViewModels;

public class Detalhe{{ EntityInfo.Name }}ViewModel
{
    {{~ for prop in EntityInfo.Properties ~}}
    {{~ if prop | abp.is_ignore_property; continue; end ~}}
    [Display(Name = "{{ prop.DisplayName ?? prop.Name }}")]
    public {{ prop.Type}} {{ prop.Name }} { get; set; }
    {{~ if !for.last ~}}

    {{~ end ~}}
    {{~ end ~}}
}
