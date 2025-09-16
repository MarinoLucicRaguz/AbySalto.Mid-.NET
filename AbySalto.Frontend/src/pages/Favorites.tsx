import { useQuery } from "@tanstack/react-query";
import { type ServiceResponse } from "../types/api";
import type { FavoriteDto } from "../types/dto";
import { getFavorites } from "../api/favoriteApi";
import { useRemoveFavorite } from "../hooks/useFavorites";

import { Box, Typography, Grid, Card, CardContent, CardActions, Button, CircularProgress } from "@mui/material";

export default function Favorites() {
  const { data, isLoading, error } = useQuery({
    queryKey: ["favorites"],
    queryFn: getFavorites,
  });

  const { mutate: removeFavorite } = useRemoveFavorite();

  if (isLoading)
    return (
      <Box display="flex" justifyContent="center" mt={4}>
        <CircularProgress />
      </Box>
    );

  if (error) return <Typography color="error">Error loading favorites</Typography>;

  const result = data as ServiceResponse<FavoriteDto[]>;
  const favorites = result.data ?? [];

  if (!favorites.length) return <Typography>No favorites yet</Typography>;

  return (
    <Box maxWidth="lg" mx="auto" p={2}>
      <Typography variant="h4" gutterBottom>
        Favorites
      </Typography>

      <Grid container spacing={3}>
        {favorites.map((f) => (
          <Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={f.id}>
            <Card sx={{ height: "100%", display: "flex", flexDirection: "column" }}>
              <CardContent sx={{ flexGrow: 1 }}>
                <Typography variant="h6">{f.product?.title}</Typography>
                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                  {f.product?.description}
                </Typography>
                <Typography variant="subtitle1" fontWeight="bold">
                  {f.product?.price?.toFixed(2)}€
                </Typography>
              </CardContent>
              <CardActions>
                <Button variant="contained" color="error" fullWidth onClick={() => removeFavorite(f.productId)}>
                  Remove
                </Button>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Box>
  );
}
