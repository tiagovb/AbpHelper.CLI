{{- SKIP_GENERATE = true  -}} //Desabilitador por enquanto. Ser� gerado no projeto de test do EFCore para consumir BD.
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace {{ EntityInfo.Namespace }};

public class {{ EntityInfo.Name }}AppServiceTests : {{ ProjectInfo.Name }}ApplicationTestBase
{
    private readonly I{{ EntityInfo.Name }}AppService _{{ EntityInfo.Name | abp.camel_case }}AppService;

    public {{ EntityInfo.Name }}AppServiceTests()
    {
        _{{ EntityInfo.Name | abp.camel_case }}AppService = GetRequiredService<I{{ EntityInfo.Name }}AppService>();
    }

    /*
    [Fact]
    public async Task Test1()
    {
        // Arrange

        // Act

        // Assert
    }
    */
}

