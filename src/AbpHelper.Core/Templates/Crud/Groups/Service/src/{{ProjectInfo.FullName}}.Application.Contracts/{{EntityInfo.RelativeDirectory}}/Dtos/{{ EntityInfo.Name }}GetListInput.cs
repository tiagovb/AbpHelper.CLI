{{- SKIP_GENERATE = Option.SkipGetListInputDto -}}
using System;
using System.ComponentModel;
using Volo.Abp.Application.Dtos;
using System.ComponentModel.DataAnnotations;

namespace {{ EntityInfo.Namespace }}.Dtos;

[Serializable]
public class {{ EntityInfo.Name }}GetListInput : PagedAndSortedResultRequestDto
{
    {{~ for prop in EntityInfo.Properties ~}}
    {{~ if prop | abp.is_ignore_property || string.starts_with prop.Type "IList" || string.starts_with prop.Type "List<" ; continue; end ~}}
    {{~ if prop.Document| !string.whitespace ~}}
    /// <summary>
    /// {{ prop.Document }}
    /// </summary>
    {{~ end ~}} 
    {{~ if Option.SkipViewModel ~}}
    [Display(Name = "{{ prop.DisplayName ?? prop.Name }}")]
    {{~ end ~}}    
    public {{ prop.Type}}{{- if !string.ends_with prop.Type "?"; "?"; end}} {{ prop.Name }} { get; set; }
    {{~ if !for.last ~}}

    {{~ end ~}}
    {{~ end ~}}
}