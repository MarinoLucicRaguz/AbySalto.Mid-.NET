import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { addFavorite, getFavorites, removeFavorite } from "../api/favoriteApi";

export const useFavorites = () =>
  useQuery({
    queryKey: ["favorites"],
    queryFn: getFavorites,
  });

export const useAddFavorite = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (productId: number) => addFavorite(productId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["favorites"] }),
  });
};

export const useRemoveFavorite = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (productId: number) => removeFavorite(productId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ["favorites"] }),
  });
};
