import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable'
import { Subject } from 'rxjs/Subject';
import { HttpUtility } from '../shared/utilities/http/http-utility.service';
import { Interest } from '../shared/models';

@Injectable()
export class UserInterestsService {

  constructor(private http: HttpUtility, ) {

  }
  GetUserInterests(): Observable<Interest[]> {
    let userInterests: Interest[] = [
      { id: 1, interestDescription: "sample interestDescription 1", interestName: "interestName 1" },
      { id: 2, interestDescription: "sample interestDescription 2", interestName: "interestName 2" },
      { id: 3, interestDescription: "sample interestDescription 3", interestName: "interestName 3" },
      { id: 4, interestDescription: "sample interestDescription 4", interestName: "interestName 4" },
      { id: 5, interestDescription: "sample interestDescription 5", interestName: "interestName 5" },
      { id: 6, interestDescription: "sample interestDescription 6", interestName: "interestName 6" },
      { id: 7, interestDescription: "sample interestDescription 7", interestName: "interestName 7" },
      { id: 8, interestDescription: "sample interestDescription 8", interestName: "interestName 8" },
      { id: 9, interestDescription: "sample interestDescription 1", interestName: "interestName 1" },
      { id: 10, interestDescription: "sample interestDescription 2", interestName: "interestName 2" },
      { id: 11, interestDescription: "sample interestDescription 3", interestName: "interestName 3" },
      { id: 12, interestDescription: "sample interestDescription 4", interestName: "interestName 4" },
      { id: 13, interestDescription: "sample interestDescription 5", interestName: "interestName 5" },
      { id: 14, interestDescription: "sample interestDescription 6", interestName: "interestName 6" },
      { id: 15, interestDescription: "sample interestDescription 7", interestName: "interestName 7" },
      { id: 16, interestDescription: "sample interestDescription 8", interestName: "interestName 8" },
      { id: 17, interestDescription: "sample interestDescription 1", interestName: "interestName 1" },
      { id: 18, interestDescription: "sample interestDescription 2", interestName: "interestName 2" },
      { id: 19, interestDescription: "sample interestDescription 3", interestName: "interestName 3" },
      { id: 20, interestDescription: "sample interestDescription 4", interestName: "interestName 4" },
      { id: 21, interestDescription: "sample interestDescription 5", interestName: "interestName 5" },
      { id: 22, interestDescription: "sample interestDescription 6", interestName: "interestName 6" },
      { id: 23, interestDescription: "sample interestDescription 7", interestName: "interestName 7" },
      { id: 24, interestDescription: "sample interestDescription 8", interestName: "interestName 8" },
      { id: 25, interestDescription: "sample interestDescription 1", interestName: "interestName 1" },
      { id: 26, interestDescription: "sample interestDescription 2", interestName: "interestName 2" },
      { id: 27, interestDescription: "sample interestDescription 3", interestName: "interestName 3" },
      { id: 28, interestDescription: "sample interestDescription 4", interestName: "interestName 4" },
      { id: 29, interestDescription: "sample interestDescription 5", interestName: "interestName 5" },
      { id: 30, interestDescription: "sample interestDescription 6", interestName: "interestName 6" },
      { id: 31, interestDescription: "sample interestDescription 7", interestName: "interestName 7" },
      { id: 32, interestDescription: "sample interestDescription 8", interestName: "interestName 8" },
    ]
    return Observable.of(userInterests);
  }
}
