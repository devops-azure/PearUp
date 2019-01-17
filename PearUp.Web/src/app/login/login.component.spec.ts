import { ComponentFixture } from '@angular/core/testing';
import { LoginComponent } from './login.component'
import { AlertService, AuthenticationService } from '../shared/services';
import { from } from 'rxjs/observable/from';
import { routing } from './login.routing';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MessagesModule } from 'primeng/primeng';
import { NgaModule } from '../layout/nga.module';
import { NgModule } from '@angular/core';
import { RouterModule, Routes, ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { getBaseTestBed } from '../shared/utilities/test-helper.spec'
import { UserService } from '../shared/services';
import { User, AuthUser, AuthToken, ResponseModel } from '../shared/models/index';
import { Subject } from 'rxjs/Subject';

describe("Component: Login", () => {

    let component: LoginComponent;
    let fixture: ComponentFixture<LoginComponent>;
    let authService: AuthenticationService;
    let userService: UserService;
    let currentUser: User;
    beforeEach(() => {

        // refine the test module by declaring the test component

        let imports = [
            CommonModule,
            ReactiveFormsModule,
            FormsModule,
            NgaModule,
            MessagesModule,
            routing,
            RouterModule,
            RouterTestingModule
        ];
        let declarations = [
            LoginComponent,
        ];
        let providers = [AuthenticationService]

        let TestBed = getBaseTestBed(imports, providers, declarations);

        fixture = TestBed.createComponent(LoginComponent);
        component = fixture.componentInstance;
        authService = TestBed.get(AuthenticationService);
        userService = TestBed.get(UserService);
        let user = {
            email: "admin@pearup.com",
            isAdmin: true,
            name: "admin",
            id: 100,
            fullName: "Admin",
            phoneNumber: "",
            status: "",
            password: "",
            accessToken: ""
        }
        currentUser = user as User;
    });


    it("Should create login app", () => {
        expect(fixture).toBeTruthy();
    });

    it("Should Current user null before Login", () => {
        let user = userService.getCurrentUser();
        expect(user).toBeNull();
    })

    it("Should Current user null before Login", () => {
        let user = userService.getCurrentUser();
        expect(user).toBeNull();
    })

    it("Should able to set login User", () => {
        userService.setCurrentUser(currentUser);
        let user = userService.getCurrentUser();
        expect(user).not.toBeNull();
    })

    it("Should able to logout Current user", () => {
        userService.removeCurrentUser();
        let user = userService.getCurrentUser();
        expect(user).toBeNull();
    })
    it("Should able to login with valid credentials", () => {
        userService.removeCurrentUser();
        let response = new ResponseModel<AuthUser>();
        let authUser = new AuthUser();
        authUser.pearUpUser = new User();
        authUser.pearUpUser.email = "admin@pearup.com",
            authUser.pearUpUser.isAdmin = true,
            authUser.pearUpUser.fullName = "admin",
            authUser.pearUpUser.id = 100,
            authUser.pearUpUser.fullName = "Admin",
            authUser.pearUpUser.phoneNumber = "",
            authUser.pearUpUser.status = "",
            authUser.pearUpUser.password = "",
            authUser.pearUpUser.accessToken = ""
        authUser.token = new AuthToken();
        authUser.token.validTo = new Date();
        authUser.token.value = "tyeeyye asysy asysy";
        response.value = authUser;
        response.isSuccessed = true;
        let curentUser = new Subject<ResponseModel<AuthUser>>();
        curentUser.next(response);
        spyOn(authService, 'verifyUser').and.returnValue(curentUser.asObservable());
        component.login({
            email: "admin@pearup.com",
            password: '123456'
        } as User)
        let user = userService.getCurrentUser();
        expect(user).toBeNull();
    })
});
