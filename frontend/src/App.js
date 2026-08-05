import React from "react";
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Customers from "./pages/Customers";
import Products from "./pages/Products";
import Quotations from "./pages/Quotations";
import Register from "./pages/Register";
import Users from "./pages/Users";

const ProtectedRoute = ({ children }) => {
  const token = localStorage.getItem("token");
  if (!token) {
    return <Navigate to="/login" replace />;
  }
  return children;
};

const Layout = ({ children }) => {
  const role = localStorage.getItem("userRole") || "";

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userRole");
    window.location.href = "/login";
  };

  return (
    <div>
      <nav style={{ background: "#eee", padding: "10px", marginBottom: "20px" }}>
        <Link to="/" style={{ marginRight: "10px" }}>Dashboard</Link>
        
        {role !== "Customer" && (
          <>
            <Link to="/customers" style={{ marginRight: "10px" }}>Customers</Link>
            <Link to="/products" style={{ marginRight: "10px" }}>Products</Link>
          </>
        )}
        
        <Link to="/quotations" style={{ marginRight: "10px" }}>
          {role === "Customer" ? "My Quotations" : "Quotations"}
        </Link>
        
        {role === "Manager" && (
          <Link to="/users" style={{ marginRight: "10px" }}>Users</Link>
        )}
        
        <button onClick={handleLogout} style={{ marginLeft: "20px" }}>Logout</button>
      </nav>
      {children}
    </div>
  );
};

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        
        <Route path="/" element={
          <ProtectedRoute>
            <Layout><Dashboard /></Layout>
          </ProtectedRoute>
        } />
        
        <Route path="/customers" element={
          <ProtectedRoute>
            <Layout><Customers /></Layout>
          </ProtectedRoute>
        } />
        
        <Route path="/products" element={
          <ProtectedRoute>
            <Layout><Products /></Layout>
          </ProtectedRoute>
        } />
        
        <Route path="/quotations" element={
          <ProtectedRoute>
            <Layout><Quotations /></Layout>
          </ProtectedRoute>
        } />
        
        <Route path="/users" element={
          <ProtectedRoute>
            <Layout><Users /></Layout>
          </ProtectedRoute>
        } />
      </Routes>
    </Router>
  );
}

export default App;
