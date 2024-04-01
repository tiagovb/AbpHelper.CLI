using System;
{{~ if !Option.SkipLocalization && Option.SkipViewModel ~}}
using System.ComponentModel;
{{~ end ~}}

namespace {{ EntityInfo.Namespace }}.Dtos;

[Serializable]
public class {{ DtoInfo.CreateTypeName }}  : {{ DtoInfo.ReadTypeName }}
{

}