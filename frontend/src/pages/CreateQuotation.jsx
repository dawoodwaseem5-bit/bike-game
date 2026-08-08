import React, { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { fetchApi } from "../api/api";

function CreateQuotation() {
  const [customers, setCustomers] = useState([]);
  const [products, setProducts] = useState([]);
  const [message, setMessage] = useState("");
  const [customerId, setCustomerId] = useState("");
  const [taxRate, setTaxRate] = useState(0);
  const [items, setItems] = useState([
    { productId: "", quantity: 1, discountPercent: 0 },
  ]);
  const navigate = useNavigate();

  const loadLookups = useCallback(async () => {
    try {
      const custRes = await fetchApi("/customers");
      if (custRes.ok) setCustomers((await custRes.json()).data || []);

      const prodRes = await fetchApi("/products");
      if (prodRes.ok) setProducts((await prodRes.json()).data || []);
    } catch (err) {
      console.error(err);
    }
  }, []);

  useEffect(() => {
    loadLookups();
  }, [loadLookups]);

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
      const response = await fetchApi("/quotations", {
        method: "POST",
        body: JSON.stringify({
          customerId: parseInt(customerId),
          taxRate: parseFloat(taxRate),
          items: validItems,
        }),
      });

      if (response.ok) {
        setMessage("Quotation created successfully!");
        setTimeout(() => navigate("/quotations"), 1000);
      } else {
        const data = await response.json();
        setMessage(`Error: ${data.message || "Failed to create quotation"}`);
      }
    } catch (err) {
      setMessage("Error creating quotation.");
    }
  };

  return (
    <div className="page">
      <h2>Create Quotation</h2>
      {message && (
        <p>
          <b>{message}</b>
        </p>
      )}

      <div className="panel">
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
            <button type="button" onClick={handleAddItem} className="mt-sm">
              + Add Item
            </button>
          </div>

          <button type="submit">Create Quotation</button>
        </form>
      </div>
    </div>
  );
}

export default CreateQuotation;
