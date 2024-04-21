{{ SKIP_GENERATE = (ProjectInfo.TemplateType == 'Module')}}
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace {{ ProjectInfo.FullName }}.Pages.Tests.{{ EntityInfo.RelativeNamespace}};

public class Index_Tests : {{ ProjectInfo.Name }}WebTestBase
{
    [Fact]
    public async Task Index_Page_Test()
    {
        // Arrange
        string url = "/{{ EntityInfo.RelativeNamespace}}";
        
        // Act
        var response = await GetResponseAsync(url);
        var responseString = await GetResponseAsStringAsync(url);
        
        //Assert
        response.ShouldNotBeNull();
        responseString.ShouldNotBeNull();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
    }
}
