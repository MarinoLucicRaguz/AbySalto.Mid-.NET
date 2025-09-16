import type { PagedResult } from "../types/api";
import type { ProductDto, ProductQuery } from "../types/dto";
import { safeGet } from "./client/http";

export async function getProducts(query?: ProductQuery) {
  const params = new URLSearchParams();
  if (query?.page) params.append("page", query.page.toString());
  if (query?.size) params.append("size", query.size.toString());
  if (query?.sortBy) params.append("sortBy", query.sortBy);
  if (query?.order) params.append("order", query.order);

  return safeGet<PagedResult<ProductDto>>(`/product/getallpaginated?${params.toString()}`);
}

export async function getProductById(id: number) {
  return safeGet<ProductDto>(`/product/${id}`);
}
