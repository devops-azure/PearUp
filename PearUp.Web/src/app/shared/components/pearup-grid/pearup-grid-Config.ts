export class PearupGridConfig {
    editable: boolean;
    rowsPerPage: number;
    filter: boolean = true;
    lazy: boolean = false;
    totalRecords: number;
    cellConfig: Array<PearupGridCellConfig>;
    pagination:boolean;
    pageLinks:number;
    filterPlaceholderText:string;
  }
  
  export class PearupGridCellConfig {
    field:string;
    header:string;
    editable: boolean;
    center:boolean=false
  }