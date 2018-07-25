import axios from "axios";

const instance = axios.create({
  // baseURL: 'https://some-domain.com/api/',
  baseURL: 'http://itssolution.azurewebsites.net',
  timeout: 10000,
});

import setMock from "./axiosMock";

// // Xóa dòng này
// setMock(instance);
export default instance;
