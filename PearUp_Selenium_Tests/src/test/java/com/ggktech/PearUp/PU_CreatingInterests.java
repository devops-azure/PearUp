package com.ggktech.PearUp;

import com.ggktech.utils.TestCaseTemplate;

/**
 * Class file containing test method for creating interests.
 */
public class PU_CreatingInterests extends TestCaseTemplate {
	public void testScript() {
		appLib.loginPearUp(testEnv, scriptName);
		appLib.createInterests();
		appLib.logoutPearUp();
	}
}
