const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7109';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
      "/api/Account/signUp",
      "/api/Account/login",
      "/api/Account/resetPassword",
      "/api/Account/changePasswordWithURL",
      "/api/Account/validateRecoveryRequestID",
      "/api/EventCounter",
      "/api/EventCounter/getById",
      "/api/EventCounter/getCountersResume",
      "/api/Environment",
      "/api/EventCounter/changeCounterPrivacy",
      "/api/EventCounter/setTimeRefreshCounterUI",
      "/api/EventCounter/editCounterEvent",
      "/api/Profile/getImages",
      "/api/Profile/addImages",
      "/api/Admin/getAllUsers",
      "/api/Account/impersonate",
      "/api/Account/getGoogleClientID",
      "/api/Account/getMicrosoftClientID",
      "/api/Account/registerGoogleAuth",
      "/api/Account/registerMicrosoftAuth",
      "/api/Account/loginWithGoogle",
      "/api/Account/loginMicrosoftAuth",
      "/api/Profile/getProfileData",
      "/api/Relapses/getEventCounterRelapses",
      "/api/Relapses/getRelapseReasons",
      "/api/EventCounter/deleteCounterByID",
      "/api/Account/changePassword",
      "/api/Account/maintenancePage"
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
