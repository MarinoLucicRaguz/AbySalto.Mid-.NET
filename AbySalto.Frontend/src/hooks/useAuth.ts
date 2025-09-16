import { useMutation, useQuery } from "@tanstack/react-query";
import { getCurrentUser, login, logout, register } from "../api/authApi";
import { useAppDispatch } from "../store/hooks";
import { setAuth, clearAuth } from "../store/store";

export const useLogin = () => {
  const dispatch = useAppDispatch();
  return useMutation({
    mutationFn: ({ email, password }: { email: string; password: string }) => login(email, password),
    onSuccess: (res) => {
      if (res.success && res.data) {
        dispatch(setAuth({ token: res.data.token, user: res.data.user }));
      }
    },
  });
};

export const useRegister = () => {
  const dispatch = useAppDispatch();
  return useMutation({
    mutationFn: ({ username, email, password }: { username: string; email: string; password: string }) => register(username, email, password),
    onSuccess: (res) => {
      if (res.success && res.data) {
        dispatch(setAuth({ token: res.data.token, user: res.data.user }));
      }
    },
  });
};

export const useCurrentUser = () =>
  useQuery({
    queryKey: ["currentUser"],
    queryFn: getCurrentUser,
  });

export const useLogout = () => {
  const dispatch = useAppDispatch();
  return useMutation({
    mutationFn: logout,
    onSuccess: () => {
      dispatch(clearAuth());
    },
  });
};
