using NSubstitute;
using System.Linq.Expressions;
using {{ ProjectInfo.FullName }};
using {{ ProjectInfo.FullName }}.{{ EntityInfo.RelativeNamespace}};
using Xunit;

namespace {{ ProjectInfo.FullName }}.Tests.{{ EntityInfo.RelativeNamespace}};

public class {{EntityInfo.Name}}ManagerTests : SISPRECDomainTestBase<SISPRECTestBaseModule>
{
    private readonly I{{EntityInfo.Name}}Repository _repository;
    private readonly {{EntityInfo.Name}}Manager _manager;

    public {{EntityInfo.Name}}ManagerTests()
    {
        _repository = Substitute.For<I{{EntityInfo.Name}}Repository>();
        _manager = new {{EntityInfo.Name}}Manager(_repository);
    }

    private {{EntityInfo.Name}} Inicializar{{EntityInfo.Name}}ComValoresValidos()
    {
        return new {{EntityInfo.Name}}
        {
            {{~ for prop in EntityInfo.Properties ~}}
            {{ prop.Name }} = ({{prop.Type}})"",//preencher com valores;
            {{~ end ~}}
        };
    }

    [Fact]
    public async Task Deletar_Assincronamente()
    {
        var objeto = Inicializar{{EntityInfo.Name}}ComValoresValidos();
        await _manager.ExcluirAsync(objeto);
        await _repository.Received(1).DeleteAsync(objeto, Arg.Any<bool>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Deletar_Com_Predicado()
    {
        Expression<Func<{{EntityInfo.Name}}, bool>> predicate = {{ EntityInfo.Name | abp.camel_case }} => {{ EntityInfo.Name | abp.camel_case }}.{{EntityInfo.CompositeKeys[0].Name}} == 1;//AJUSTAR ID
        await _manager.ExcluirAsync(predicate);
        await _repository.Received(1).DeleteAsync(predicate, Arg.Any<bool>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Deletar_Direto_Com_Predicado()
    {
        Expression<Func<{{EntityInfo.Name}}, bool>> predicate = {{ EntityInfo.Name | abp.camel_case }} => {{ EntityInfo.Name | abp.camel_case }}.{{EntityInfo.CompositeKeys[0].Name}} == 1;
        await _manager.ExcluirDiretoAsync(predicate);
        await _repository.Received(1).DeleteDirectAsync(predicate, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Deletar_Muitos_Assincronamente()
    {
        var objeto = Inicializar{{EntityInfo.Name}}ComValoresValidos();
        var objetos = new List<{{EntityInfo.Name}}> { objeto }; 
        await _manager.ExcluirMuitosAsync(objetos);
        await _repository.Received(1).DeleteManyAsync(objetos, Arg.Any<bool>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Inserir_Muitos_Assincronamente()
    {
        var objeto = Inicializar{{EntityInfo.Name}}ComValoresValidos();
        var objetos = new List<{{EntityInfo.Name}}> { objeto };
        await _manager.InserirMuitosAsync(objetos);
        await _repository.Received(1).InsertManyAsync(objetos, Arg.Any<bool>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Atualizar_Muitos_Assincronamente()
    {
        var objeto = Inicializar{{EntityInfo.Name}}ComValoresValidos();
        var objetos = new List<{{EntityInfo.Name}}> { objeto };
        await _manager.AlterarMuitosAsync(objetos);
        await _repository.Received(1).UpdateManyAsync(objetos, Arg.Any<bool>(), Arg.Any<CancellationToken>());
    }
}
