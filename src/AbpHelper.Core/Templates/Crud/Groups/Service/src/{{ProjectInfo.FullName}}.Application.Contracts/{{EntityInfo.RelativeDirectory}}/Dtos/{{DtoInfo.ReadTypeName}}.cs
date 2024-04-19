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
{{
    if string.contains prop.Type "List"
        propType = string.replace prop.Type ">" "Dto>"
    else if string.contains prop.Modifiers "virtual"
        propType = prop.Type + "Dto"
    else
        propType = prop.Type
    end ~}}
    {{~ if prop | abp.is_ignore_property; continue; end ~}}
    {{~ if prop.Document| !string.whitespace ~}}
    /// <summary>
    /// {{ prop.Document }}
    /// </summary>
    {{~ end ~}} 
    {{~ if Option.SkipViewModel ~}}
    [Display(Name = "{{ prop.DisplayName ?? prop.Name }}")]
    {{~ end ~}}
    public {{ propType}} {{ prop.Name }} { get; set; }
    {{~ if !for.last ~}}

    {{~ end ~}}
    {{~ end ~}}
}