import {
  APP_INITIALIZER,
  ApplicationRef,
  enableProdMode,
  ErrorHandler
} from '@angular/core';
import { Config } from './shared/utilities/config';
import { CustomExceptionHandler } from
  './shared/utilities/exception-handler/custom-exception-handler';
import { disableDebugTools, enableDebugTools } from '@angular/platform-browser';
import { GlobalEventsManager } from './shared/utilities/global-events-manager';
import { PearUpAuthGuardService } from './shared/services/pearUpAuthGuard.service';
// Angular 2
// CustomExceptionHandler


(<any>window).GlobalEventsManager = new GlobalEventsManager();

// Environment Providers
let PROVIDERS: any[] = [
  // common env directives
  { provide: ErrorHandler, useClass: CustomExceptionHandler },
  PearUpAuthGuardService,
  GlobalEventsManager,
  {
    provide: APP_INITIALIZER, useValue: () => {
      return (<any>window).GlobalEventsManager;
    }, multi: true
  },
  Config,
  {
    provide: APP_INITIALIZER,
    useFactory: (config: Config) => () => config.load(),
    deps: [Config],
    multi: true
  }
];

// Angular debug tools in the dev console
// https://github.com/angular/angular/blob/86405345b781a9dc2438c0fbe3e9409245647019/TOOLS_JS.md
let _decorateModuleRef = function identity<T>(value: T): T { return value; };

if ('production' === ENV || 'renderer' === ENV) {
  // Production
  disableDebugTools();
  enableProdMode();

  PROVIDERS = [
    ...PROVIDERS,
    // custom providers in production
  ];

} else {

  _decorateModuleRef = (modRef: any) => {
    const appRef = modRef.injector.get(ApplicationRef);
    const cmpRef = appRef.components[0];

    let _ng = (<any>window).ng;
    enableDebugTools(cmpRef);
    (<any>window).ng.probe = _ng.probe;
    return modRef;
  };

  // Development
  PROVIDERS = [
    ...PROVIDERS,
    // custom providers in development
  ];

}

export const decorateModuleRef = _decorateModuleRef;

export const ENV_PROVIDERS = [
  ...PROVIDERS
];
