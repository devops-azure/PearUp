import { HttpUtility } from '../../../shared/utilities/http/http-utility.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

@Injectable()
export class GgkSidebarService {
    constructor(private http: HttpUtility) { }
    getMenuItems()
    {
        return this.http.get('http://localhost:65350/api/menu')
            .map((response: Response) => response.json());
    }
}