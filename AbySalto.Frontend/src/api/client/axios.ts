import axios from "axios";
import { store, type RootState } from "../../store/store";
import { logoutUser } from "../../utils/authUtils";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: { "Content-Type": "application/json" },
});

api.interceptors.request.use((config) => {
  const state: RootState = store.getState();
  const token = state.auth.token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response.status === 401) {
      logoutUser();
    }

    return Promise.reject(error);
  }
);

export default api;
