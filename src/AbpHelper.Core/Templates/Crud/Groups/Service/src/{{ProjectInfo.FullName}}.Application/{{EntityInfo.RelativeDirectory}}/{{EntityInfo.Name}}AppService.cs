{{-
if Option.SkipCustomRepository
    if EntityInfo.CompositeKeyName
        repositoryType = "IRepository<" + EntityInfo.Name + ">"
    else
        repositoryType = "IRepository<" + EntityInfo.Name + ", " + EntityInfo.PrimaryKey + ">"
    end
    repositoryName = "Repository"
else
    repositoryType = "I" + EntityInfo.Name + "Repository"
    repositoryName = "_repository"
end ~}}
using System;
{{~ if Option.ReadOnlyAppServices
    crudClassName = "BaseReadOnlyAppService"
else if  Option.DomainManager
    crudClassName = "BaseCrudAppService"
else
    crudClassName = "AbstractKeyCrudAppService"
end ~}}
{{~ if EntityInfo.CompositeKeyName || !Option.SkipGetListInputDto~}}
using System.Linq;
using System.Threading.Tasks;
{{~ end -}}
{{~ if !Option.SkipPermissions
    permissionNamesPrefix = ProjectInfo.Name + "Permissions." + EntityInfo.Name
~}}
using {{ ProjectInfo.FullName }}.Permissions;
{{~ end ~}}
using {{ EntityInfo.Namespace }}.Dtos;
{{~ if Option.SkipGetListInputDto ~}}
using Volo.Abp.Application.Dtos;
{{~ end ~}}
using Volo.Abp.Application.Services;
{{~ if Option.SkipCustomRepository ~}}
using Volo.Abp.Domain.Repositories;
{{~ end ~}}

namespace {{ EntityInfo.Namespace }};

{{~
    if !Option.SkipGetListInputDto
        TGetListInput = EntityInfo.Name + "GetListInput"
    else
        TGetListInput = "PagedAndSortedResultRequestDto"
end ~}}

