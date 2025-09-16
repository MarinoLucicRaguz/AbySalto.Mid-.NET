import { store } from "../store/store";
import { clearAuth } from "../store/store";

export function logoutUser() {
  store.dispatch(clearAuth());
  window.location.href = "/login";
}
