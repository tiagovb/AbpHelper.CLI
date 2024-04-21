using Shouldly;
using System.Threading.Tasks;
using Xunit;
using {{ ProjectInfo.FullName }}.Samples;
using {{ ProjectInfo.FullName }}.EntityFrameworkCore;

namespace {{ EntityInfo.Namespace }};

{{- 
entityNameLowerCase = EntityInfo.Name | abp.camel_case
entityObj1 = entityNameLowerCase + "Obj1"
entityObj2 = entityNameLowerCase + "Obj2"
-}}
public class {{ EntityInfo.Name }}AppServiceTests : BaseAppServiceTests<{{ ProjectInfo.Name }}EntityFrameworkCoreTestModule>
{
    private readonly I{{ EntityInfo.Name }}AppService _appService;
    private readonly I{{ EntityInfo.Name }}Repository _repository;
    private readonly {{ EntityInfo.Name }} {{ entityObj1 }};
    private readonly {{ EntityInfo.Name }} {{ entityObj2 }};

    public {{ EntityInfo.Name }}AppServiceTests()
    {
        _appService = GetRequiredService<I{{ EntityInfo.Name }}AppService>();
        _repository = GetRequiredService<I{{ EntityInfo.Name }}Repository>();

        {{ entityObj1 }} = new {{ EntityInfo.Name }}
        {
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
            {{~ if prop | abp.is_ignore_property ; continue; end ~}}
            {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name ; continue; end ~}}
            {{ prop.Name }} = ({{prop.Type}})"",//preencher com valores;
            {{~ end ~}}
        };
        _repository.InsertAsync(entityObj1, true);
        
        {{ entityObj2 }} = new {{ EntityInfo.Name }}
        {
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
            {{~ if prop | abp.is_ignore_property ; continue; end ~}}
            {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name ; continue; end ~}}
            {{ prop.Name }} = ({{prop.Type}})"",//preencher com valores;
            {{~ end ~}}
        };
        _repository.InsertAsync(entityObj2, true);
    }

    [Fact]
    public async Task Criar_{{ EntityInfo.Name }}_Deve_Passar()
    {
        // Arrange
        var input = new CreateUpdate{{ EntityInfo.Name }}Dto
        {
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
            {{~ if prop | abp.is_ignore_property ; continue; end ~}}
            {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name ; continue; end ~}}
            {{ prop.Name }} = ({{prop.Type}})"",//preencher com valores;
            {{~ end ~}}
        };

        // Act
        var result = await _appService.CreateAsync(input);

        // Assert
        result.ShouldNotBeNull();
        {{~ for prop in EntityInfo.Properties ~}}
        {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
        {{~ if prop | abp.is_ignore_property ; continue; end ~}}
        result.{{prop.Name}}.ShouldBe(input.{{prop.Name}});
        {{~ end ~}}
    }

    [Fact]
    public async Task Atualizar_{{ EntityInfo.Name }}_Deve_Passar()
    {
        // Arrange
        var objetoOriginal = _repository.GetListAsync().Result.FirstOrDefault();
        var input = new CreateUpdate{{ EntityInfo.Name }}Dto
        {
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
            {{~ if prop | abp.is_ignore_property ; continue; end ~}}
            {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name ; continue; end ~}}
            {{ prop.Name }} = ({{prop.Type}})"",//preencher com valores;
            {{~ end ~}}
        };

        // Act
        var result = await _appService.UpdateAsync(objetoOriginal.{{EntityInfo.CompositeKeys[0].Name}}, input);

        // Assert
        result.ShouldNotBeNull();
        {{~ for prop in EntityInfo.Properties ~}}
        {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
        {{~ if prop | abp.is_ignore_property ; continue; end ~}}
        result.{{prop.Name}}.ShouldBe(input.{{prop.Name}});
        {{~ end ~}}
    }

    [Fact]
    public async Task Excluir_{{ EntityInfo.Name }}_Deve_Passar()
    {
        // Arrange
        var objetoParaExcluir = await _repository.InsertAsync(new {{ EntityInfo.Name }}
        {
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if string.contains(prop.Modifiers, "virtual") ; continue; end ~}}
            {{~ if prop | abp.is_ignore_property ; continue; end ~}}
            {{~ if EntityInfo.CompositeKeys[0].Name == prop.Name ; continue; end ~}}
            {{ prop.Name }} = ({{prop.Type}})"",//preencher com valores;
            {{~ end ~}}
        }, autoSave: true);

        // Act
        await _appService.DeleteAsync(objetoParaExcluir.Id);

        // Assert
        var objetoDeletado = await _repository.FindAsync(a => a.Id == objetoParaExcluir.Id);
        objetoDeletado.ShouldBeNull();
    }

    [Fact]
    public async Task Obter_{{ EntityInfo.Name }}_Por_Id_Deve_Passar()
    {
        // Arrange
        var objetoExistente = await _repository.FindAsync(a => a.{{EntityInfo.CompositeKeys[0].Name}} == {{entityObj1}}.{{EntityInfo.CompositeKeys[0].Name}});

        // Act
        var result = await _appService.GetAsync(objetoExistente.{{EntityInfo.CompositeKeys[0].Name}});

        // Assert
        result.ShouldNotBeNull();
        result.{{EntityInfo.CompositeKeys[0].Name}}.ShouldBe(objetoExistente.{{EntityInfo.CompositeKeys[0].Name}});
    }

    [Fact]
    public async Task Obter_{{ EntityInfo.Name }}_Com_Ordenacao_Padrao_Deve_Passar()
    {
        // Arrange
        var input = new {{ EntityInfo.Name }}GetListInput();  // Sem filtros específicos para testar a ordenação padrão.

        // Act
        var result = await _appService.GetListAsync(input);

        // Assert
        result.Items.ShouldNotBeEmpty(); // Correção aplicada diretamente ao acessar a propriedade Items
        result.Items.OrderBy(l => l.{{EntityInfo.CompositeKeys[0].Name}}).SequenceEqual(result.Items).ShouldBeTrue(); // Corrigido para usar OrderBy e SequenceEqual para verificar a ordem
    }
}

