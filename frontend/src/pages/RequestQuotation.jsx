import React, { useEffect, useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { fetchApi } from "../api/api";

function RequestQuotation() {
  const [products, setProducts] = useState([]);
  const [message, setMessage] = useState("");
  const [items, setItems] = useState([{ productId: "", quantity: 1 }]);
  const navigate = useNavigate();

  const loadProducts = useCallback(async () => {
    try {
      const prodRes = await fetchApi("/products");
      if (prodRes.ok) setProducts((await prodRes.json()).data || []);
    } catch (err) {
      console.error(err);
    }
  }, []);

  useEffect(() => {
    loadProducts();
  }, [loadProducts]);

  const handleAddItem = () => {
    setItems([...items, { productId: "", quantity: 1 }]);
  };

  const handleRemoveItem = (index) => {
    setItems(items.filter((_, i) => i !== index));
  };

  const handleItemChange = (index, field, value) => {
    const newItems = [...items];
    newItems[index][field] = value;
    setItems(newItems);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const validItems = items
      .filter((i) => i.productId && i.quantity > 0)
      .map((i) => ({
        productId: parseInt(i.productId),
        quantity: parseInt(i.quantity),
        discountPercent: 0,
      }));

    if (validItems.length === 0)
      return setMessage("Please add at least one valid product item");

    try {
      const response = await fetchApi("/quotations", {
        method: "POST",
        body: JSON.stringify({ items: validItems }),
      });

      if (response.ok) {
        setMessage("Quotation request submitted!");
        setItems([{ productId: "", quantity: 1 }]);
        setTimeout(() => navigate("/quotations"), 1000);
      } else {
        const data = await response.json();
        setMessage(`Error: ${data.message || "Failed to submit request"}`);
      }
    } catch (err) {
      setMessage("Error submitting request.");
    }
  };

  return (
    <div className="page">
      <h2>Request Quotation</h2>
      {message && (
        <p>
          <b>{message}</b>
        </p>
      )}

      <div className="panel">
        <form onSubmit={handleSubmit}>
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

          <button type="submit">Submit Request</button>
        </form>
      </div>
    </div>
  );
}

export default RequestQuotation;
