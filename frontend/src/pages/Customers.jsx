import React, { useEffect, useState, useCallback } from "react";
import { fetchApi } from "../api/api";

function Customers() {
  const [customers, setCustomers] = useState([]);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const [search, setSearch] = useState("");

  const [editId, setEditId] = useState(null);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [company, setCompany] = useState("");
  const [address, setAddress] = useState("");
  const [isActive, setIsActive] = useState(true);

  const loadCustomers = useCallback(async (q = "") => {
    try {
      const response = await fetchApi(`/customers?search=${encodeURIComponent(q)}`);
      if (response.ok) {
        const result = await response.json();
        setCustomers(result.data || []);
      } else {
        setError("Failed to load customers");
      }
    } catch (err) {
      setError("Error connecting to server");
    }
  }, []);

  useEffect(() => {
    loadCustomers("");
  }, [loadCustomers]);

  const handleUpdateCustomer = async (e) => {
    e.preventDefault();
    try {
      const response = await fetchApi(`/customers/${editId}`, {
        method: "PUT",
        body: JSON.stringify({ name, email, company, address, isActive })
      });
      if (response.ok) {
        setMessage("Customer updated successfully!");
        handleCancel();
        loadCustomers(search);
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
    setAddress(c.address || "");
    setIsActive(c.isActive !== false);
    window.scrollTo(0, 0);
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Are you sure you want to delete this customer?")) return;
    try {
      const response = await fetchApi(`/customers/${id}`, { method: "DELETE" });
      if (response.ok) {
        setMessage("Customer deleted.");
        loadCustomers(search);
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
    setAddress("");
    setIsActive(true);
  };

  return (
    <div className="page">
      <h2>Customers</h2>
      {error && <p className="error">{error}</p>}

      <form
        className="field"
        onSubmit={(e) => {
          e.preventDefault();
          loadCustomers(search);
        }}
      >
        <input
          type="text"
          placeholder="Search by name, email, or company..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        <button type="submit" className="ml">Search</button>
      </form>

      {editId && (
        <div className="panel panel-edit">
          <h3>Edit Customer</h3>
          <form onSubmit={handleUpdateCustomer}>
            <div className="field">
              <label>Name: </label>
              <input type="text" value={name} onChange={e => setName(e.target.value)} required />
            </div>
            <div className="field">
              <label>Email: </label>
              <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
            </div>
            <div className="field">
              <label>Company: </label>
              <input type="text" value={company} onChange={e => setCompany(e.target.value)} />
            </div>
            <div className="field">
              <label>Address: </label>
              <input type="text" value={address} onChange={e => setAddress(e.target.value)} className="w-addr" />
            </div>
            <button type="submit">Update Customer</button>
            <button type="button" onClick={handleCancel} className="ml">Cancel</button>
          </form>
          {message && <p><b>{message}</b></p>}
        </div>
      )}
      {!editId && message && <p><b>{message}</b></p>}

      <table border="1" cellPadding="5" className="table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>Company</th>
            <th>Address</th>
            <th>Active</th>
            <th>Created</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {customers.length === 0 ? (
            <tr><td colSpan="8">No customers found.</td></tr>
          ) : (
            customers.map(c => (
              <tr key={c.customerId}>
                <td>{c.customerId}</td>
                <td>{c.name}</td>
                <td>{c.email}</td>
                <td>{c.company || "N/A"}</td>
                <td>{c.address || "N/A"}</td>
                <td>{c.isActive ? "Yes" : "No"}</td>
                <td>{c.createdAt ? new Date(c.createdAt).toLocaleDateString() : "N/A"}</td>
                <td>
                  <button onClick={() => handleEdit(c)}>Edit</button>
                  <button onClick={() => handleDelete(c.customerId)} className="ml error">Delete</button>
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
