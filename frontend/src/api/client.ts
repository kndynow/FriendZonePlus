import axios, { type AxiosInstance } from "axios";
import { setupInterceptors } from "./interceptors";


const apiClient: AxiosInstance = axios.create({
    baseURL: '/api',
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
    timeout: 10000
});

setupInterceptors(apiClient);

export default apiClient;