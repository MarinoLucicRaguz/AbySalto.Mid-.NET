import { configureStore, createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";
import type { UserDto } from "../types/dto";

type AuthState = {
  token: string | null;
  user: UserDto | null;
};

const initialAuth: AuthState = (() => {
  try {
    const raw = localStorage.getItem("auth");
    return raw ? (JSON.parse(raw) as AuthState) : { token: null, user: null };
  } catch {
    return { token: null, user: null };
  }
})();

const authSlice = createSlice({
  name: "auth",
  initialState: initialAuth,
  reducers: {
    setAuth(state, action: PayloadAction<AuthState>) {
      state.token = action.payload.token;
      state.user = action.payload.user;
      localStorage.setItem("auth", JSON.stringify(state));
    },
    clearAuth(state) {
      state.token = null;
      state.user = null;
      localStorage.removeItem("auth");
    },
  },
});

export const { setAuth, clearAuth } = authSlice.actions;

export const store = configureStore({
  reducer: {
    auth: authSlice.reducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
