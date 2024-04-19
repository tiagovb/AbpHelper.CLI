{{- SKIP_GENERATE = Option.ReadOnlyAppServices || Option.SkipDomainManager -}}
using {{ProjectInfo.FullName}}.Domain;

namespace {{ EntityInfo.Namespace }};

public class {{ EntityInfo.Name }}Manager : BaseDomainManager<{{ EntityInfo.Name }}>, I{{ EntityInfo.Name }}Manager
{
    public {{ EntityInfo.Name }}Manager(I{{ EntityInfo.Name }}Repository repository) : base(repository)
    {
    }
}
