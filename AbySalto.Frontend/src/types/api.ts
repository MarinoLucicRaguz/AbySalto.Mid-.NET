export interface ServiceResponse<T> {
  data: T | null;
  success: boolean;
  message?: string | null;
  statusCode?: number | null;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
  skip: number;
  limit: number;
}
