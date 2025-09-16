import type { UserDto, AuthResponseDto } from "../types/dto";
import { safeGet, safePost } from "./client/http";

export function login(email: string, password: string) {
  return safePost<AuthResponseDto>("/user/login", { email, password });
}

export function register(username: string, email: string, password: string) {
  return safePost<AuthResponseDto>("/user/register", {
    username,
    email,
    password,
  });
}

export function getCurrentUser() {
  return safeGet<UserDto>("/user");
}

export function refresh() {
  return safePost<AuthResponseDto>("/user/refresh");
}

export function logout() {
  return safePost<void>("/user/logout");
}
