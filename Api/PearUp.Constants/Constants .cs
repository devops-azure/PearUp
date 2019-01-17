namespace PearUp.Constants
{
    public class UserErrorMessages
    {
        public const string PhoneNumber_Or_Password_Is_Incorrect = "Phone Number Or Password Is Incorrect";

        public const string Email_Or_Password_Is_Incorrect = "Email Or Password Is Incorrect";

        public const string Password_Is_Incorrect = "Password Is Incorrect";

        public const string Phone_Number_And_Password_Are_Required = "PhoneNumber and Password are Required";

        public const string EmailAddress_And_Password_Are_Required = "Email Address and Password are Required";

        public const string User_Does_Not_Exist_With_Given_PhoneNumber = "User does not exist with given phone number";

        public const string User_Does_Not_Exist = "User does not exist";

        public const string Phone_Number_Is_Required = "Phone number is required";

        public const string User_Token_Object_Should_Not_Be_null = "User Token Object Should Not Be null";

        public const string Invalid_Details_Provided = "Invalid Details Provided";

        public const string Token_Generation_Failed = "Token Generation Failed";

        public const string User_Registration_Failed = "User Registration Failed";

        public const string Failed_To_Change_Status = "Failed to change User status";

        public const string User_Already_Exists_With_Given_Phone_Number = "User Already Exists With Given Phone Number";

        public const string Error_Occurred_While_Saving_Profile_Photo = "Error occurred while saving profile photo";
    }
    public class CommonErrorMessages
    {
        public const string Request_Is_Not_Valid = "Request Is Not Valid";

        public const string SecretKey_Is_Required = "Secret Key Is Required";

        public const string User_Not_Authenticated = "User Not Authenticated";
    }
    public class Common
    {
        public const string SQLConnectionString = "SQLConnectionString";
        public const string EmailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
    }

    public class AuthConstants
    {
        public const string SecretKey = "pearup-secret-key";

        public const string CleintName = "PearUp";

        public const string UserId = "UserId";

        public const string AdminId = "AdminId";

        public const int ExpiryInMinutes = 30;

        public const string RoleUser = "User";

        public const string PolicyUser = "User";

        public const string PolicyAdmin = "Admin";

        public const string RoleAdmin = "Admin";
    }

    public class PhoneVerifyMessages
    {
        public const string Verification_Code_Sent = "Verification code has been sent Successfully.";
        public const string Verification_Code_Already_Sent = "Verification code has been sent already.";
        public const string Error_Message = "Please check the phone number and try again.";
        public const string Code_Error = "Please send the latest verfication code.";
        public const string Verification_Completed = "Phone number is verified.";

        //Error messages
        public const string Country_Phone_Empty = "Phone number or Country code are empty.";
        public const string Country_Phone_VerifyCode_Empty = "Phone number or Country code or Verification Code are empty.";
        public const string Country_Phone_Invalid = "Phone number or Country code are invalid.";
        public const string Country_Phone_VerifyCode_Invalid = "Phone number or Country code or Verification are invalid.";
        public const string Twilio_Unauthorized = "Something went wrong. Please try later.";

    }
    public class AgeConstants
    {
        public const int MinimumAge = 18;
    }

    public class EmailConstants
    {
        public const string MailSubject = "PearUp Verification Code";
        public const string SMTPHost = "smtp.gmail.com";
        public const int SMTPPort = 587;
    }

    public class MongoCollectionConstants
    {
        public const string User = "User";
    }

    public class InterestErrorMessages
    {
        public const string Interest_Does_Not_Exist = "Interest does not exist";
        public const string Interest_Should_Not_Be_Empty = "Interests can not be empty";
        public const string Error_Occured_While_Getting_Interest = " Error Occured While Getting Interest From DB";
        public const string Error_Occured_While_Inserting_Interest = "Error occured while inserting new Interest to DB.";
        public const string Error_Occured_While_Updating_Interest = "Error occured while updating Interest to DB.";
    }

    public class LoggerMessages
    {
        public const string Oops_Message = "Oops.. something went wrong. Please try after some time.";
    }
}
