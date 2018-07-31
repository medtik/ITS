import axios from "axios";

const instance = axios.create({
  // baseURL: 'https://some-domain.com/api/',
  baseURL: 'http://itssolution.azurewebsites.net',
  // baseURL: 'http://localhost:56288/',
  timeout: 10000,
});

export default instance;
