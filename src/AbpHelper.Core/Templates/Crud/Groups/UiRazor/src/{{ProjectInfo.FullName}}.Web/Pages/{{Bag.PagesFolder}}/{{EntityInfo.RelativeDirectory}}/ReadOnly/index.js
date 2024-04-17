{{~ if Bag.PagesFolder; pagesFolder = Bag.PagesFolder + "/"; end ~}}
$(function () {
{{~ if !Option.SkipGetListInputDto ~}}

    $("#{{ EntityInfo.Name }}Filter :input").on('input', function () {
        dataTable.ajax.reload();
    });

    const getFilter = function () {
        const input = {};
        $("#{{ EntityInfo.Name }}Filter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/{{ EntityInfo.Name }}Filter./g, ''))] = data.value;
                }
            })
        return input;
    };
{{~ end ~}}

    const service = {{ EntityInfo.Namespace + '.' + EntityInfo.Name | abp.camel_case }};
    const detalheModal = new abp.ModalManager(abp.appPath + '{{ pagesFolder }}{{ EntityInfo.RelativeDirectory }}/DetalheModal');

    const dataTable = $('#{{ EntityInfo.Name }}Table').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,{{ if !Option.SkipGetListInputDto;"//disable default searchbox"; end}}
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(service.getList{{- if !Option.SkipGetListInputDto;",getFilter"; end-}}),
        columnDefs: [
                        {
                rowAction: {
                    items:
                        [
                            {
                                icon: 'eye',
{{~ if !Option.SkipPermissions ~}}
                                visible: abp.auth.isGranted('{{ ProjectInfo.Name }}.{{ EntityInfo.Name }}.Update'),
{{~ end ~}}
                                action: function (data) {
{{~ if EntityInfo.CompositeKeyName ~}}
                                    detalheModal.open({
    {{~ for prop in EntityInfo.CompositeKeys ~}}
                                    {{ "'"}}{{ EntityInfo.CompositeKeyName }}{{"."}}{{ prop.Name | abp.camel_case}}{{"'"}}: data.record.{{ prop.Name | abp.camel_case}}{{if !for.last}},{{end}}
    {{~ end ~}}
                                    });
{{~ else ~}}
                                    detalheModal.open({ id: data.record.id });
{{~ end ~}}
                                }
                            },
                        ]
                }
            },
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if prop | abp.is_ignore_property || string.starts_with prop.Type "List<" || string.starts_with prop.Type "IList"; continue; end ~}}
            {
                title: "{{ prop.DisplayName != null && prop.DisplayName != '' ? prop.DisplayName : prop.Name }}",
                data: "{{ prop.Name | abp.camel_case }}"
            },
            {{~ end ~}}
        ]
    }));

    detalheModal.onResult(function () {
        dataTable.ajax.reload();
    });
});
