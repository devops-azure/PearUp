package com.ggktech.PearUp;

import com.ggktech.utils.TestCaseTemplate;

/**
 * Class file containing test method for verifying pear up menu list items
 *
 */
public class PU_MenuListItems extends TestCaseTemplate {
	public void testScript() {
		appLib.loginPearUp(testEnv, scriptName);
		appLib.verifyMenuListItems();
		appLib.logoutPearUp();
	}
}
