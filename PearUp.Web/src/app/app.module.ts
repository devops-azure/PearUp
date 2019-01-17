import {
  AlertService,
  AuthenticationService,
  PearUpAuthGuardService,
  UserService
  } from './shared/services';
import { APP_BASE_HREF } from '@angular/common';
import {
  APP_INITIALIZER,
  ApplicationRef,
  ErrorHandler,
  NgModule
  } from '@angular/core';
import { AppComponent } from './app.component';
import { AppState, InternalStateType } from './app.service';
import { AuthInterceptor } from './shared/utilities/http/auth-interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { Config } from './shared/utilities/config';
import { CustomExceptionHandler } from './shared/utilities/exception-handler/custom-exception-handler';
import { ErrorPageComponent } from './layout/components/error-page/error-page.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GlobalEventsManager } from './shared/utilities/global-events-manager';
import { GlobalState } from './global.state';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HttpUtility } from './shared/utilities/http/http-utility.service';
import { NgaModule } from './layout/nga.module';
import { RouterModule } from '@angular/router';
import { routing } from './app.routing';
import { CookieService } from 'ngx-cookie-service';
/*
 * Platform and Environment providers/directives/pipes
 */

// App is our top level component


// Application wide providers
const APP_PROVIDERS = [
  AppState,
  GlobalState
];

export type StoreType = {
  state: InternalStateType,
  restoreInputValues: () => void,
  disposeOldHosts: () => void
};

export function moduleResolveFactory(config: Config) {
  return () => config.load();
}
export function sessionResolveFactory() {
  return sessionStorage;
}

export function moduleResolveFactoryGem(globalEvents: GlobalEventsManager) {
  return () => globalEvents;
}


/**
 * `AppModule` is the main entry point into Angular2's bootstraping process
 */
@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    ErrorPageComponent,
  ],
  imports: [ // import Angular's modules
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    NgaModule.forRoot(),
    routing
  ],
  providers: [ // expose our Services and Providers into Angular's dependency injection
    APP_PROVIDERS,
    { provide: APP_BASE_HREF, useValue: '/' },
    { provide: ErrorHandler, useClass: CustomExceptionHandler },
    {
      provide: 'Store',
      useFactory: sessionResolveFactory,
    },
    PearUpAuthGuardService,
    AlertService,
    GlobalEventsManager,
    {
      provide: APP_INITIALIZER,
      useFactory: moduleResolveFactoryGem,
      deps: [GlobalEventsManager],
      multi: true
    },
    Config,
    {
      provide: APP_INITIALIZER,
      useFactory: moduleResolveFactory,
      deps: [Config],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    HttpUtility,
    UserService,
    AuthenticationService,
    CookieService
  ]
})

export class AppModule {

  constructor(public appState: AppState) {
  }
}
