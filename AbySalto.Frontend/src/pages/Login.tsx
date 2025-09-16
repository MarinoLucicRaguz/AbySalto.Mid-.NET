import { useState } from "react";
import { useLogin } from "../hooks/useAuth";
import { useNavigate } from "react-router-dom";
import { Box, Button, Paper, TextField, Typography, CircularProgress } from "@mui/material";
import "../styles/Login.css";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const { mutateAsync, isPending } = useLogin();
  const [error, setError] = useState("");
  const nav = useNavigate();

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    const res = await mutateAsync({ email, password });
    if (res.success) nav("/");
    else setError(res.message ?? "Login failed");
  };

  return (
    <Box className="login-container">
      <Paper elevation={3} className="login-card">
        <Typography variant="h5" className="login-title">
          Login
        </Typography>
        <form onSubmit={onSubmit} className="login-form">
          <TextField label="Email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} fullWidth required />
          <TextField label="Password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} fullWidth required />
          <Button type="submit" variant="contained" color="primary" disabled={isPending} className="login-button" fullWidth>
            {isPending ? <CircularProgress size={24} /> : "Login"}
          </Button>
          {error && (
            <Typography color="error" className="login-error">
              {error}
            </Typography>
          )}
        </form>
      </Paper>
    </Box>
  );
}
