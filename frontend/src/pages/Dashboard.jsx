import React, { useEffect, useState } from "react";
import { fetchApi } from "../api/api";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Cell,
} from "recharts";

const STATUS_COLORS = {
  Pending: "#f0ad4e",
  Approved: "#5cb85c",
  Rejected: "#d9534f",
  Draft: "#777777",
};

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

  if (error) return <p className="error page">{error}</p>;
  if (!summary) return <p className="page">Loading...</p>;

  const chartData = Object.entries(summary.quotationsByStatus || {}).map(
    ([status, count]) => ({ status, count })
  );

  return (
    <div className="page">
      <h2>Dashboard Summary</h2>
      <ul>
        <li>
          <strong>Total Customers:</strong> {summary.totalCustomers}
        </li>
        <li>
          <strong>Total Products:</strong> {summary.totalProducts}
        </li>
        <li>
          <strong>Total Quotations:</strong> {summary.totalQuotations}
        </li>
        <li>
          <strong>Total Revenue:</strong> ${summary.totalRevenue?.toFixed(2)}
        </li>
      </ul>

      <h3 className="mt">Quotations by Status</h3>
      {chartData.length === 0 ? (
        <p>No quotation data to chart.</p>
      ) : (
        <div className="chart">
          <ResponsiveContainer width="100%" height="100%">
            <BarChart data={chartData} margin={{ top: 10, right: 20, left: 0, bottom: 5 }}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="status" />
              <YAxis allowDecimals={false} />
              <Tooltip />
              <Bar dataKey="count" name="Quotations">
                {chartData.map((entry) => (
                  <Cell
                    key={entry.status}
                    fill={STATUS_COLORS[entry.status] || "#337ab7"}
                  />
                ))}
              </Bar>
            </BarChart>
          </ResponsiveContainer>
        </div>
      )}
    </div>
  );
}

export default Dashboard;
