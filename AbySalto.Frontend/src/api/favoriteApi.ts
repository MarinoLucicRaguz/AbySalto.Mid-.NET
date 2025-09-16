import type { FavoriteDto } from "../types/dto";
import { safeDelete, safeGet, safePost } from "./client/http";

export async function getFavorites() {
  return safeGet<FavoriteDto[]>("/favorite");
}

export async function addFavorite(productId: number) {
  return safePost<FavoriteDto>(`/favorite/${productId}`);
}

export async function removeFavorite(productId: number) {
  return safeDelete<boolean>(`/favorite/${productId}`);
}
