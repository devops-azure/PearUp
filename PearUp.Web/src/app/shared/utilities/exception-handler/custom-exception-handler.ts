import { NgModule, ErrorHandler } from '@angular/core';

export class CustomExceptionHandler implements ErrorHandler {
    handleError(error) {
        console.error('Web Error-', error);
    }
    call(exception, stackTrace, reason) {
        if (window.console) {
            console.error('Web Exception-', exception, stackTrace, reason);
        }
    }
}
