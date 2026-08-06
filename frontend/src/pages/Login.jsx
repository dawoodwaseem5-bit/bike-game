import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { fetchApi } from "../api/api";
import "../styles/login.css";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await fetchApi("/auth/login", {
        method: "POST",
        body: JSON.stringify({ email, password }),
      });

      const data = await response.json();
      if (response.ok) {
        const role = data.user?.role || "";
        localStorage.setItem("token", data.token);
        localStorage.setItem("userRole", role);
        setMessage(`Success! Welcome ${data.user?.username || email}`);
        navigate(role === "Customer" ? "/quotations" : "/");
      } else {
        setMessage(`Error: ${data.message}`);
      }
    } catch (error) {
      setMessage("Network error or backend is not running.");
    }
  };

  return (
    <div className="login-page">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <div className="field">
          <label>Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="field">
          <label>Password</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Login</button>
      </form>
      {message && (
        <p className="msg">
          <b>{message}</b>
        </p>
      )}
      <p className="link">
        If not already a user? <Link to="/register">Register here</Link>
      </p>
    </div>
  );
}

export default Login;
