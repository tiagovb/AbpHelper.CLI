{{- SKIP_GENERATE = Option.ReadOnlyAppServices || Option.SkipDomainManager -}}
using {{ProjectInfo.FullName}}.Domain;

namespace {{ EntityInfo.Namespace }};

public interface I{{ EntityInfo.Name }}Manager : IBaseDomainManager<{{ EntityInfo.Name }}>
{
}
