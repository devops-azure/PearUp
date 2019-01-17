import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    Validators
} from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import {
    animate,
    group,
    style,
    transition,
    trigger
} from '@angular/animations';
import {
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    ViewEncapsulation
} from '@angular/core';
import { GlobalEventsManager } from '../shared/utilities/global-events-manager';
import { loginAnimation } from '../shared/utilities/animations/shared-animations';
import { ResponseModel, User, AuthUser, AuthToken } from '../shared/models';
import { UserService } from '../shared/services';
import { AlertService, AuthenticationService } from '../shared/services';
import {AppConstants} from '../app.constants'

@Component({
    selector: 'app-login',
    encapsulation: ViewEncapsulation.Emulated,
    templateUrl: './login.component.html',
    styleUrls: ['./login.scss'],
    animations: [
        loginAnimation
    ]
})
export class LoginComponent {
    public loginform: FormGroup;
    public email: AbstractControl;
    public password: AbstractControl;
    public model: any = {};
    public loading = false;
    public returnUrl: string;
    public currentUser: User;
    public displayMessages: any = [];
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private _userService: UserService,
        private formBuilder: FormBuilder,
        private _globalEventsManager: GlobalEventsManager
    ) {
        this.currentUser = this._userService.getCurrentUser();
        // Check whether user is exists or not.
        // If user exists redirect ot dashboard directly, dont show login page
        if (this.currentUser !== null) {
            this.router.navigate([AppConstants.common.dashBoard]);
        }
        // Navigate to Specified URL
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || AppConstants.common.dashBoard;

        if (this.currentUser !== null) {
            this.router.navigate([this.returnUrl]);
        } else {
            this.authenticationService.logout();
        }
    }

    public ngOnInit() {
        if (this.currentUser === null) {
            // prepare the form controls
            this.buildForm();
        }
    }



    private buildForm(): void {

        this.loginform = this.formBuilder.group({
            'email': [null, Validators.compose([Validators.required
            ])],
            'password': [null, Validators.compose([Validators.required
                , Validators.minLength(8), Validators.maxLength(15)
            ])]
        });
        
        this.email = this.loginform.controls['email'];
        this.password = this.loginform.controls['password'];
    }

    public onSubmit(values: Object): void {
        if (this.loginform.valid) {
            // Call Login method, if form is valid
            this.login({
                email: this.loginform.value.email,
                password: this.loginform.value.password
            } as User);
        }
    }

    // Functions & Methods
    public login(loginData: User) {
        // severity: success,info,warn,error
        
        this.displayMessages = [];
        this.loading = true;
        // let currentUser ={email:"tirupathi.temburu@gmail.com",accessToken:"test"} as User;
        // this.router.navigate([AppConstants.common.dashBoard]);
        //         this._userService.setCurrentUser(currentUser);
        // Get Token
        this.authenticationService.verifyUser(loginData).subscribe(authUser => {
            let response: ResponseModel<AuthUser> = authUser as ResponseModel<AuthUser>;
            if (response.isSuccessed) {
                // Store the user in sessionstorage
                let currentUser = response.value.pearUpUser as User;
                let token = response.value.token as AuthToken;
                currentUser.email = currentUser.email;
                currentUser.accessToken = token.value;
                // Store User on the web  storage
                this._userService.setCurrentUser(currentUser);
                this._globalEventsManager.isUserLoggedIn(true);
                this.loading = false;

                // Redirect the user after login
                this.router.navigate([AppConstants.common.dashBoard]);
            } else {
                this.displayMessages.push({ severity: 'error', summary: AppConstants.commonErrors.login_Failed, detail: (response && response.errors) ? response.errors[0] : AppConstants.commonErrors.api_Response_Failed })
            }
        },
            (err) => {
                this.loading = false;
                this.displayMessages.push({ severity: 'error', summary: AppConstants.commonErrors.login_Failed, detail:   err.error ? err.error : AppConstants.commonErrors.error_occured_while_login })
            },
            () => {
                this.loading = false;
            }
        )
    }
}
