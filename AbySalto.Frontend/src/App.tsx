import { Routes, Route } from "react-router-dom";
import Navbar from "./components/layout/Navbar";
import ProtectedRoute from "./components/layout/ProtectedRoute";
import Login from "./pages/Login";
import { CssBaseline } from "@mui/material";
import Register from "./pages/Register";
import Products from "./pages/Products";
import Basket from "./pages/Basket";
import Favorites from "./pages/Favorites";
import ProductDetail from "./pages/ProductDetail";

export default function App() {
  return (
    <>
      <CssBaseline />
      <Navbar />
      <Routes>
        <Route path="/products" element={<Products />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        <Route element={<ProtectedRoute />}>
          <Route path="/products/:id" element={<ProductDetail />} />
          <Route path="/basket" element={<Basket />} />
          <Route path="/favorites" element={<Favorites />} />
        </Route>
      </Routes>
    </>
  );
}
