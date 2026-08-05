import React, { useEffect, useState } from "react";
import { fetchApi } from "../api/api";

function Dashboard() {
  const [summary, setSummary] = useState(null);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadDashboard = async () => {
      try {
        const response = await fetchApi("/dashboard/summary");
        if (response.ok) {
          const data = await response.json();
          setSummary(data);
        } else {
          setError("Failed to load dashboard data");
        }
      } catch (err) {
        setError("Error connecting to server");
      }
    };
    loadDashboard();
  }, []);

  if (error) return <p style={{ color: "red" }}>{error}</p>;
  if (!summary) return <p>Loading...</p>;

  return (
    <div style={{ padding: "20px" }}>
      <h2>Dashboard Summary</h2>
      <ul>
        <li><strong>Total Customers:</strong> {summary.totalCustomers}</li>
        <li><strong>Total Products:</strong> {summary.totalProducts}</li>
        <li><strong>Total Quotations:</strong> {summary.totalQuotations}</li>
        <li><strong>Total Revenue:</strong> ${summary.totalRevenue?.toFixed(2)}</li>
      </ul>
    </div>
  );
}

export default Dashboard;
