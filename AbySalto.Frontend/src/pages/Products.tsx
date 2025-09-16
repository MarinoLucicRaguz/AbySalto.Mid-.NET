import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { type PagedResult, type ServiceResponse } from "../types/api";
import { getProducts } from "../api/productApi";
import type { FavoriteDto, ProductDetailDto } from "../types/dto";
import { useAddToBasket } from "../hooks/useBasket";
import { useFavorites, useAddFavorite, useRemoveFavorite } from "../hooks/useFavorites";

import {
  Box,
  Typography,
  Grid,
  Card,
  CardContent,
  CardActions,
  Button,
  CircularProgress,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Paper,
  CardActionArea,
} from "@mui/material";
import { Link, useLocation } from "react-router-dom";

const sortableKeys: (keyof ProductDetailDto)[] = [
  "title",
  "price",
  "rating",
  "stock",
  "category",
  "brand",
  "sku",
  "discountPercentage",
  "availabilityStatus",
  "minimumOrderQuantity",
  "warrantyInformation",
  "shippingInformation",
  "returnPolicy",
  "weight",
];

export default function Products() {
  const location = useLocation();
  const state = location.state as { page?: number; size?: number; sortBy?: keyof ProductDetailDto; order?: string } | undefined;

  const [page, setPage] = useState(state?.page ?? 0);
  const [size, setSize] = useState<number>(state?.size ?? 10);
  const [sortBy, setSortBy] = useState<keyof ProductDetailDto>(state?.sortBy ?? "title");
  const [order, setOrder] = useState(state?.order ?? "asc");

  const { data, isLoading, error } = useQuery({
    queryKey: ["products", page, size, sortBy, order],
    queryFn: () => getProducts({ page: page + 1, size, sortBy, order }),
  });

  const { data: favoritesData } = useFavorites();
  const favorites = (favoritesData as ServiceResponse<FavoriteDto[]>)?.data ?? [];

  const { mutate: addToBasket, isPending } = useAddToBasket();
  const { mutate: addFavorite } = useAddFavorite();
  const { mutate: removeFavorite } = useRemoveFavorite();

  if (isLoading)
    return (
      <Box display="flex" justifyContent="center" mt={4}>
        <CircularProgress />
      </Box>
    );

  if (error) {
    return <Typography color="error">Error loading products</Typography>;
  }

  const result = data as ServiceResponse<PagedResult<ProductDetailDto>>;
  const products = result.data?.items ?? [];
  const totalCount = result.data?.total ?? 0;
  const totalPages = Math.ceil(totalCount / size);

  return (
    <Box maxWidth="xl" mx="auto" p={2} pb={8}>
      <Box display="flex" justifyContent="flex-end" gap={2} mb={3}>
        <FormControl size="small">
          <InputLabel id="sort-by-label">Sort By</InputLabel>
          <Select labelId="sort-by-label" value={sortBy} label="Sort By" onChange={(e) => setSortBy(e.target.value as keyof ProductDetailDto)}>
            {sortableKeys.map((key) => (
              <MenuItem key={key} value={key}>
                {key.charAt(0).toUpperCase() + key.slice(1)}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
        <FormControl size="small">
          <InputLabel id="order-label">Order</InputLabel>
          <Select labelId="order-label" value={order} label="Order" onChange={(e) => setOrder(e.target.value)}>
            <MenuItem value="asc">Ascending</MenuItem>
            <MenuItem value="desc">Descending</MenuItem>
          </Select>
        </FormControl>
      </Box>
      <Grid container spacing={2}>
        {products.map((p) => {
          const isFav = favorites.some((f) => f.productId === p.id);
          return (
            <Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={p.id}>
              <Card
                sx={{
                  height: 170,
                  display: "flex",
                  flexDirection: "column",
                  transition: "transform 0.2s, box-shadow 0.2s",
                  "&:hover": {
                    transform: "translateY(-3px)",
                    boxShadow: 4,
                  },
                }}>
                <CardActionArea
                  component={Link}
                  to={`/products/${p.id}`}
                  state={{ page, size, sortBy, order }}
                  sx={{ flexGrow: 1, textDecoration: "none", color: "inherit" }}>
                  <CardContent sx={{ flexGrow: 1, pb: 1 }}>
                    <Typography variant="subtitle1" noWrap title={p.title} sx={{ mb: 1 }}>
                      {p.title}
                    </Typography>
                    <Typography
                      variant="body2"
                      color="text.secondary"
                      sx={{
                        mb: 1,
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                        display: "-webkit-box",
                        WebkitLineClamp: 2,
                        WebkitBoxOrient: "vertical",
                      }}>
                      {p.description}
                    </Typography>
                    <Typography variant="subtitle2" fontWeight="bold">
                      {p.price.toFixed(2)}€
                    </Typography>
                  </CardContent>
                </CardActionArea>
                <CardActions sx={{ gap: 1, pt: 0 }}>
                  <Button variant="contained" color="primary" size="small" disabled={isPending} onClick={() => addToBasket({ productId: p.id })}>
                    {isPending ? "Adding..." : "Basket"}
                  </Button>

                  {isFav ? (
                    <Button variant="contained" color="error" size="small" onClick={() => removeFavorite(p.id)}>
                      Remove
                    </Button>
                  ) : (
                    <Button variant="outlined" size="small" onClick={() => addFavorite(p.id)}>
                      Favorite
                    </Button>
                  )}
                </CardActions>
              </Card>
            </Grid>
          );
        })}
      </Grid>
      <Paper
        elevation={3}
        sx={{
          position: "fixed",
          bottom: 0,
          left: 0,
          width: "100%",
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          gap: 2,
          p: 1,
          borderTop: "1px solid #ddd",
          zIndex: 1200,
        }}>
        <Button variant="outlined" disabled={page === 0} onClick={() => setPage((p) => p - 1)}>
          Previous
        </Button>
        <Typography>
          Page {page + 1} of {totalPages || 1}
        </Typography>
        <Button variant="outlined" disabled={page + 1 >= totalPages} onClick={() => setPage((p) => p + 1)}>
          Next
        </Button>

        <FormControl size="small">
          <Select<number>
            value={size}
            onChange={(e) => {
              setSize(e.target.value as number);
              setPage(0);
            }}>
            <MenuItem value={5}>5</MenuItem>
            <MenuItem value={10}>10</MenuItem>
            <MenuItem value={15}>15</MenuItem>
          </Select>
        </FormControl>
      </Paper>
    </Box>
  );
}
