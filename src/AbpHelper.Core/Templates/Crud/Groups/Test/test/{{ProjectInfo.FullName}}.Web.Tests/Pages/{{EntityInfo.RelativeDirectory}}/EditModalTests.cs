{{ SKIP_GENERATE = Option.ReadOnlyAppServices }}
using HtmlAgilityPack;
using Shouldly;
using {{ ProjectInfo.FullName }};
using {{ ProjectInfo.FullName }}.{{ EntityInfo.RelativeNamespace}};
using Xunit;

namespace {{ ProjectInfo.FullName }}.Pages.Tests.{{ EntityInfo.RelativeNamespace }};

{{- 
entityNameLowerCase = EntityInfo.Name | abp.camel_case
entityObj = entityNameLowerCase + "Obj"
-}}

public class EditModalTests : SISPRECWebTestBase
{
    private readonly I{{ EntityInfo.Name }}Repository _{{entityNameLowerCase}}Repository;
    private readonly {{ EntityInfo.Name }} {{ entityObj }};

    public EditModalTests()
    {
        _{{entityNameLowerCase}}Repository = GetRequiredService<I{{ EntityInfo.Name }}Repository>();

        {{entityNameLowerCase}}Obj = new {{ EntityInfo.Name }}
        {
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
            {{~ if prop | abp.is_ignore_property ; continue; end ~}}
            {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name ; continue; end ~}}
            {{ prop.Name }} = ({{prop.Type}})"",//preencher com valores;
            {{~ end ~}}
        };
        
        _{{entityNameLowerCase}}Repository.InsertAsync({{entityObj}}, true);
    }

    [Fact]
    public async Task Edit_Modal_Teste()
    {
        // Arrange
        var url = "/{{ EntityInfo.RelativeNamespace }}/EditModal?id=" + {{entityObj}}.Id;

        // Act
        var responseAsString = await GetResponseAsStringAsync(url);
        var response = await GetResponseAsync(url);

        // Assert
        responseAsString.ShouldNotBeNull();
        var htmlDocument = new HtmlDocument();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        htmlDocument.LoadHtml(responseAsString);

        // Obtenha o elemento de entrada oculto pelo ID e verifique se o valor corresponde ao autorObj.Id
        var hiddenIdInput = htmlDocument.GetElementbyId("Id");
        hiddenIdInput.ShouldNotBeNull();

        // AJUSTE AQUI!! Faça os ajustes para a tela a ser testada
        // Verifica campos preenchidos
        {{~ for prop in EntityInfo.Properties ~}}
        {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
        {{~ if prop | abp.is_ignore_property ; continue; end ~}}
        {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name ; continue; end ~}}
        var {{ prop.Name }}Input = htmlDocument.GetElementbyId("ViewModel_{{ prop.Name }}");
        {{ prop.Name }}Input.ShouldNotBeNull();
        {{ prop.Name }}Input.Attributes["value"].Value.ShouldBe({{ entityObj }}.{{ prop.Name }});
        {{~ if !for.last ~}}

        {{~ end ~}} 
        {{~ end ~}}

        // Verificar existência do rodapé do modal
        var footer = htmlDocument.DocumentNode.SelectSingleNode("//*[contains(@class, 'modal-footer')]");
        footer.ShouldNotBeNull();

        // Verificar botões Cancelar e Salvar
        var buttons = footer.SelectNodes(".//button");
        buttons.Count.ShouldBe(2);
        buttons[0].InnerText.Trim().ShouldBe("Cancelar");
        buttons[1].InnerText.Trim().ShouldBe("Salvar");
    }
}
