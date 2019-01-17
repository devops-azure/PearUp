package com.ggktech.salesCRM;

import com.ggktech.utils.TestCaseTemplate;

/**
 * Class file containing test method for creating lead.
 */
public class TS_CreatingLead extends TestCaseTemplate {

	public void testScript() {
		appLib.loginCRM(testEnv, scriptName);
		appLib.navigationToLead();
		appLib.createLead(testEnv, scriptName);
		appLib.navigatingSearchLead();
		appLib.searchLeadDetails();
		appLib.deleteLead();
		appLib.logoutSalesCrm();
	}
}
