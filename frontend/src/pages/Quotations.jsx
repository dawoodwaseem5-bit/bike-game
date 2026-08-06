import React, { useEffect, useState, useCallback } from "react";
import { fetchApi } from "../api/api";

function Quotations() {
  const [quotations, setQuotations] = useState([]);
  const [customers, setCustomers] = useState([]);
  const [products, setProducts] = useState([]);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  const role = localStorage.getItem("userRole") || "";

  // Form states
  const [customerId, setCustomerId] = useState("");
  const [taxRate, setTaxRate] = useState(0);
  const [items, setItems] = useState([
    { productId: "", quantity: 1, discountPercent: 0 },
  ]);

  const loadQuotations = useCallback(async () => {
    try {
      const response = await fetchApi("/quotations");
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

  const loadLookups = useCallback(async () => {
    try {
      if (role === "SalesRep") {
        const custRes = await fetchApi("/customers");
        if (custRes.ok) setCustomers((await custRes.json()).data || []);

        const prodRes = await fetchApi("/products");
        if (prodRes.ok) setProducts((await prodRes.json()).data || []);
      }
    } catch (err) {
      console.error(err);
    }
  }, [role]);

  useEffect(() => {
    loadQuotations();
    loadLookups();
  }, [loadQuotations, loadLookups]);

  const handleAddItem = () => {
    setItems([...items, { productId: "", quantity: 1, discountPercent: 0 }]);
  };

  const handleRemoveItem = (index) => {
    setItems(items.filter((_, i) => i !== index));
  };

  const handleItemChange = (index, field, value) => {
    const newItems = [...items];
    newItems[index][field] = value;
    setItems(newItems);
  };

  const handleCreateQuotation = async (e) => {
    e.preventDefault();
    if (!customerId) return setMessage("Please select a customer");

    const validItems = items
      .filter((i) => i.productId && i.quantity > 0)
      .map((i) => ({
        productId: parseInt(i.productId),
        quantity: parseInt(i.quantity),
        discountPercent: parseFloat(i.discountPercent) || 0,
      }));

    if (validItems.length === 0)
      return setMessage("Please add at least one valid product item");

    try {
      const payload = {
        customerId: parseInt(customerId),
        taxRate: parseFloat(taxRate),
        items: validItems,
      };

      const response = await fetchApi("/quotations", {
        method: "POST",
        body: JSON.stringify(payload),
      });

      if (response.ok) {
        setMessage("Quotation created successfully!");
        setCustomerId("");
        setTaxRate(0);
        setItems([{ productId: "", quantity: 1, discountPercent: 0 }]);
        loadQuotations();
      } else {
        const data = await response.json();
        setMessage(`Error: ${data.message || "Failed to create quotation"}`);
      }
    } catch (err) {
      setMessage("Error creating quotation.");
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
        loadQuotations();
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
        loadQuotations();
      } else {
        setMessage("Failed to delete.");
      }
    } catch (err) {
      setMessage("Error deleting.");
    }
  };

  return (
    <div className="page">
      <h2>Quotations</h2>
      {error && <p className="error">{error}</p>}
      {message && (
        <p>
          <b>{message}</b>
        </p>
      )}

      {role === "SalesRep" && (
        <div className="panel">
          <h3>Create Quotation</h3>
          <form onSubmit={handleCreateQuotation}>
            <div className="field">
              <label>Customer: </label>
              <select
                value={customerId}
                onChange={(e) => setCustomerId(e.target.value)}
                required
              >
                <option value="">-- Select Customer --</option>
                {customers.map((c) => (
                  <option key={c.customerId} value={c.customerId}>
                    {c.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="field">
              <label>Tax Rate (%): </label>
              <input
                type="number"
                step="0.01"
                value={taxRate}
                onChange={(e) => setTaxRate(e.target.value)}
                required
              />
            </div>

            <div className="panel-inner">
              <h4>Line Items</h4>
              {items.map((item, index) => (
                <div key={index} className="field-sm">
                  <select
                    value={item.productId}
                    onChange={(e) =>
                      handleItemChange(index, "productId", e.target.value)
                    }
                    required
                  >
                    <option value="">-- Product --</option>
                    {products.map((p) => (
                      <option key={p.productId} value={p.productId}>
                        {p.name} (${p.unitPrice})
                      </option>
                    ))}
                  </select>

                  <label className="ml">Qty: </label>
                  <input
                    type="number"
                    min="1"
                    className="w-qty"
                    value={item.quantity}
                    onChange={(e) =>
                      handleItemChange(index, "quantity", e.target.value)
                    }
                    required
                  />

                  <label className="ml">Discount %: </label>
                  <input
                    type="number"
                    step="0.01"
                    min="0"
                    max="100"
                    className="w-disc"
                    value={item.discountPercent}
                    onChange={(e) =>
                      handleItemChange(index, "discountPercent", e.target.value)
                    }
                  />

                  {items.length > 1 && (
                    <button
                      type="button"
                      onClick={() => handleRemoveItem(index)}
                      className="ml error"
                    >
                      X
                    </button>
                  )}
                </div>
              ))}
              <button
                type="button"
                onClick={handleAddItem}
                className="mt-sm"
              >
                + Add Item
              </button>
            </div>

            <button type="submit">Create Quotation</button>
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
            {role === "Manager" && <th>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {quotations.length === 0 ? (
            <tr>
              <td colSpan={role === "Manager" ? 7 : 6}>
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
