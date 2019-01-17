import { PearUpProjectPage } from './login.po';
import { browser, by, element } from 'protractor';
import {E2eConstants} from '../constants.e2e-spec'
import { protractor } from 'protractor/built/ptor';

describe('pear-up Login', () => {
  let page: PearUpProjectPage;
  let SetEmailPassword: any;
  beforeEach(() => {
    page = new PearUpProjectPage();
    SetEmailPassword = (email, password) => {
      page.getEmailTextBox().clear();
      page.getPasswordTextBox().clear();
      page.getEmailTextBox().sendKeys(email)
      page.getPasswordTextBox().sendKeys(password)
      page.getApp().click();
      browser.sleep(100);
    }
  });

  it('should display welcome message', () => {
    // browser.pause();
    page.navigateTo();
  });

  it('Should Throw Error Email Id and Password is Empty When Email And Password are empty and Login Shoud Disable ', () => {
    //browser.pause();
    // page.navigateTo();
    SetEmailPassword("", "");
    page.getSigninButton().click();
    expect(page.getEmailRequiredErrorSpan().getText()).toEqual("Email is required");
    expect(page.getPasswordRequiredErrorSpan().getText()).toEqual("Password is required");
    expect(page.getSigninButton().getAttribute("disabled")).toBeTruthy();
  });

  it('Should Throw Error Email is Invalid When Email is not valid and Login Shoud Disable', () => {
    //browser.pause();
    // page.navigateTo();
    SetEmailPassword("admin.pearup", "");
    page.getSigninButton().click();
    expect(page.getEmailInvalidErrorSpan().getText()).toEqual("Please enter valid email");
    expect(page.getPasswordRequiredErrorSpan().getText()).toEqual("Password is required");
    expect(page.getSigninButton().getAttribute("disabled")).toBeTruthy();
  });

  it('Should Change login button Enable When Email And Password are valid in format', () => {
    SetEmailPassword(E2eConstants.loginEmail, E2eConstants.loginPassword);
    expect(page.getSigninButton().getAttribute("disabled")).toBeFalsy();
  });

  it('Should Throw Error login Credentials are invalid ', () => {
    SetEmailPassword("test@test.com", "123456789");
    expect(protractor.ExpectedConditions.urlContains("dashboard")).toBeFalsy()
  });

  it('Should navigate to DashBoard When Login details are valid', () => {
    SetEmailPassword(E2eConstants.loginEmail, E2eConstants.loginPassword);
    page.getSigninButton().click();
    browser.sleep(300);
    expect(protractor.ExpectedConditions.urlContains("dashboard")).toBeTruthy()
  });
  
  it('waiter', () => {
    browser.pause();   
    expect(1).toEqual(1);
  });

});

