export interface ProductDto {
  id: number;
  title: string;
  description: string;
  price: number;
  rating: number;
  stock: number;
}

export interface ProductDetailDto extends ProductDto {
  category: string;
  brand: string;
  sku: string;
  discountPercentage: number;
  availabilityStatus: string;
  minimumOrderQuantity: number;
  warrantyInformation: string;
  shippingInformation: string;
  returnPolicy: string;
  weight: number;
  dimensions: {
    width: number;
    height: number;
    depth: number;
  };
  tags: string[];
  images: string[];
  thumbnail: string;
  reviews: ReviewDto[];
  meta: {
    createdAt: string;
    updatedAt: string;
    barcode: string;
    qrCode: string;
  };
}

export interface ReviewDto {
  rating: number;
  comment: string;
  date: string;
  reviewerName: string;
  reviewerEmail: string;
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
