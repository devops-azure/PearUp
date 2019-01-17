package com.ggktech.PearUp;

import com.ggktech.utils.TestCaseTemplate;

/**
 * Class file containing test method for failing interest with random name
 *
 */
public class PU_FailingInterest extends TestCaseTemplate {
	public void testScript() {
		appLib.loginPearUp(testEnv, scriptName);
		appLib.failInterest();
		appLib.logoutPearUp();
	}

}
