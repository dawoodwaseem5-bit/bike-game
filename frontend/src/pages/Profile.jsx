import React, { useEffect, useState } from "react";
import { fetchApi } from "../api/api";

function Profile() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [address, setAddress] = useState("");
  const [company, setCompany] = useState("");
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadProfile = async () => {
      try {
        const response = await fetchApi("/customers/me");
        if (response.ok) {
          const data = await response.json();
          setName(data.name || "");
          setEmail(data.email || "");
          setAddress(data.address || "");
          setCompany(data.company || "");
        } else {
          const data = await response.json().catch(() => ({}));
          setError(data.message || "Failed to load profile");
        }
      } catch (err) {
        setError("Error connecting to server");
      } finally {
        setLoading(false);
      }
    };
    loadProfile();
  }, []);

  const handleSave = async (e) => {
    e.preventDefault();
    setMessage("");
    try {
      const response = await fetchApi("/customers/me", {
        method: "PUT",
        body: JSON.stringify({ name, address, company }),
      });
      if (response.ok) {
        setMessage("Profile updated successfully!");
      } else {
        const data = await response.json().catch(() => ({}));
        setMessage(`Error: ${data.message || "Failed to update profile"}`);
      }
    } catch (err) {
      setMessage("Error updating profile.");
    }
  };

  if (loading) return <p className="page">Loading...</p>;
  if (error) return <p className="page error">{error}</p>;

  return (
    <div className="page">
      <h2>My Profile</h2>
      <form onSubmit={handleSave} className="form-narrow">
        <div className="field">
          <label>Email: </label>
          <input type="email" value={email} disabled />
        </div>
        <div className="field">
          <label>Name: </label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="field">
          <label>Company: </label>
          <input
            type="text"
            value={company}
            onChange={(e) => setCompany(e.target.value)}
          />
        </div>
        <div className="field">
          <label>Address: </label>
          <textarea
            value={address}
            onChange={(e) => setAddress(e.target.value)}
            rows={3}
          />
        </div>
        <button type="submit">Save Profile</button>
      </form>
      {message && (
        <p>
          <b>{message}</b>
        </p>
      )}
    </div>
  );
}

export default Profile;
