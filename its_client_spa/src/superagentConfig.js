import superagent from 'superagent';
import superagentJsonapify from 'superagent-jsonapify';
import prefix from "superagent-prefix";

// superagentJsonapify(superagent);
// const host = "http://itssolution.azurewebsites.net";
const host = "http://localhost:56288";
export default {
  prefix: prefix('http://localhost'),
  root: host
}



