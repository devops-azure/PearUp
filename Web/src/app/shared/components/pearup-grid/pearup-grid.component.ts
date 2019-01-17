import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';
import * as _ from 'lodash';
import { PearupGridConfig } from './pearup-grid-Config';
@Component({
  selector: 'pearup-grid',
  templateUrl: './pearup-grid.component.html',
  styleUrls: ['./pearup-grid.component.css']
})
export class PearupGridComponent implements OnInit {

  @Input() datasource: any;
  @Input() config: PearupGridConfig;
  // @Input() isExpandedOnHover: boolean = false;
  // @Output() expandMenu = new EventEmitter<any>();
  constructor() { }

  ngOnInit() {
    console.log(this.datasource);
    console.log(this.config);
  }
  onEdit(event: any): void {
    // console.log('onEdit -', JSON.stringify(event));
    if (!event.data.enabled || event.data.enabled == false)
      event.data.enabled = true
  }

  onEditComplete(event: any): void {
    console.log('onEditComplete -', JSON.stringify(event));
  }

  onUpdateRow($event, item): void {
    console.log('onUpdateRow -', JSON.stringify(item));
  }

  onAddRow($event: any): void {
    if (this.datasource && this.datasource!=null && typeof Object.keys === "function") {
      var newItem = {};
      _.each(Object.keys(this.datasource[0]), (key) => {
        newItem[key] = "";
      });
      this.datasource.push(newItem);
      this.datasource={...this.datasource};
    }
  }
}
