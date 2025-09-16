import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { addToBasket, clearBasket, getBasket, reduceFromBasket, removeFromBasket } from "../api/basketApi";

export const useBasket = () =>
  useQuery({
    queryKey: ["basket"],
    queryFn: getBasket,
  });

export const useAddToBasket = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ productId, qty = 1 }: { productId: number; qty?: number }) => addToBasket(productId, qty),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["basket"] }),
  });
};

export const useReduceFromBasket = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ productId, qty = 1 }: { productId: number; qty?: number }) => reduceFromBasket(productId, qty),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["basket"] }),
  });
};

export const useRemoveFromBasket = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (productId: number) => removeFromBasket(productId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["basket"] }),
  });
};

export const useClearBasket = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: clearBasket,
    onSuccess: () => qc.invalidateQueries({ queryKey: ["basket"] }),
  });
};
