import { useParams, useNavigate, useLocation } from "react-router-dom";
import { type ServiceResponse } from "../types/api";
import type { ProductDto } from "../types/dto";

import { Box, Typography, CircularProgress, Button, Card, CardContent } from "@mui/material";

import { useAddToBasket } from "../hooks/useBasket";
import { useAddFavorite, useFavorites, useRemoveFavorite } from "../hooks/useFavorites";
import { useProduct } from "../hooks/useProducts";

export default function ProductDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const productId = Number(id);

  const location = useLocation();
  const fromState = location.state as { page?: number; size?: number; sortBy?: keyof ProductDto; order?: string } | undefined;

  const { data, isLoading, error } = useProduct(productId);

  const { mutate: addToBasket, isPending } = useAddToBasket();
  const { data: favoritesData } = useFavorites();
  const { mutate: addFavorite } = useAddFavorite();
  const { mutate: removeFavorite } = useRemoveFavorite();

  if (isLoading)
    return (
      <Box display="flex" justifyContent="center" mt={4}>
        <CircularProgress />
      </Box>
    );

  if (error) return <Typography color="error">Error loading product</Typography>;

  const result = data as ServiceResponse<ProductDto>;
  const product = result.data;
  if (!product) return <Typography>Product not found</Typography>;

  const isFav = favoritesData && (favoritesData.data ?? []).some((f) => f.productId === product.id);

  return (
    <Box maxWidth="md" mx="auto" p={2}>
      <Button
        variant="outlined"
        onClick={() => {
          if (fromState) {
            navigate("/products", { state: fromState });
          } else {
            navigate(-1);
          }
        }}
        sx={{ mb: 2 }}>
        Back
      </Button>
      <Card>
        <CardContent>
          <Typography variant="h4" gutterBottom>
            {product.title}
          </Typography>
          <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
            {product.description}
          </Typography>
          <Typography variant="h6" fontWeight="bold" sx={{ mb: 2 }}>
            {product.price.toFixed(2)}€
          </Typography>
          <Box display="flex" gap={2}>
            <Button variant="contained" color="primary" disabled={isPending} onClick={() => addToBasket({ productId: product.id })}>
              {isPending ? "Adding..." : "Add to Basket"}
            </Button>
            {isFav ? (
              <Button variant="contained" color="error" onClick={() => removeFavorite(product.id)}>
                Remove Favorite
              </Button>
            ) : (
              <Button variant="outlined" onClick={() => addFavorite(product.id)}>
                Add Favorite
              </Button>
            )}
          </Box>
        </CardContent>
      </Card>
    </Box>
  );
}
