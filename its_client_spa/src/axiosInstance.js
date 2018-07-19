import axios from "axios";
import store from "./store";

const instance = axios.create({
  // baseURL: 'https://some-domain.com/api/',
  baseURL: 'http://itssolution.azurewebsites.net',
  timeout: 10000,
});

export default instance;
