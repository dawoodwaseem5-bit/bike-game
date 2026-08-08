import React, { useEffect, useState, useCallback } from "react";
import { fetchApi } from "../api/api";
import { useAuth } from "../context/AuthContext";

function Quotations() {
  const [quotations, setQuotations] = useState([]);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const [search, setSearch] = useState("");
  const [finalizeId, setFinalizeId] = useState(null);
  const [finalizeTax, setFinalizeTax] = useState(0);
  const [finalizeDiscount, setFinalizeDiscount] = useState(0);

  const { role } = useAuth();

  const loadQuotations = useCallback(async (q = "") => {
    try {
      const response = await fetchApi(`/quotations?search=${encodeURIComponent(q)}`);
      if (response.ok) {
        const result = await response.json();
        setQuotations(result.data || []);
      } else {
        setError("Failed to load quotations");
      }
    } catch (err) {
      setError("Error connecting to server");
    }
  }, []);

  useEffect(() => {
    loadQuotations();
  }, [loadQuotations]);

  const openFinalize = (id) => {
    setFinalizeId(id);
    setFinalizeTax(0);
    setFinalizeDiscount(0);
    window.scrollTo(0, 0);
  };

  const handleFinalize = async (e) => {
    e.preventDefault();
    try {
      const response = await fetchApi(`/quotations/${finalizeId}/finalize`, {
        method: "PUT",
        body: JSON.stringify({
          taxRate: parseFloat(finalizeTax) || 0,
          discountPercent: parseFloat(finalizeDiscount) || 0,
        }),
      });
      if (response.ok) {
        setMessage("Quotation finalized and sent for approval.");
        setFinalizeId(null);
        loadQuotations(search);
      } else {
        const data = await response.json();
        setMessage(`Error: ${data.message || "Failed to finalize"}`);
      }
    } catch (err) {
      setMessage("Error finalizing quotation.");
    }
  };

  const handleUpdateStatus = async (id, status) => {
    try {
      const response = await fetchApi(`/quotations/${id}/status`, {
        method: "PUT",
        body: JSON.stringify(status),
        headers: { "Content-Type": "application/json" },
      });
      if (response.ok) {
        setMessage(`Quotation marked as ${status}`);
        loadQuotations(search);
      } else {
        setMessage("Failed to update status.");
      }
    } catch (err) {
      setMessage("Error updating status.");
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm("Are you sure you want to delete this quotation?"))
      return;
    try {
      const response = await fetchApi(`/quotations/${id}`, {
        method: "DELETE",
      });
      if (response.ok) {
        setMessage("Quotation deleted.");
        loadQuotations(search);
      } else {
        setMessage("Failed to delete.");
      }
    } catch (err) {
      setMessage("Error deleting.");
    }
  };

  return (
    <div className="page">
      <h2>{role === "Customer" ? "My Quotations" : "Quotations"}</h2>
      {error && <p className="error">{error}</p>}
      {message && (
        <p>
          <b>{message}</b>
        </p>
      )}

      {role !== "Customer" && (
        <form
          className="field"
          onSubmit={(e) => {
            e.preventDefault();
            loadQuotations(search);
          }}
        >
          <input
            type="text"
            placeholder="Search by quote number or customer..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
          <button type="submit" className="ml">Search</button>
        </form>
      )}

      {role === "SalesRep" && finalizeId && (
        <div className="panel panel-edit">
          <h3>Finalize Quotation #{finalizeId}</h3>
          <form onSubmit={handleFinalize}>
            <div className="field">
              <label>Tax Rate (%): </label>
              <input
                type="number"
                step="0.01"
                min="0"
                value={finalizeTax}
                onChange={(e) => setFinalizeTax(e.target.value)}
                required
              />
            </div>
            <div className="field">
              <label>Discount (%): </label>
              <input
                type="number"
                step="0.01"
                min="0"
                max="100"
                value={finalizeDiscount}
                onChange={(e) => setFinalizeDiscount(e.target.value)}
                required
              />
            </div>
            <button type="submit">Submit Finalize</button>
            <button
              type="button"
              className="ml"
              onClick={() => setFinalizeId(null)}
            >
              Cancel
            </button>
          </form>
        </div>
      )}

      <table
        border="1"
        cellPadding="5"
        className="table"
      >
        <thead>
          <tr>
            <th>ID</th>
            <th>Quote Number</th>
            <th>Status</th>
            <th>Customer Name</th>
            <th>Total Amount</th>
            <th>Date</th>
            {(role === "Manager" || role === "SalesRep") && <th>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {quotations.length === 0 ? (
            <tr>
              <td colSpan={role === "Manager" || role === "SalesRep" ? 7 : 6}>
                No quotations found.
              </td>
            </tr>
          ) : (
            quotations.map((q) => (
              <tr key={q.quotationId}>
                <td>{q.quotationId}</td>
                <td>{q.quotationNumber}</td>
                <td>
                  <b>{q.status}</b>
                </td>
                <td>{q.customerName}</td>
                <td>${q.totalAmount?.toFixed(2)}</td>
                <td>{new Date(q.createdAt).toLocaleDateString()}</td>
                {role === "SalesRep" && (
                  <td>
                    {q.status === "Draft" && (
                      <button
                        onClick={() => openFinalize(q.quotationId)}
                        className="ok"
                      >
                        Finalize
                      </button>
                    )}
                  </td>
                )}
                {role === "Manager" && (
                  <td>
                    {q.status === "Pending" && (
                      <>
                        <button
                          onClick={() =>
                            handleUpdateStatus(q.quotationId, "Approved")
                          }
                          className="ok"
                        >
                          Approve
                        </button>
                        <button
                          onClick={() =>
                            handleUpdateStatus(q.quotationId, "Rejected")
                          }
                          className="ml warn"
                        >
                          Reject
                        </button>
                      </>
                    )}
                    <button
                      onClick={() => handleDelete(q.quotationId)}
                      className="ml error"
                    >
                      Delete
                    </button>
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

export default Quotations;
