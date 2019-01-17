package com.ggktech.resultobjects;

import org.codehaus.jackson.annotate.JsonProperty;

public class TestStep {
	
	private String No;
	private String Name;
	private String Description;
	private String StepStatus;
	private String DateTime;
	private String Screenshot;
	
	@JsonProperty("No")
	public String getNo() {
		return No;
	}
	@JsonProperty("No")
	public void setNo(String no) {
		No = no;
	}
	
	@JsonProperty("Name")
	public String getName() {
		return Name;
	}
	@JsonProperty("Name")
	public void setName(String name) {
		Name = name;
	}
	
	@JsonProperty("Description")
	public String getDescription() {
		return Description;
	}
	@JsonProperty("Description")
	public void setDescription(String description) {
		Description = description;
	}
	
	@JsonProperty("StepStatus")
	public String getStepStatus() {
		return StepStatus;
	}
	@JsonProperty("StepStatus")
	public void setStepStatus(String stepStatus) {
		StepStatus = stepStatus;
	}
	
	@JsonProperty("DateTime")
	public String getDateTime() {
		return DateTime;
	}
	@JsonProperty("DateTime")
	public void setDateTime(String dateTime) {
		DateTime = dateTime;
	}
	
	@JsonProperty("ScreenShot")
	public String getScreenshot() {
		return Screenshot;
	}
	@JsonProperty("ScreenShot")
	public void setScreenshot(String screenshot) {
		Screenshot = screenshot;
	}
	
	
}
