const _ = require("lodash-4.17");
const axios = require('axios');

/*
 * CommonJS wrapper for the script. It receives a single argument, which can be destructured into:
 *  - params: Object containing the parameters provided to the script, accessible as object properties
 *  - context: Object containing the projectId, environmentId, environmentName, playerId and accessToken properties.
 *  - logger: Logging client for the script. Provides debug(), info(), warning() and error() log levels.
 */
module.exports = async ({ params, context, logger }) => {
  // Log an info message with the parameters provided to the script and the invocation context
  //logger.info("Script parameters: " + JSON.stringify(params));
  //logger.info("Authenticated within the following context: " + JSON.stringify(context));

  const clientId = "1116970853905743894";
  const clientSecret = "6ZOuRcQ4omsdZrWjgxgXnRiDAos_mrvs";
  const token = params.token;
  const REQUEST_URI = "https://discord.com/api/oauth2/token";

  let data = {
    'client_id': clientId,
    'client_secret': clientSecret,
    'grant_type': 'refresh_token',
    'refresh_token': token
  };

  const urlEncodedData = Object.keys(data)
    .map(key => encodeURIComponent(key) + '=' + encodeURIComponent(data[key]))
    .join('&');

  let config = {
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded'
    }
  }

  const result = await axios.post(REQUEST_URI, urlEncodedData, config);
  
  logger.info(result.data);
  
  return {
    access_token: result.data.access_token,
    expires_in: result.data.expires_in,
    refresh_token: result.data.refresh_token,
    scope: result.data.scope,
    token_type: result.data.token_type
  }
};