{{ SKIP_GENERATE = !Option.ReadOnlyAppServices }}
using System.Threading.Tasks;
using HtmlAgilityPack;
using Shouldly;
using Xunit;
using {{ ProjectInfo.FullName }};
using {{ ProjectInfo.FullName }}.{{ EntityInfo.RelativeNamespace}};

namespace {{ ProjectInfo.FullName }}.Pages.Tests.{{ EntityInfo.RelativeNamespace}};

public class DetalheModalTests : {{ ProjectInfo.Name }}WebTestBase
{
    private readonly I{{ EntityInfo.Name }}Repository _repository;

    public DetalheModalTests()
    {
        _repository = GetRequiredService<I{{ EntityInfo.Name }}Repository>();
        var {{ EntityInfo.Name | abp.camel_case }}1 = new {{ EntityInfo.Name }}
        {
            //Coloque valores das propriedades
        };
        _repository.InsertAsync({{ EntityInfo.Name | abp.camel_case }}1, true);
    }

    [Fact]
    public async Task Detalhe_Modal_Teste()
    {
        // Arrange
        string url = "/{{ EntityInfo.Name }}s/DetalheModal?id=1";//Altere o numero do ID
        
        // Act
        var response = await GetResponseAsync(url);
        var responseString = await GetResponseAsStringAsync(url);
        
        //Assert
        response.ShouldNotBeNull();
        responseString.ShouldNotBeNull();

        //Verifique se o método de busca retorna statusCode diferente de 200 se não achar.
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        /* Utilize código para buscar elementos na resposta obtida para garantir que carregou corretamente. Exemplo:
         * 
         * 
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(responseString);

            var tableElement = htmlDocument.GetElementbyId("ALGUMA_ELEMENTO_DA_TELA");
            tableElement.ShouldNotBeNull();

            var trNodes = tableElement.SelectNodes("//tbody/tr");
            trNodes.Count.ShouldBeGreaterThan(0);
        */
    }
}
