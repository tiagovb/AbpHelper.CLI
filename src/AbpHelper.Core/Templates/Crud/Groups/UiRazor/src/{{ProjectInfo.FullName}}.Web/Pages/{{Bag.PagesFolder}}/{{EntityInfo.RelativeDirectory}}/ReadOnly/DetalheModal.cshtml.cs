{{-
    dtoType = DtoInfo.UpdateTypeName
    if Option.SkipViewModel
        viewModelType = dtoType
    else
        viewModelType = "Detalhe" + EntityInfo.Name + "ViewModel"
    end
-}}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using {{ EntityInfo.Namespace }};
using {{ EntityInfo.Namespace }}.Dtos;
{{~ if Bag.PagesFolder; pagesNamespace = Bag.PagesFolder + "."; end ~}}
{{~ if !Option.SkipViewModel ~}}
using {{ ProjectInfo.FullName }}.Web.Pages.{{ pagesNamespace }}{{ EntityInfo.RelativeNamespace}}.{{ EntityInfo.Name }}.ViewModels;
{{~ end ~}}

namespace {{ ProjectInfo.FullName }}.Web.Pages.{{ pagesNamespace }}{{ EntityInfo.RelativeNamespace }}.{{ EntityInfo.Name }};

public class DetalheModalModel : {{ ProjectInfo.Name }}PageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public {{ EntityInfo.PrimaryKey ?? EntityInfo.CompositeKeyName }} Id { get; set; }

    [BindProperty]
    public {{ viewModelType }} ViewModel { get; set; }

    private readonly I{{ EntityInfo.Name }}AppService _service;

    public DetalheModalModel(I{{ EntityInfo.Name }}AppService service)
    {
        _service = service;
    }

    public virtual async Task OnGetAsync()
    {
        var dto = await _service.GetAsync(Id);
        ViewModel = ObjectMapper.Map<{{ DtoInfo.ReadTypeName }}, {{ viewModelType }}>(dto);
    }

}