export interface ProductDto {
  id: number;
  title: string;
  description: string;
  price: number;
  rating: number;
  stock: number;
}

export interface ProductQuery {
  page?: number;
  size?: number;
  sortBy?: string;
  order?: string;
}

export interface FavoriteDto {
  id: number;
  productId: number;
  product?: ProductDto;
}

export interface FavoriteBasicDto {
  id: number;
  productId: number;
}

export interface UserDto {
  id: number;
  username: string;
  email: string;
  firstName?: string | null;
  lastName?: string | null;
}

export interface AuthResponseDto {
  token: string;
  user: UserDto;
}

export interface BasketItemDto {
  productId: number;
  quantity: number;
  product?: ProductDto | null;
}

export interface BasketDto {
  totalItems: number;
  totalQuantity: number;
  totalPrice: number;
  items: BasketItemDto[];
}
