import { useQuery } from "@tanstack/react-query";
import type { ProductQuery } from "../types/dto";
import { getProductById, getProducts } from "../api/productApi";

export const useProducts = (query: ProductQuery = { page: 1, size: 10, sortBy: "", order: "" }) =>
  useQuery({
    queryKey: ["products", query],
    queryFn: () => getProducts(query),
  });

export const useProduct = (id: number) =>
  useQuery({
    queryKey: ["product", id],
    queryFn: () => getProductById(id),
    enabled: !!id,
  });
