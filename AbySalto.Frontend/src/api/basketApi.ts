import type { BasketDto } from "../types/dto";
import { safeDelete, safeGet, safePost } from "./client/http";

export async function getBasket() {
  return safeGet<BasketDto>("/basket");
}

export async function addToBasket(productId: number, increment = 1) {
  return safePost<BasketDto>(`/basket/add/${productId}?increment=${increment}`);
}

export async function reduceFromBasket(productId: number, decrement = 1) {
  return safePost<BasketDto>(`/basket/reduce/${productId}?decrement=${decrement}`);
}

export async function removeFromBasket(productId: number) {
  return safeDelete<boolean>(`/basket/${productId}`);
}

export async function clearBasket() {
  return safeDelete<boolean>("/basket");
}
