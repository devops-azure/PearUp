export class ResponseModel<T> {
    isSuccessed: boolean;
    status: string;
    messages: any;
    value: T;
    errors:any;
}
