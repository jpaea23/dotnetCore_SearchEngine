import axios from 'axios';
import { WEB_API_SERVICE } from '../Constants/Constants';

const instance = axios.create({
  baseURL: WEB_API_SERVICE
});

export default instance;