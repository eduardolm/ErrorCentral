import axios from 'axios';

const api = axios.create({
    baseURL: 'https://errorcentralv102.azurewebsites.net'
})

export default api;