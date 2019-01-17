export class PrimeGridData {
    data: any[];
    columns: any[];
    //columns: PrimeGridColumn[];
    isLazyLoading: boolean;

    contextMenuItems: any[];
    selectionMode: string = 'single';
    defaultSortField: string;
    //  Sort order as number, 1 for asc and -1 for dec
    defaultSortOrder: number = 1;
}

export class PrimeGridColumn {
    //	Property of a row data.
    field: string = null;
    //	Property of a row data used for sorting, defaults to field.
    sortField: string = null;
    //	Header text of a column.
    header: string = null;
    //	Footer text of a column.
    footer: string = null;
    //	Defines if a column is sortable.
    sortable: any = false;
    //	Sort function for custom sorting.
    sortFunction: Function = null;
    //	Defines if a column is editable.
    editable: boolean = false;
    //	Defines if a column can be filtered.
    filterable: boolean = false;
    //	Defines filterMatchMode; "startsWith", "contains", "endsWidth", "equals" and "in".
    filterMatchMode: string = null;
    //	Type of the filter input field.
    filterType: string = "text";
    //	Defines placeholder of the input fields.
    filterPlaceholder: string = null;
    //	Number of rows to span for grouping.
    rowspan: string = null;
    //	Number of columns to span for grouping.
    colspan: string = null;
    //	Inline style of the column.
    style: string = null;
    //	Style class of the column.
    styleClass: string = null;
    //	Inline style of the table element.
    tableStyle: string = null;
    //	Style class of the table element.
    tableStyleClass: string = null;
    //	Controls visiblity of the column.
    hidden: boolean = false;
    //	Displays an icon to toggle row expansion.
    expander: boolean = false;
    //	Defines column based selection mode, options are "single" and "multiple".
    selectionMode: string = null;
    //	Whether the column is fixed in horizontal scrolling or not.
    frozen: boolean = false;
}