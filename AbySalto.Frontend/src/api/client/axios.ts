import axios, { AxiosError, type AxiosRequestConfig } from "axios";
import { clearAuth, setAuth, store, type RootState } from "../../store/store";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: { "Content-Type": "application/json" },
  withCredentials: true,
});

api.interceptors.request.use((config) => {
  const state: RootState = store.getState();
  const token = state.auth.token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

let isRefreshingToken = false;
let queue: Array<(token: string | null) => void> = [];

const processQueue = (token: string | null) => {
  queue.forEach((cb) => cb(token));
  queue = [];
};

api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const original = error.config as AxiosRequestConfig & { _retry?: boolean };

    if (!error.response) return Promise.reject(error);

    if (isRefreshingToken && !original._retry) {
      return new Promise((resolve, reject) => {
        queue.push((newToken) => {
          if (!newToken) return reject(error);
          original.headers = {
            ...original.headers,
            Authorization: `Bearer ${newToken}`,
          };
          resolve(api(original));
        });
      });
    }

    if (error.response.status === 401 && !original._retry) {
      original._retry = true;
      isRefreshingToken = true;

      try {
        const result = await axios.post(`${import.meta.env.VITE_API_URL}/user/refresh`, {}, { withCredentials: true });

        if (!result.data.success || !result.data.data) {
          throw new Error("Refresh failed");
        }

        const newToken = result.data.data.token;
        const user = result.data.data.user;

        store.dispatch(setAuth({ token: newToken, user }));
        processQueue(newToken);

        original.headers = {
          ...original.headers,
          Authorization: `Bearer ${newToken}`,
        };
        return api(original);
      } catch (err) {
        processQueue(null);
        store.dispatch(clearAuth());
        window.location.href = "/login";
        return Promise.reject(err);
      } finally {
        isRefreshingToken = false;
      }
    }

    if (error.response.status === 401 && original._retry) {
      store.dispatch(clearAuth());
      window.location.href = "/login";
    }

    return Promise.reject(error);
  }
);

export default api;
