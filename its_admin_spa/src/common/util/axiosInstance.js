import axios from "axios";
const root = "https://itssolutiong9.azurewebsites.net/";
// const root = "http://localhost:56288/";

const instance = axios.create({
  baseURL: root,
  timeout: 130000,
});

export default instance;
