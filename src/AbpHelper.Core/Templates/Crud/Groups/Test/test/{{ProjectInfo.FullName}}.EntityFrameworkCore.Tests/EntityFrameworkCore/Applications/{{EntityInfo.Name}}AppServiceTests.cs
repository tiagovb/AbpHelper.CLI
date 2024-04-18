using Shouldly;
using System.Threading.Tasks;
using Xunit;
using {{ ProjectInfo.FullName }}.Samples;
using {{ ProjectInfo.FullName }}.EntityFrameworkCore;

namespace {{ EntityInfo.Namespace }};

[Collection(SISPRECTestConsts.CollectionDefinitionName)]
public class {{ EntityInfo.Name }}AppServiceTests : BaseAppServiceTests<{{ ProjectInfo.Name }}EntityFrameworkCoreTestModule>
{
    private readonly I{{ EntityInfo.Name }}AppService _appService;
    private readonly I{{ EntityInfo.Name }}Repository _repository;

    public {{ EntityInfo.Name }}AppServiceTests()
    {
        _appService = GetRequiredService<I{{ EntityInfo.Name }}AppService>();
        _repository = GetRequiredService<I{{ EntityInfo.Name }}Repository>();
        
        //Usando repositório, insira registros no BD para usá-los nos testes.
        var {{ EntityInfo.Name | abp.camel_case }}1 = new {{ EntityInfo.Name }}
        {
            //Coloque valores das propriedades Exemplo:
            //Id = 10
        };
        _repository.InsertAsync({{ EntityInfo.Name | abp.camel_case }}1, true);
    }

    /*
     * Exemplo:
    [Fact]
    public async Task Obter_{{ EntityInfo.Name }}_Por_Id_Deve_Retornar_Corretamente()
    {
        //Arrange
        var id = 10;

        //Act
        var resultado = await _appService.GetAsync(id);

        //Assert
        resultado.ShouldNotBeNull();
        resultado.Id.ShouldBe(id);
    }
    */
}

