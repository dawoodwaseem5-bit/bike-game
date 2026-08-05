import React, { useEffect, useState } from "react";
import { fetchApi } from "../api/api";

function Customers() {
  const [customers, setCustomers] = useState([]);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  const [editId, setEditId] = useState(null);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [company, setCompany] = useState("");

  const loadCustomers = async () => {
    try {
      const response = await fetchApi("/customers");
      if (response.ok) {
        const result = await response.json();
        setCustomers(result.data || []);
      } else {
        setError("Failed to load customers");
      }
    } catch (err) {
      setError("Error connecting to server");
    }
  };

  useEffect(() => {
    loadCustomers();
  }, []);

  const handleUpdateCustomer = async (e) => {
    e.preventDefault();
    try {
      const response = await fetchApi(`/customers/${editId}`, {
        method: "PUT",
        body: JSON.stringify({ name, email, company })
      });
      if (response.ok) {
        setMessage("Customer updated successfully!");
        handleCancel();
        loadCustomers();
      } else {
        const data = await response.json();
        setMessage(`Error: ${data.message || JSON.stringify(data.errors) || "Failed to update"}`);
      }
    } catch (err) {
      setMessage("Error updating customer.");
    }
  };

  const handleEdit = (c) => {
    setEditId(c.customerId);
    setName(c.name);
    setEmail(c.email);
    setCompany(c.company || "");
    window.scrollTo(0, 0);
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Are you sure you want to delete this customer?")) return;
    try {
      const response = await fetchApi(`/customers/${id}`, { method: "DELETE" });
      if (response.ok) {
        setMessage("Customer deleted.");
        loadCustomers();
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
    setEmail("");
    setCompany("");
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Customers</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}

      {editId && (
        <div style={{ border: "1px solid #ccc", padding: "15px", marginBottom: "20px", background: "#f9f9f9" }}>
          <h3>Edit Customer</h3>
          <form onSubmit={handleUpdateCustomer}>
            <div style={{ marginBottom: "10px" }}>
              <label>Name: </label>
              <input type="text" value={name} onChange={e => setName(e.target.value)} required />
            </div>
            <div style={{ marginBottom: "10px" }}>
              <label>Email: </label>
              <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
            </div>
            <div style={{ marginBottom: "10px" }}>
              <label>Company: </label>
              <input type="text" value={company} onChange={e => setCompany(e.target.value)} />
            </div>
            <button type="submit">Update Customer</button>
            <button type="button" onClick={handleCancel} style={{ marginLeft: "10px" }}>Cancel</button>
          </form>
          {message && <p><b>{message}</b></p>}
        </div>
      )}
      {!editId && message && <p><b>{message}</b></p>}

      <table border="1" cellPadding="5" style={{ borderCollapse: "collapse", width: "100%" }}>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>Company</th>
            <th>Active</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {customers.length === 0 ? (
            <tr><td colSpan="6">No customers found.</td></tr>
          ) : (
            customers.map(c => (
              <tr key={c.customerId}>
                <td>{c.customerId}</td>
                <td>{c.name}</td>
                <td>{c.email}</td>
                <td>{c.company || "N/A"}</td>
                <td>{c.isActive ? "Yes" : "No"}</td>
                <td>
                  <button onClick={() => handleEdit(c)}>Edit</button>
                  <button onClick={() => handleDelete(c.customerId)} style={{ marginLeft: "10px", color: "red" }}>Delete</button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default Customers;
