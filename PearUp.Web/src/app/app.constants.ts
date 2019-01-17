const User = {
    currentUser:"currentUser"
}
const Common = {
    loginUrl: "/asdfasdfasdf",
    dashBoard:"/dashboard"
}
const CommonErrors={
    login_Failed:"Login failed",
    api_Response_Failed:"API Response failed",
    error_occured_while_login:"Error occured while login",
    delete_confirmation:"Are you sure you want to delete this item?",
}
export const UserInterests={
    maxRows:10,
    pageLinks:3,
    filterPlaceholderText:"Search interests eg. Cricket",
}
export const AppConstants = {
    common: { ...Common },
    user: { ...User },
    commonErrors:{...CommonErrors},
    userInterests:{...UserInterests}
}
 