using System;
using Volo.Abp.Application.Dtos;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace {{ EntityInfo.Namespace }}.Dtos;

{{~ if EntityInfo.Document | !string.whitespace ~}}
/// <summary>
/// {{ EntityInfo.Document }}
/// </summary>
{{~ end ~}}
[Serializable]
public class {{ DtoInfo.ReadTypeName }} : {{ EntityInfo.BaseType | string.replace "AggregateRoot" "Entity"}}Dto{{ if EntityInfo.PrimaryKey }}<{{ EntityInfo.PrimaryKey}}>{{ end }}
{
    {{~ for prop in EntityInfo.Properties ~}}
    {{~ if prop | abp.is_ignore_property; continue; end ~}}
    {{~ if prop.Document| !string.whitespace ~}}
    /// <summary>
    /// {{ prop.Document }}
    /// </summary>
    {{~ end ~}} 
    {{~ if Option.SkipViewModel ~}}
    [Display(Name = "{{ prop.DisplayName ?? prop.Name }}")]
    {{~ end ~}}    
    public {{ prop.Type}} {{ prop.Name }} { get; set; }
    {{~ if !for.last ~}}

    {{~ end ~}}
    {{~ end ~}}
}