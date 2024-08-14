import React, { useState, useEffect } from "react";
import { useParams, useNavigate, Link } from "react-router-dom";
import axios from "axios";
import "./homepage.css";

const fetchUserData = async (customerID) => {
  const response = await axios.get(`http://localhost:4000/accounts/${customerID}`);
  const userData = response.data;
  if (!Array.isArray(userData.transactions)) {
    userData.transactions = [];
  }
  return userData;
};

const Customer = () => {
  const [userData, setUserData] = useState({
    firstname: "",
    lastname: "",
    email: "",
    phone: "",
    savings: 0,
    checking: 0,
    investment: 0,
    transactions: [],
    customerId: "" // Ensure this is populated
  });
  const [transaction, setTransaction] = useState({
    type: 'Deposit',
    account: 'savings',
    amount: '',
  });
  const { id } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    async function getUserData() {
      try {
        const data = await fetchUserData(id);
        setUserData(data);
      } catch (error) {
        console.error("Failed to fetch customer data:", error);
        window.alert("Failed to fetch customer data");
        navigate("/");
      }
    }

    getUserData();
  }, [id, navigate]);

  const handleTransactionChange = (e) => {
    setTransaction({ ...transaction, [e.target.name]: e.target.value });
  };

  const handleTransactionSubmit = async (e) => {
    e.preventDefault();
    const newTransaction = {
      customerId: userData.customerId,
      date: new Date().toISOString().split('T')[0],
      ...transaction,
      amount: parseFloat(transaction.amount),
      accountType: transaction.account,
    };
    console.log('here');

    try {
      console.log('Submitting transaction:', newTransaction);
      const response = await axios.post(`http://localhost:4000/accounts/${transaction.type.toLowerCase()}`, newTransaction);
      
      console.log('Transaction Response:', response.data);

      setUserData(prevUserData => {
        const transactions = Array.isArray(prevUserData.transactions) ? prevUserData.transactions : [];
        return {
          ...prevUserData,
          [transaction.account]: transaction.type === 'Deposit'
            ? prevUserData[transaction.account] + newTransaction.amount
            : prevUserData[transaction.account] - newTransaction.amount,
          transactions: [...transactions, newTransaction],
        };
      });

      setTransaction({ type: 'Deposit', account: 'savings', amount: '' });
    } catch (error) {
      console.error("Failed to create transaction:", error.response ? error.response.data : error.message);
      window.alert(`Failed to create transaction: ${error.response ? error.response.data.message : error.message}`);
    }
  };

  return (
    <div>
      <h3 className="m-3">{userData.firstname} {userData.lastname}'s Account</h3>
      <div>
        <h4>Account Balances</h4>
        <p>Savings: ${userData.savings.toFixed(2)}</p>
        <p>Checking: ${userData.checking.toFixed(2)}</p>
        <p>Investment: ${userData.investment.toFixed(2)}</p>
      </div>

      <div>
        <h4>Make a Transaction</h4>
        <form onSubmit={handleTransactionSubmit} className="transaction-form">
          <select
            name="type"
            value={transaction.type}
            onChange={handleTransactionChange}
          >
            <option value="Deposit">Deposit</option>
            <option value="Withdraw">Withdraw</option>
          </select>
          <select
            name="account"
            value={transaction.account}
            onChange={handleTransactionChange}
          >
            <option value="savings">Savings</option>
            <option value="checking">Checking</option>
            <option value="investment">Investment</option>
          </select>
          <input
            type="number"
            name="amount"
            value={transaction.amount}
            onChange={handleTransactionChange}
            placeholder="Amount"
            className="transaction-amount"
          />
          <button type="submit" className="submit-button account-button">Submit</button>
        </form>
      </div>

      <div>
        <br />
        <br />
        <Link to={`/transactions/${userData.customerId}`} className="submit-button account-button">View Transaction History</Link>
      </div>
    </div>
  );
};

export default Customer;
