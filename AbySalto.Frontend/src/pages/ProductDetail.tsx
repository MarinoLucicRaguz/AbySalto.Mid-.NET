import { useParams, useNavigate, useLocation } from "react-router-dom";
import type { ServiceResponse } from "../types/api";
import type { ProductDto, FavoriteDto, ProductDetailExtendedDto } from "../types/dto";
import { Box, Typography, CircularProgress, Button, Card, CardContent, Chip, Divider, Rating } from "@mui/material";
import Grid from "@mui/material/Grid";
import { useAddToBasket } from "../hooks/useBasket";
import { useAddFavorite, useFavorites, useRemoveFavorite } from "../hooks/useFavorites";
import { useProductDetails } from "../hooks/useProducts";

export default function ProductDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const location = useLocation();

  const productId = Number(id);
  const fromState = location.state as { page?: number; size?: number; sortBy?: keyof ProductDto; order?: string } | undefined;

  const { data, isLoading, error } = useProductDetails(productId);

  const { mutate: addToBasket, isPending } = useAddToBasket();
  const { data: favoritesData } = useFavorites();
  const { mutate: addFavorite } = useAddFavorite();
  const { mutate: removeFavorite } = useRemoveFavorite();

  if (isLoading) {
    return (
      <Box display="flex" justifyContent="center" mt={4}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) return <Typography color="error">Error loading product</Typography>;

  const result = data as ServiceResponse<ProductDetailExtendedDto>;
  const product = result?.data;

  if (!product) return <Typography>Product not found</Typography>;

  const favs = (favoritesData as ServiceResponse<FavoriteDto[]>)?.data ?? [];
  const isFav = favs.some((f) => f.productId === product.id);

  return (
    <Box maxWidth="lg" mx="auto" p={2}>
      <Button
        variant="outlined"
        onClick={() => {
          if (fromState) navigate("/products", { state: fromState });
          else navigate(-1);
        }}
        sx={{ mb: 2 }}>
        Back
      </Button>
      <Grid container spacing={3}>
        <Grid size={{ xs: 12, md: 6 }}>
          {product.thumbnail && <img src={product.thumbnail} alt={product.title} style={{ width: "35%", borderRadius: 8 }} />}
          {product.images?.length ? (
            <Box display="flex" gap={1} mt={1} flexWrap="wrap">
              {product.images.map((img) => (
                <img key={img} src={img} style={{ width: 80, height: 80, objectFit: "cover", borderRadius: 4 }} />
              ))}
            </Box>
          ) : null}
        </Grid>
        <Grid size={{ xs: 12, md: 6 }}>
          <Typography variant="h4" gutterBottom>
            {product.title}
          </Typography>
          <Box display="flex" alignItems="center" gap={1} mb={1}>
            <Rating value={Number(product.rating) || 0} precision={0.1} readOnly />
            <Typography variant="body2" color="text.secondary">
              {product.rating?.toFixed?.(1)} / 5
            </Typography>
          </Box>
          <Typography variant="subtitle1" color="text.secondary" gutterBottom>
            {product.brand} {product.category}
          </Typography>
          <Typography variant="h5" fontWeight="bold" gutterBottom>
            {product.price.toFixed(2)}€
            {product.discountPercentage ? (
              <Typography component="span" variant="body1" color="success.main" sx={{ ml: 1 }}>
                -{product.discountPercentage}%
              </Typography>
            ) : null}
          </Typography>
          <Typography variant="body2" color="text.secondary" gutterBottom>
            Stock: {product.stock} {product.availabilityStatus ? `(${product.availabilityStatus})` : ""}
          </Typography>
          <Typography variant="body2" gutterBottom>
            SKU: {product.sku} {product.minimumOrderQuantity ? ` - MOQ: ${product.minimumOrderQuantity}` : ""}
          </Typography>
          {product.tags?.length ? (
            <Box mt={1} mb={2}>
              {product.tags.map((tag) => (
                <Chip key={tag} label={tag} size="small" sx={{ mr: 0.5, mb: 0.5 }} />
              ))}
            </Box>
          ) : null}
          <Box display="flex" gap={2} mt={2}>
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
        </Grid>
        <Grid size={{ xs: 12 }}>
          <Divider sx={{ my: 2 }} />
          <Typography variant="h6" gutterBottom>
            Description
          </Typography>
          <Typography>{product.description}</Typography>
        </Grid>
        {(product.warrantyInformation || product.shippingInformation || product.returnPolicy) && (
          <Grid size={{ xs: 12 }}>
            <Typography variant="h6" gutterBottom>
              Shipping & Warranty
            </Typography>
            {product.warrantyInformation && <Typography>Warranty: {product.warrantyInformation}</Typography>}
            {product.shippingInformation && <Typography>Shipping: {product.shippingInformation}</Typography>}
            {product.returnPolicy && <Typography>Return Policy: {product.returnPolicy}</Typography>}
          </Grid>
        )}
        {(product.weight || product.dimensions) && (
          <Grid size={{ xs: 12 }}>
            <Typography variant="h6" gutterBottom>
              Specifications
            </Typography>
            <Typography>
              {typeof product.weight === "number" ? `Weight: ${product.weight}g` : null}
              {product.dimensions ? ` - W: ${product.dimensions.width} × H: ${product.dimensions.height} × D: ${product.dimensions.depth}` : null}
            </Typography>
          </Grid>
        )}
        <Grid size={{ xs: 12 }}>
          <Typography variant="h6" gutterBottom>
            Reviews
          </Typography>
          {product.reviews?.length ? (
            product.reviews.map((r, idx) => (
              <Card key={`${r.reviewerEmail}-${idx}`} sx={{ mb: 2 }}>
                <CardContent>
                  <Box display="flex" alignItems="center" gap={1}>
                    <Typography fontWeight="bold">{r.reviewerName}</Typography>
                    <Rating value={Number(r.rating) || 0} precision={0.5} readOnly size="small" />
                  </Box>
                  <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
                    {new Date(r.date).toLocaleDateString()}
                  </Typography>
                  <Typography sx={{ mt: 1 }}>{r.comment}</Typography>
                </CardContent>
              </Card>
            ))
          ) : (
            <Typography>No reviews yet</Typography>
          )}
        </Grid>
        {product.meta && (
          <Grid size={{ xs: 12 }}>
            <Divider sx={{ my: 2 }} />
            <Typography variant="caption" color="text.secondary">
              Added: {new Date(product.meta.createdAt).toLocaleDateString()} - Updated: {new Date(product.meta.updatedAt).toLocaleDateString()} -
              Barcode: {product.meta.barcode}
            </Typography>
          </Grid>
        )}
      </Grid>
    </Box>
  );
}
