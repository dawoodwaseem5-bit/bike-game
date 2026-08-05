import React, { useState } from "react";
import { fetchApi } from "../api/api";

function Users() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("SalesRep");
  const [message, setMessage] = useState("");

  const handleAddUser = async (e) => {
    e.preventDefault();
    try {
      const response = await fetchApi("/users", {
        method: "POST",
        body: JSON.stringify({ username, email, password, role }),
      });
      const data = await response.json();
      if (response.ok) {
        setMessage("User added successfully!");
        setUsername("");
        setEmail("");
        setPassword("");
      } else {
        setMessage(`Error: ${data.message || "Failed to add user"}`);
      }
    } catch (error) {
      setMessage("Network error.");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Manage Users</h2>
      
      <div style={{ border: "1px solid #ccc", padding: "15px", marginBottom: "20px" }}>
        <h3>Add New User (Manager / SalesRep)</h3>
        <form onSubmit={handleAddUser}>
          <div style={{ marginBottom: "10px" }}>
            <label>Username: </label>
            <input type="text" value={username} onChange={e => setUsername(e.target.value)} required />
          </div>
          <div style={{ marginBottom: "10px" }}>
            <label>Email: </label>
            <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
          </div>
          <div style={{ marginBottom: "10px" }}>
            <label>Password: </label>
            <input type="password" value={password} onChange={e => setPassword(e.target.value)} required />
          </div>
          <div style={{ marginBottom: "10px" }}>
            <label>Role: </label>
            <select value={role} onChange={e => setRole(e.target.value)}>
              <option value="SalesRep">Sales Rep</option>
              <option value="Manager">Manager</option>
            </select>
          </div>
          <button type="submit">Add User</button>
        </form>
        {message && <p><b>{message}</b></p>}
      </div>
    </div>
  );
}

export default Users;
