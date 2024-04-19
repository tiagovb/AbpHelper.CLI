{{~ if Bag.PagesFolder; pagesFolder = Bag.PagesFolder + "/"; end ~}}
$(function () {
{{~ if !Option.SkipGetListInputDto ~}}

    $("#{{ EntityInfo.Name }}Filter :input").on('input', function () {
        dataTable.ajax.reload();
    });

    //After abp v7.2 use dynamicForm 'column-size' instead of the following settings
    //$('#{{ EntityInfo.Name }}Collapse div').addClass('col-sm-3').parent().addClass('row');

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

    const l = abp.localization.getResource('{{ ProjectInfo.Name }}');

    const service = {{ EntityInfo.Namespace + '.' + EntityInfo.Name | abp.camel_case }};
    const createModal = new abp.ModalManager(abp.appPath + '{{ pagesFolder }}{{ EntityInfo.RelativeDirectory }}/CreateModal');
    const editModal = new abp.ModalManager(abp.appPath + '{{ pagesFolder }}{{ EntityInfo.RelativeDirectory }}/EditModal');

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
                                text: "Alterar",
{{~ if !Option.SkipPermissions ~}}
                                visible: abp.auth.isGranted('{{ ProjectInfo.Name }}.{{ EntityInfo.Name }}.Update'),
{{~ end ~}}
                                action: function (data) {
{{~ if EntityInfo.CompositeKeys.Count > 1 ~}}
                                    editModal.open({
    {{~ for prop in EntityInfo.CompositeKeys ~}}
                                    {{ "'"}}{{ EntityInfo.CompositeKeyName }}{{ "."}}{{ prop.Name | abp.camel_case }}{{ "'"}}: data.record.{{ prop.Name | abp.camel_case }}{{ if !for.last }}, {{ end }}
    {{~ end ~}}
                                    });
{{~ else ~}}
                                    editModal.open({ id: data.record.id });
{{~ end ~}}
                                }
                            },
                            {
                                text: "Excluir",
{{~ if !Option.SkipPermissions ~}}
                                visible: abp.auth.isGranted('{{ ProjectInfo.Name }}.{{ EntityInfo.Name }}.Delete'),
{{~ end ~}}
                                confirmMessage: function (data) {
                                    return "Tem certeza de que deseja excluir?"
                                },
                                action: function (data) {
                                    service.delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyDeleted'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                }
            },
            {{~ for prop in EntityInfo.Properties ~}}
            {{~ if prop | abp.is_ignore_property || string.starts_with prop.Type "List<" || string.starts_with prop.Type "IList" ; continue; end ~}}
            {
                title: "{{ prop.DisplayName ?? prop.Name }}",
                data: "{{ prop.Name | abp.camel_case }}"
            },
            {{~ end ~}}
        ]
    }));

    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#New{{ EntityInfo.Name }}Button').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
