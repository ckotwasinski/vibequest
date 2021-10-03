// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
const baseUrl = "http://localhost:8100";
export const environment = {
  production: false,
  appShellConfig: {
    debug: false,
    networkDelay: 500
  },
  oAuthConfig: {
    //issuer:'https://localhost:44388',
    //issuer:'http://184.72.220.237/VibeQuestApi',
    issuer:'https://app.vibe-quest.com/api',
    redirectUri: baseUrl,
    responseType: 'code'
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.