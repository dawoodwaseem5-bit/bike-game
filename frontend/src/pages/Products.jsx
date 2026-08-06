import React, { useEffect, useState } from "react";
import { fetchApi } from "../api/api";

function Products() {
  const [products, setProducts] = useState([]);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  const role = localStorage.getItem("userRole") || "";

  const [name, setName] = useState("");
  const [unitPrice, setUnitPrice] = useState("");
  const [stockQuantity, setStockQuantity] = useState("");
  const [editId, setEditId] = useState(null);

  const loadProducts = async () => {
    try {
      const response = await fetchApi("/products");
      if (response.ok) {
        const result = await response.json();
        setProducts(result.data || []);
      } else {
        setError("Failed to load products");
      }
    } catch (err) {
      setError("Error connecting to server");
    }
  };

  useEffect(() => {
    loadProducts();
  }, []);

  const handleSaveProduct = async (e) => {
    e.preventDefault();
    const endpoint = editId ? `/products/${editId}` : "/products";
    const method = editId ? "PUT" : "POST";
    try {
      const response = await fetchApi(endpoint, {
        method,
        body: JSON.stringify({ name, unitPrice: parseFloat(unitPrice), stockQuantity: parseInt(stockQuantity) })
      });
      if (response.ok) {
        setMessage(editId ? "Product updated!" : "Product added!");
        handleCancel();
        loadProducts();
      } else {
        const data = await response.json();
        setMessage(`Error: ${data.message || JSON.stringify(data.errors) || "Failed to save"}`);
      }
    } catch (err) {
      setMessage("Error saving product.");
    }
  };

  const handleEdit = (p) => {
    setEditId(p.productId);
    setName(p.name);
    setUnitPrice(p.unitPrice);
    setStockQuantity(p.stockQuantity);
    window.scrollTo(0, 0);
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Are you sure you want to delete this product?")) return;
    try {
      const response = await fetchApi(`/products/${id}`, { method: "DELETE" });
      if (response.ok) {
        setMessage("Product deleted.");
        loadProducts();
      } else {
        setMessage("Failed to delete.");
      }
    } catch (err) {
      setMessage("Error deleting.");
    }
  };

  const handleCancel = () => {
    setEditId(null);
    setName("");
    setUnitPrice("");
    setStockQuantity("");
  };

  return (
    <div className="page">
      <h2>Products</h2>
      {error && <p className="error">{error}</p>}
      
      {role === "Manager" && (
      <div className="panel">
        <h3>{editId ? "Edit Product" : "Add New Product"}</h3>
        <form onSubmit={handleSaveProduct}>
          <div className="field">
            <label>Name: </label>
            <input type="text" value={name} onChange={e => setName(e.target.value)} required />
          </div>
          <div className="field">
            <label>Unit Price ($): </label>
            <input type="number" step="0.01" value={unitPrice} onChange={e => setUnitPrice(e.target.value)} required />
          </div>
          <div className="field">
            <label>Stock Quantity: </label>
            <input type="number" value={stockQuantity} onChange={e => setStockQuantity(e.target.value)} required />
          </div>
          <button type="submit">{editId ? "Update Product" : "Add Product"}</button>
          {editId && <button type="button" onClick={handleCancel} className="ml">Cancel</button>}
        </form>
        {message && <p><b>{message}</b></p>}
      </div>
      )}

      {role !== "Manager" && message && <p><b>{message}</b></p>}

      <table border="1" cellPadding="5" className="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Unit Price</th>
            <th>Stock</th>
            {role === "Manager" && <th>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {products.length === 0 ? (
            <tr><td colSpan={role === "Manager" ? 5 : 4}>No products found.</td></tr>
          ) : (
            products.map(p => (
              <tr key={p.productId}>
                <td>{p.productId}</td>
                <td>{p.name}</td>
                <td>${p.unitPrice?.toFixed(2)}</td>
                <td>{p.stockQuantity}</td>
                {role === "Manager" && (
                <td>
                  <button onClick={() => handleEdit(p)}>Edit</button>
                  <button onClick={() => handleDelete(p.productId)} className="ml error">Delete</button>
                </td>
                )}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default Products;
