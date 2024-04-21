{{ SKIP_GENERATE = Option.ReadOnlyAppServices }}
using HtmlAgilityPack;
using Shouldly;
using Xunit;
using {{ ProjectInfo.FullName }};
using {{ ProjectInfo.FullName }}.{{ EntityInfo.RelativeNamespace}};

namespace {{ ProjectInfo.FullName }}.Pages.Tests.{{ EntityInfo.RelativeNamespace}};

public class CreateModalTests : SISPRECWebTestBase
{
    [Fact]
    public async Task Create_Modal_Teste()
    {
        // Arrange
        var url = "/{{ EntityInfo.RelativeNamespace}}/CreateModal";

        // Act
        var responseAsString = await GetResponseAsStringAsync(url);
        var response = await GetResponseAsync(url);

        // Assert
        responseAsString.ShouldNotBeNull();
        var htmlDocument = new HtmlDocument();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);


        htmlDocument.LoadHtml(responseAsString);

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
