import React from "react";
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from "react-router-dom";
import Login from "./pages/Login";
import Dashboard from "./pages/Dashboard";
import Customers from "./pages/Customers";
import Products from "./pages/Products";
import Quotations from "./pages/Quotations";
import CreateQuotation from "./pages/CreateQuotation";
import RequestQuotation from "./pages/RequestQuotation";
import Register from "./pages/Register";
import Users from "./pages/Users";
import Profile from "./pages/Profile";
import { AuthProvider, useAuth } from "./context/AuthContext";
import "./styles/main.css";

const ProtectedRoute = ({ children, roles }) => {
  const { token, role } = useAuth();
  if (!token) {
    return <Navigate to="/login" replace />;
  }
  if (roles && !roles.includes(role)) {
    return <Navigate to={role === "Customer" ? "/quotations" : "/"} replace />;
  }
  return children;
};

const Layout = ({ children }) => {
  const { role, logout } = useAuth();

  return (
    <div>
      <nav className="nav">
        {role !== "Customer" && (
          <Link to="/">Dashboard</Link>
        )}
        
        {role !== "Customer" && (
          <Link to="/customers">Customers</Link>
        )}

        <Link to="/products">Products</Link>

        {role === "SalesRep" && (
          <Link to="/create-quotation">Create Quotation</Link>
        )}

        {role === "Customer" && (
          <Link to="/request-quotation">Request Quotation</Link>
        )}
        
        <Link to="/quotations">
          {role === "Customer" ? "My Quotations" : "Quotations"}
        </Link>

        {role === "Customer" && (
          <Link to="/profile">My Profile</Link>
        )}
        
        {role === "Manager" && (
          <Link to="/users">Users</Link>
        )}
        
        <button onClick={logout}>Logout</button>
      </nav>
      {children}
    </div>
  );
};

function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      
      <Route path="/" element={
        <ProtectedRoute roles={["Manager", "SalesRep"]}>
          <Layout><Dashboard /></Layout>
        </ProtectedRoute>
      } />
      
      <Route path="/customers" element={
        <ProtectedRoute roles={["Manager", "SalesRep"]}>
          <Layout><Customers /></Layout>
        </ProtectedRoute>
      } />
      
      <Route path="/products" element={
        <ProtectedRoute roles={["Manager", "SalesRep", "Customer"]}>
          <Layout><Products /></Layout>
        </ProtectedRoute>
      } />
      
      <Route path="/quotations" element={
        <ProtectedRoute>
          <Layout><Quotations /></Layout>
        </ProtectedRoute>
      } />

      <Route path="/create-quotation" element={
        <ProtectedRoute roles={["SalesRep"]}>
          <Layout><CreateQuotation /></Layout>
        </ProtectedRoute>
      } />

      <Route path="/request-quotation" element={
        <ProtectedRoute roles={["Customer"]}>
          <Layout><RequestQuotation /></Layout>
        </ProtectedRoute>
      } />

      <Route path="/profile" element={
        <ProtectedRoute roles={["Customer"]}>
          <Layout><Profile /></Layout>
        </ProtectedRoute>
      } />
      
      <Route path="/users" element={
        <ProtectedRoute roles={["Manager"]}>
          <Layout><Users /></Layout>
        </ProtectedRoute>
      } />
    </Routes>
  );
}

function App() {
  return (
    <AuthProvider>
      <Router>
        <AppRoutes />
      </Router>
    </AuthProvider>
  );
}

export default App;