{{~ if EntityInfo.Document | !string.whitespace ~}}
/// <summary>
/// {{ EntityInfo.Document }}
/// </summary>
{{~ end ~}}
{{~ if !Option.ReadOnlyAppServices ~}}
public class {{ EntityInfo.Name }}AppService: {{ crudClassName }}<{{ EntityInfo.Name }}, {{ DtoInfo.ReadTypeName }}, {{ EntityInfo.PrimaryKey ?? EntityInfo.CompositeKeyName }}, {{ TGetListInput}}, {{ DtoInfo.CreateTypeName }}, {{ DtoInfo.UpdateTypeName }}>,
    I{{ EntityInfo.Name }}AppService
{{~ end ~}}
{{~ if Option.ReadOnlyAppServices ~}}
public class {{ EntityInfo.Name }}AppService: {{ crudClassName }}<{{ EntityInfo.Name }}, {{ DtoInfo.ReadTypeName }}, {{ EntityInfo.PrimaryKey ?? EntityInfo.CompositeKeyName }}, {{ TGetListInput}}>, I{{ EntityInfo.Name }}AppService
{{~ end ~}}
{
    {{~ if !Option.SkipPermissions ~}}
    protected override string GetPolicyName { get; set; } = {{ permissionNamesPrefix }}.Default;
    protected override string GetListPolicyName { get; set; } = {{ permissionNamesPrefix }}.Default;
    protected override string CreatePolicyName { get; set; } = {{ permissionNamesPrefix }}.Create;
    protected override string UpdatePolicyName { get; set; } = {{ permissionNamesPrefix }}.Update;
    protected override string DeletePolicyName { get; set; } = {{ permissionNamesPrefix }}.Delete;
    {{~ end ~}}
    
    {{~ if !Option.ReadOnlyAppServices && Option.DomainManager ~}}
    private readonly I{{ EntityInfo.Name }}Manager _manager;
    {{~ end ~}}
    {{~ if !Option.SkipCustomRepository ~}}
    private readonly {{ repositoryType }} {{ repositoryName }};

    public {{ EntityInfo.Name }}AppService({{ repositoryType }} repository{{~ if !Option.ReadOnlyAppServices && Option.DomainManager ~}},I{{ EntityInfo.Name }}Manager manager{{~ end ~}}) : base(repository{{~ if !Option.ReadOnlyAppServices && Option.DomainManager ~}},manager{{~ end ~}})
    {
        {{ repositoryName }} = repository;
        {{~ if !Option.ReadOnlyAppServices && Option.DomainManager ~}}
        _manager = manager;
        {{~ end ~}}
    }
    {{~ else ~}}
    public {{ EntityInfo.Name }}AppService({{ repositoryType }} repository{{~ if !Option.ReadOnlyAppServices && Option.DomainManager ~}},I{{ EntityInfo.Name }}Manager manager{{~ end ~}}) : base(repository{{~ if !Option.ReadOnlyAppServices && Option.DomainManager ~}},manager{{~ end ~}})
    {
        {{~ if !Option.ReadOnlyAppServices && Option.DomainManager ~}}
        _manager = manager;
        {{~ end ~}}
    }
    {{~ end ~}}

    {{~ if !Option.ReadOnlyAppServices ~}}
    protected override Task DeleteByIdAsync({{ EntityInfo.CompositeKeyName }} id)
    {
        return {{ repositoryName }}.DeleteAsync(e =>
        {{~ if EntityInfo.CompositeKeys.Count > 1 ~}}
        {{~ for prop in EntityInfo.CompositeKeys ~}}
            e.{{ prop.Name }} == id.{{ prop.Name}}{{ if !for.last}} &&{{end}}
        {{~ end ~}}
        {{~ else ~}}
            e.{{ EntityInfo.CompositeKeys[0].Name }} == id
        {{~ end ~}}
        );
    }
    {{~ end ~}}

    protected override async Task<{{ EntityInfo.Name }}> GetEntityByIdAsync({{ EntityInfo.CompositeKeyName }} id)
    {
        return await AsyncExecuter.FirstOrDefaultAsync(
            (await {{ repositoryName }}.WithDetailsAsync()).Where(e =>
            {{~ if EntityInfo.CompositeKeys.Count > 1 ~}}
            {{~ for prop in EntityInfo.CompositeKeys ~}}
                e.{{ prop.Name }} == id.{{ prop.Name}}{{ if !for.last}} &&{{end}}
            {{~ end ~}}
            {{~ else ~}}
                e.{{ EntityInfo.CompositeKeys[0].Name }} == id
            {{~ end ~}}
            ));
    }

    protected override IQueryable<{{ EntityInfo.Name }}> ApplyDefaultSorting(IQueryable<{{ EntityInfo.Name }}> query)
    {
        return query.OrderBy(e => e.{{ EntityInfo.CompositeKeys[0].Name }});
    }

    {{~ if !Option.SkipGetListInputDto ~}}
    protected override async Task<IQueryable<{{ EntityInfo.Name }}>> CreateFilteredQueryAsync({{ EntityInfo.Name }}GetListInput input)
    {
        return (await base.CreateFilteredQueryAsync(input))
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if (prop | abp.is_ignore_property) || string.starts_with prop.Type "List<"; continue; end ~}}
            {{~ if prop.Type == "string" ~}}
            .WhereIf(!input.{{ prop.Name }}.IsNullOrWhiteSpace(), x => x.{{ prop.Name }}.Contains(input.{{ prop.Name }}))
            {{~ else ~}}
            .WhereIf(input.{{ prop.Name }} != null, x => x.{{ prop.Name }} == input.{{ prop.Name }})
            {{~ end ~}}
            {{~ end ~}}
            ;
    }
    {{~ end ~}}
}
