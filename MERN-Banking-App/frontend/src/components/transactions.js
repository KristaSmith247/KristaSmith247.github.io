// src/components/TransactionPage.js

import React, { useState, useEffect } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import axios from "axios";
import "./homepage.css";

const fetchTransactionHistory = async (customerID) => {
  const response = await axios.get(`http://localhost:4000/transactions/${customerID}`);
  return response.data;
};

const fetchIndividualAccountHistory = async (customerID, accountType) => {
  const response = await axios.get(`http://localhost:4000/transactions/${customerID}/${accountType}`);
  return response.data;
};

const TransactionPage = () => {
  const [transactions, setTransactions] = useState([]);
  const [selectedAccount, setSelectedAccount] = useState('all');
  const { id } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    async function getTransactions() {
      const data = await fetchTransactionHistory(id);
      setTransactions(data);
    }

    getTransactions();
  }, [id]);

  const handleAccountChange = async (e) => {
    const accountType = e.target.value;
    console.log(accountType)
    setSelectedAccount(accountType);
    if (accountType === 'all') {
      const data = await fetchTransactionHistory(id);
      setTransactions(data);
    } else {
      const data = await fetchIndividualAccountHistory(id, accountType);
      setTransactions(data);
    }
  };

  return (
    <div>
      <h3 className="m-3">Transaction History</h3>
      <div>
        <label htmlFor="account-select">Filter by account:</label>
        <select id="account-select" value={selectedAccount} onChange={handleAccountChange}>
          <option value="all">All Accounts</option>
          <option value="savings">Savings</option>
          <option value="checking">Checking</option>
          <option value="investment">Investment</option>
        </select>
      </div>
      
      <div>
        <h4>Transaction Details</h4>
        <table className="transaction-table">
          <thead>
            <tr>
              <th>Date</th>
              <th>Type</th>
              <th>Account</th>
              <th>Amount</th>
              <th>Description</th>
            </tr>
          </thead>
          <tbody>
            {transactions.map((tx, index) => (
              <tr key={index}>
                <td>{new Date(tx.date).toLocaleString()}</td>
                <td>{tx.type}</td>
                <td>{tx.account || '-'}</td>
                <td>${tx.amount.toFixed(2)}</td>
                <td>{tx.description}</td>
              </tr>
            ))}
          </tbody>
        </table>
        <br/>
        <button
          onClick={() => {
            navigate(-1);
          }}
        >
          <p className="submit-button account-button">Back To Dashboard</p>
        </button>
      </div>
    </div>
  );
};

export default TransactionPage;
