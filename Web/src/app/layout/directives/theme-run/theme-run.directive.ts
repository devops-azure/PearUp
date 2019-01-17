import { Directive, HostBinding } from '@angular/core';
import { ThemeConfigProvider, isMobile } from '../../../layout';


@Directive({
    selector: '[ggkThemeRun]'
})
export class ThemeRunDirective {

    @HostBinding('class') classesString: string;
    private _classes: Array<string> = [];

    constructor(private _ggkConfig: ThemeConfigProvider) {
    }

    public ngOnInit(): void {
        this._assignTheme();
        this._assignMobile();
    }

    private _assignTheme(): void {
        this._addClass(this._ggkConfig.get().theme.name);
    }

    private _assignMobile(): void {
        if (isMobile()) {
            this._addClass('mobile');
        }
    }

    private _addClass(cls: string) {
        this._classes.push(cls);
        this.classesString = this._classes.join(' ');
    }
}
