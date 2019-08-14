def ProjectName = "PearUp"
def EmailRecipients = "siva.kondabathini@ggktech.com"
def checkOutInformation

node('BuildNode')
{
  stage('GitHub Pull') {
    try{
      // Get some code from GitHub repository
      checkOutInformation = git credentialsId: '444d8b75-cf7c-4c8d-a8dc-bbcc321fed03', url: 'https://github.com/sivasankarkondabathini/PearUp.git'
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "GitHub Pull", checkOutInformation)
        throw error
    }
   }

   stage('Build & Test') {
    try{
      // Build app using Docker image
      sh '''
        echo "Build"
        '''
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "Docker Build", checkOutInformation)
        throw error
    }
   }

   stage('Code Analysis: SonarQube') {
    try{
      // Check SonarQube project status
      sh '''#!/bin/bash
      echo "Code coverage"
      '''
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "SonarQube Analysis", checkOutInformation)
        throw error
    }
   }
   stage('Deploy to Dev') {
    try{
      // Check SonarQube project status
      sh '''#!/bin/bash
      echo "Functional tests"
      '''
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "SonarQube Analysis", checkOutInformation)
        throw error
    }
   }   
   stage('Functional Tests') {
    try{
      // Check SonarQube project status
      sh '''#!/bin/bash
      echo "Functional tests"
      '''
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "SonarQube Analysis", checkOutInformation)
        throw error
    }
   }
   stage('Integration Tests') {
    try{
      // Check SonarQube project status
      sh '''#!/bin/bash
      echo "Integration tests"
      '''
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "SonarQube Analysis", checkOutInformation)
        throw error
    }
   }
   stage('Publish Image') {
    try{
      // Publish Docker images to Nexus Docker Registry
      sh '''
        echo "Publish"
        '''
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "Publish to Nexus Artifactory", checkOutInformation)
        throw error
    }
   }
   stage('Deploy to Staging') {
    try{
      // Publish Docker images to Nexus Docker Registry
      sh '''
        echo "Staging"
        '''
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "Publish to Nexus Artifactory", checkOutInformation)
        throw error
    }
   } 
   stage('Deploy to Production') {
    try{
      // Publish Docker images to Nexus Docker Registry
      sh '''
        echo "Prod"
        '''
    }
    catch(error){
        SendMail(ProjectName, EmailRecipients, "FAILED", "Publish to Nexus Artifactory", checkOutInformation)
        throw error
    }
   }   
}

def SendMail(ProjectName, EmailRecipients, BuildStatus, FailedStage, checkOutInformation){

    //Configuring status color
    def statusColor = "red"
    if("${BuildStatus}" == "SUCCESS"){
        statusColor = "green"
    }

    //Configuring Email Body
    def emailBody = """<table style='border: 1px solid black;border-collapse: collapse;'>
        <tr>
            <td colspan='2' style='background-color:${statusColor}; color:white; text-align:middle;font-size: 20px;white-space: nowrap;padding: 5px;'>
            ${ProjectName} - Build #${BUILD_NUMBER} - ${BuildStatus}</td>
        </tr>
        <tr>
            <td style='font-weight:bold; white-space: nowrap; padding: 5px;'>Jenkins Job Name</td>
            <td>: ${JOB_NAME}</td>
        </tr>
        <tr style='background-color: #f2f2f2'>
            <td style='font-weight:bold; white-space: nowrap; padding: 5px;'>Build Status </td>
            <td>: ${BuildStatus}</td>
        </tr>
        <tr>
            <td style='font-weight:bold; white-space: nowrap; padding: 5px;'>Console Output</td>
            <td>: <a href="${BUILD_URL}/console">Click here</a></td>
        </tr>
        <tr>
            <td style='font-weight:bold; white-space: nowrap; padding: 5px;'>GIT Branch</td>
            <td>: ${checkOutInformation.GIT_BRANCH}</td>
        </tr>
        <tr style='background-color: #f2f2f2'>
            <td style='font-weight:bold; white-space: nowrap; padding: 5px;'>GIT Commit SHA</td>
            <td>: ${checkOutInformation.GIT_COMMIT}</td>
        </tr>
        <tr>
            <td style='font-weight:bold; white-space: nowrap; padding: 5px;'>GIT URL</td>
            <td>: <a href='${checkOutInformation.GIT_URL}'>Click here</a></td>
        </tr>"""

    //Adding failed stage to email body
    if("${BuildStatus}" == "FAILED"){
        emailBody = emailBody + """
        <tr style='background-color: #f2f2f2'>
            <td style='font-weight:bold; white-space: nowrap; padding: 5px;'>Failed Stage</td>
            <td>: ${FailedStage}</td>
        </tr>
        """
    }
    emailBody = emailBody + """</table>"""
    def emailSubject = "GGK - ${ProjectName} Build Status"

    //sending email
    emailext body: emailBody,
    attachLog: true,
    subject: emailSubject,
    mimeType: 'text/html',
    to: EmailRecipients
}

