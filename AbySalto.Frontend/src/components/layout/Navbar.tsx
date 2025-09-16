import { Link as RouterLink } from "react-router-dom";
import { useAppSelector } from "../../store/hooks";
import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import "../../styles/Navbar.css";
import { useLogout } from "../../hooks/useAuth";

const navLinks = [
  { to: "/", label: "Home" },
  { to: "/products", label: "Products" },
  { to: "/basket", label: "Basket" },
  { to: "/favorites", label: "Favorites" },
];

export default function Navbar() {
  const auth = useAppSelector((s) => s.auth);
  const { mutate: logout } = useLogout();

  return (
    <AppBar position="static" className="navbar-appbar">
      <Toolbar className="navbar-toolbar">
        <Box className="navbar-links">
          {navLinks.map(({ to, label }) => (
            <Button key={to} color="inherit" component={RouterLink} to={to}>
              {label}
            </Button>
          ))}
        </Box>

        <Box className="navbar-auth">
          {auth.user ? (
            <>
              <Typography variant="body1">
                Hello, <strong>{auth.user.username}</strong>
              </Typography>
              <Button color="inherit" onClick={() => logout()} className="navbar-logout">
                Logout
              </Button>
            </>
          ) : (
            <>
              <Button color="inherit" component={RouterLink} to="/login">
                Login
              </Button>
              <Button color="inherit" component={RouterLink} to="/register">
                Register
              </Button>
            </>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
}
