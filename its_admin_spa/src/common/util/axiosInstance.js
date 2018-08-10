import axios from "axios";

const instance = axios.create({
  baseURL: 'http://itssolution.azurewebsites.net',
  // baseURL: 'http://localhost:56288/',
  timeout: 130000,
});

export default instance;
