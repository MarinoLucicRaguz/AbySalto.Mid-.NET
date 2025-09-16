import { useQuery } from "@tanstack/react-query";
import { type ServiceResponse } from "../types/api";
import type { BasketDto } from "../types/dto";
import { getBasket } from "../api/basketApi";
import { useAddToBasket, useReduceFromBasket, useRemoveFromBasket, useClearBasket } from "../hooks/useBasket";

import { Box, Card, CardContent, CardActions, Typography, Button, CircularProgress, IconButton } from "@mui/material";

import { Grid } from "@mui/material";
import RemoveIcon from "@mui/icons-material/Remove";
import AddIcon from "@mui/icons-material/Add";
import DeleteIcon from "@mui/icons-material/Delete";

export default function Basket() {
  const { data, isLoading, error } = useQuery({
    queryKey: ["basket"],
    queryFn: getBasket,
  });

  const { mutate: addToBasket } = useAddToBasket();
  const { mutate: reduceFromBasket } = useReduceFromBasket();
  const { mutate: removeFromBasket } = useRemoveFromBasket();
  const { mutate: clearBasket } = useClearBasket();

  if (isLoading)
    return (
      <Box display="flex" justifyContent="center" mt={4}>
        <CircularProgress />
      </Box>
    );

  if (error) return <Typography color="error">Error loading basket</Typography>;

  const result = data as ServiceResponse<BasketDto>;
  const basket = result.data;

  if (!basket) return <Typography>Your basket is empty</Typography>;

  return (
    <Box maxWidth="lg" mx="auto" p={2}>
      <Typography variant="h4" gutterBottom>
        Basket
      </Typography>

      <Grid container spacing={2}>
        {basket.items.map((item) => (
          <Grid size={{ xs: 12, sm: 6, md: 4, lg: 3 }} key={item.productId}>
            <Card>
              <CardContent>
                <Typography variant="h6" noWrap>
                  {item.product?.title}
                </Typography>
                <Typography variant="body2" color="text.secondary" noWrap>
                  {item.product?.description}
                </Typography>
                <Typography variant="subtitle1" fontWeight="bold" mt={1}>
                  {((item.product?.price ?? 0) * item.quantity).toFixed(2)}€
                </Typography>
              </CardContent>
              <CardActions>
                <IconButton size="small" onClick={() => reduceFromBasket({ productId: item.productId, qty: 1 })}>
                  <RemoveIcon />
                </IconButton>
                <Typography>{item.quantity}</Typography>
                <IconButton size="small" onClick={() => addToBasket({ productId: item.productId, qty: 1 })}>
                  <AddIcon />
                </IconButton>
                <IconButton size="small" color="error" onClick={() => removeFromBasket(item.productId)}>
                  <DeleteIcon />
                </IconButton>
              </CardActions>
            </Card>
          </Grid>
        ))}
      </Grid>

      <Box display="flex" justifyContent="space-between" alignItems="center" mt={4}>
        <Typography variant="h6">Total: {basket.totalPrice.toFixed(2)}€</Typography>
        <Button variant="contained" color="error" onClick={() => clearBasket()}>
          Clear Basket
        </Button>
      </Box>
    </Box>
  );
}
