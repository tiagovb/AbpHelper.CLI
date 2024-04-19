{{- SKIP_GENERATE = DtoInfo.CreateTypeName == DtoInfo.UpdateTypeName -}}
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace {{ EntityInfo.Namespace }}.Dtos;

{{~ if EntityInfo.Document | !string.whitespace ~}}
/// <summary>
/// {{ EntityInfo.Document }}
/// </summary>
{{~ end ~}}
[Serializable]
public class {{ DtoInfo.UpdateTypeName }}
{
    {{~ for prop in EntityInfo.Properties ~}}
    {{~ if string.contains prop.Modifiers "virtual" ; continue; end ~}}
    {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name; continue; end ~}}
    {{~ if prop | abp.is_ignore_property || string.starts_with prop.Type "IList" || string.starts_with prop.Type "List<"; continue; end ~}}
    {{~ if prop.Document| !string.whitespace ~}}
    /// <summary>
    /// {{ prop.Document }}
    /// </summary>
    {{~ end ~}} 
    [Display(Name = "{{ prop.DisplayName ?? prop.Name }}")]
    public {{ prop.Type}} {{ prop.Name }} { get; set; }
    {{~ if !for.last ~}}

    {{~ end ~}}
    {{~ end ~}}
}