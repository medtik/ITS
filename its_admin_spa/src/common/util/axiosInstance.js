import axios from "axios";
const root = "http://itssolutiong8.azurewebsites.net/";

const instance = axios.create({
  baseURL: root,
  timeout: 130000,
});

export default instance;
