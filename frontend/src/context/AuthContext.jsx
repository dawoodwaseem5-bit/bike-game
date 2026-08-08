import React, { createContext, useContext, useMemo, useState } from "react";
import { clearAuth, getRoleFromToken, getToken } from "../api/auth";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [token, setToken] = useState(() => getToken());
  const role = useMemo(() => getRoleFromToken(token), [token]);

  const login = (newToken) => {
    localStorage.setItem("token", newToken);
    localStorage.removeItem("userRole");
    setToken(newToken);
  };

  const logout = () => {
    clearAuth();
    setToken(null);
    window.location.href = "/login";
  };

  return (
    <AuthContext.Provider
      value={{ token, role, login, logout, isAuthenticated: !!token }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);
