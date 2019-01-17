import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';


@Injectable()
export class DashBoardService {
    constructor(
        private HttpUtility: HttpUtility) {
    }

    getDashBoards() {
        return this.HttpUtility.get('api/abc');
    }
}
