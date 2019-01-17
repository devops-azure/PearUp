import {
  Directive,
  ElementRef,
  EventEmitter,
  Input,
  Output
  } from '@angular/core';
import './slim-scroll.loader.ts';

@Directive({
  selector: '[ggkSlimScroll]'
})
export class SlimScrollDirective {

  @Input() public ggkSlimScrollOptions: Object;

  constructor(private _elementRef: ElementRef) {
  }

  ngOnChanges(changes) {
    this._scroll();
  }

  private _scroll() {
    this._destroy();
    this._init();
  }

  private _init() {
    jQuery(this._elementRef.nativeElement).slimScroll(this.ggkSlimScrollOptions);
  }

  private _destroy() {
    jQuery(this._elementRef.nativeElement).slimScroll({ destroy: true });
  }
}
