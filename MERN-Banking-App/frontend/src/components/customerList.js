import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router";

import "./homepage.css";
 
const Account = (props) => (
 <tr>
  <td>{props.account.firstname}</td>
  <td>{props.account.lastname}</td>
  <td>{props.account.email}</td>
  <td>{props.account.phone}</td>
  <td>$ {props.account.savings}</td>
  <td>$ {props.account.checking}</td>
  <td>$ {props.account.investment}</td>
  <td>{props.account.role}</td>
  <td>
      <Link className="submit-button account-button" to={`/account/${props.account._id}`}>View</Link>
      <Link className="submit-button account-button" to={`/edit/${props.account._id}`}>Edit</Link>
      <button
        onClick={() => {
          props.deleteAccount(props.account._id);
        }}
      >
        <p className="submit-button account-button">Delete</p>
      </button>
   </td>
 </tr>
);
 
export default function CustomerList() {

  // Use Params
  //const params = useParams();
  // Setup Nav
  const navigate = useNavigate();

  //Check session data for role
  (async () => {
    let role = localStorage.getItem("role");
    if (await (role !== "Administrator" && role !== "Employee")) {
      const message = "Restricted Area. You're not an Employee.";
      window.alert(message);
      await navigate(-1);
      return;
    }
  })();

 const [account, setAccounts] = useState([]);
 
 // This method fetches the records from the database.
 useEffect(() => {
   async function getAccounts() {
     const response = await fetch(`http://localhost:4000/customers/`);
 
     if (!response.ok) {
       const message = `An error occurred: ${response.statusText}`;
       window.alert(message);
       return;
     }
 
     const accounts = await response.json();
     setAccounts(accounts);
   }
 
   getAccounts();
 
   return;
 }, [account.length]);

 // This method will delete a record
 async function deleteAccount(id) {
  await fetch(`http://localhost:4000/${id}`, {
    method: "DELETE"
  });

  const newAccounts = account.filter((el) => el._id !== id);
  setAccounts(newAccounts);
}
 
 
 // This method will map out the records on the table
 function accountList() {
   return account.map((account) => {
     return (
       <Account
          account={account}
          deleteAccount={() => deleteAccount(account._id)}
          key={account._id}
       />
     );
   });
 }
 
 // This following section will display the table with the records of individuals.
 return (
    <div>
      <h3 className="m-3">Customer List</h3>
      <table className="account-table content" style={{ marginTop: 20 }}>
        <thead>
          <tr className="account-tr">
            <th>First</th>
            <th>Last</th>
            <th>Email</th>
            <th>Phone</th>
            <th>Savings</th>
            <th>Checking</th>
            <th>Investment</th>
            <th>Role</th>
          </tr>
        </thead>
        <tbody>{accountList()}</tbody>
      </table>
      {/* end of card */}
      <div className="footer">
        <p>Copyright &copy; 2024 Team Gecko</p>
      </div>
    </div>
 );
}