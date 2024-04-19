using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace {{ EntityInfo.Namespace }}.Dtos;

[Serializable]
public class {{ DtoInfo.CreateTypeName }}
{
    {{~ for prop in EntityInfo.Properties ~}}
    {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name; continue; end ~}}
    {{~ if prop | abp.is_ignore_property || string.starts_with prop.Type "IList" || string.starts_with prop.Type "List<"; continue; end ~}}
    {{~ if prop.Document | !string.whitespace ~}}
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