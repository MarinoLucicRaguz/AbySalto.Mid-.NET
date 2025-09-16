import { useState } from "react";
import { useRegister } from "../hooks/useAuth";
import { useNavigate } from "react-router-dom";
import { Box, Button, Paper, TextField, Typography, CircularProgress } from "@mui/material";
import "../styles/Register.css";

export default function Register() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirm, setConfirm] = useState("");
  const { mutateAsync, isPending } = useRegister();
  const [error, setError] = useState("");
  const nav = useNavigate();

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    if (password !== confirm) {
      setError("Passwords do not match");
      return;
    }

    const res = await mutateAsync({ username, email, password });
    if (res.success) nav("/");
    else setError(res.message ?? "Registration failed");
  };

  return (
    <Box className="register-container">
      <Paper elevation={3} className="register-card">
        <Typography variant="h5" className="register-title">
          Create an Account
        </Typography>
        <form onSubmit={onSubmit} className="register-form">
          <TextField label="Username" value={username} onChange={(e) => setUsername(e.target.value)} fullWidth required />
          <TextField label="Email" type="email" value={email} onChange={(e) => setEmail(e.target.value)} fullWidth required />
          <TextField label="Password" type="password" value={password} onChange={(e) => setPassword(e.target.value)} fullWidth required />
          <TextField label="Confirm Password" type="password" value={confirm} onChange={(e) => setConfirm(e.target.value)} fullWidth required />
          <Button type="submit" variant="contained" color="primary" disabled={isPending} className="register-button" fullWidth>
            {isPending ? <CircularProgress size={24} /> : "Register"}
          </Button>

          {error && (
            <Typography color="error" className="register-error">
              {error}
            </Typography>
          )}
        </form>
      </Paper>
    </Box>
  );
}
