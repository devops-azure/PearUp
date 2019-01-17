import { browser, by, element } from 'protractor';
import * as $ from 'jquery'

export class PearUpProjectPage {
  navigateTo() {
    return browser.get('/');
  }

  getParagraphText() {
    return element(by.css('app h1')).getText();
  }

  getApp() {
    return element(by.css('app'));
  }

  getEmailTextBox(){
    return element(by.css("#inputEmail"))
  }

  getPasswordTextBox(){
    return element(by.css("#inputPassword"))
  }

  getSigninButton(){
    return element(by.css('.pull-right button')); 
  }

  getEmailRequiredErrorSpan(){
    return element(by.css('#errorEmailRequired'));
  }

  getEmailInvalidErrorSpan(){
    return element(by.css('#errorValidEmail'));
  }
  
  getPasswordRequiredErrorSpan(){
    return element(by.css('#errorPassword'));
  }
}
