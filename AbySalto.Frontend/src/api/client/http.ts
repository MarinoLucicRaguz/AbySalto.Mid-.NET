import type { ServiceResponse } from "../../types/api";
import api from "./axios";

export async function safeGet<T>(url: string): Promise<ServiceResponse<T>> {
  try {
    const { data } = await api.get<ServiceResponse<T>>(url);
    return data;
  } catch (err: any) {
    if (err.response?.data) {
      return err.response.data as ServiceResponse<T>;
    }
    return {
      success: false,
      message: "Network error",
      data: null,
      statusCode: 0,
    };
  }
}

export async function safePost<T>(url: string, body?: any): Promise<ServiceResponse<T>> {
  try {
    const { data } = await api.post<ServiceResponse<T>>(url, body);
    return data;
  } catch (err: any) {
    if (err.response?.data) {
      return err.response.data as ServiceResponse<T>;
    }
    return {
      success: false,
      message: "Network error",
      data: null,
      statusCode: 0,
    };
  }
}

export async function safePut<T>(url: string, body?: any): Promise<ServiceResponse<T>> {
  try {
    const { data } = await api.put<ServiceResponse<T>>(url, body);
    return data;
  } catch (err: any) {
    if (err.response?.data) {
      return err.response.data as ServiceResponse<T>;
    }
    return {
      success: false,
      message: "Network error",
      data: null,
      statusCode: 0,
    };
  }
}

export async function safeDelete<T>(url: string): Promise<ServiceResponse<T>> {
  try {
    const { data } = await api.delete<ServiceResponse<T>>(url);
    return data;
  } catch (err: any) {
    if (err.response?.data) {
      return err.response.data as ServiceResponse<T>;
    }
    return {
      success: false,
      message: "Network error",
      data: null,
      statusCode: 0,
    };
  }
}
