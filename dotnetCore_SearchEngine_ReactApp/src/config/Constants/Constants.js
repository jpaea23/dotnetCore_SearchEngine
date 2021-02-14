import pkg from '../../../package.json';

export const WEB_API_SERVICE = (pkg['isProd']) ? 'http://localhost:62904/' : 'http://localhost:62904/';