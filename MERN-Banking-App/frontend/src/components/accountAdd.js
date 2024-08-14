import React, { useState } from "react";
//import { Link } from "react-router-dom";
import { useNavigate } from "react-router";

export default function Add() {

  // Use Params
  //const params = useParams();
  // Setup Nav
  const navigate = useNavigate();

  //Check session data for role
  (async () => {
    let role = localStorage.getItem("role");
    if (await (role !== "Administrator" && role !== "Employee")) {
      const message = "Restricted Area. You're not an Administrator or Employee.";
      window.alert(message);
      await navigate(-1);
      return;
    }
  })();

  const [form, setForm] = useState({
    firstname: "",
    lastname: "",
    email: "",
    phone: "",
    password: "",
    confirmPassword: "",
    role: "",
    checkingWithdraw: 0.0,
    checkingDeposit: 0.0,
    checkingTransfer: 0.0,
    savingsWithdraw: 0.0,
    savingsDeposit: 0.0,
    savingsTransfer: 0.0,
    message: "",
  });

  // for error checking
  const [invalidMessage, setInvalidMessage] = useState("");

  // These methods will update the state properties.
  function updateForm(value) {
    return setForm((prev) => {
      return { ...prev, ...value };
    });
  }

  // This function will handle the submission.
  async function onSubmit(e) {
    e.preventDefault();
    let errorCheck, nameCheck, emailCheck, passwordCheck, roleCheck = 1;

    // name check
    if (!form.firstname || !form.lastname) {
      nameCheck = 1;
      setInvalidMessage("Please enter a first and last name");
    } else {
      nameCheck = 0;
    }

    console.log("name: " + nameCheck);

    // role check
    if (form.role === "Administrator" || form.role === "administrator" ||
      form.role === "Employee" || form.role === "employee" ||
      form.role === "Customer" || form.role === "customer") {
      // listed as admin, employee, or customer
      roleCheck = 0;
    }
    else {
      // incorrect role
      console.log("Role error");
      setInvalidMessage("Please enter a role that is administrator, employee or customer");
      roleCheck = 1;
    }

    console.log("role: " + roleCheck);

    if (form.email === null || form.email === "") {
      setInvalidMessage("Please enter an email");
      emailCheck = 1;
    } else {
      emailCheck = 0;
    }

    console.log("email: " + emailCheck);

    if (!form.password) {
      passwordCheck = 1;
      setInvalidMessage("Please enter two matching passwords");
    } else {
      passwordCheck = 0;
    }

    console.log("password: " + passwordCheck);

    if (nameCheck === 0 && emailCheck === 0 &&
      passwordCheck === 0 && roleCheck === 0) {
      console.log("All tests passed");
      setInvalidMessage("");
      errorCheck = 0;
    }

    if (errorCheck === 0) {
      console.log("No errors");
      // When a post request is sent to the create url, we'll add a new record to the database.
      const newPerson = { ...form };
      delete newPerson.message

      const response = await fetch("http://localhost:4000/accounts/add", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newPerson),
      })
        .catch(error => {
          window.alert(error);
          return;
        });

      const user = await response.json();

      console.log("From create: " + user.insertedId);

      setForm({ firstname: "", lastname: "", email: "", phone: "", password: "", confirmPassword: "", savings: 0.0, checking: 0.0, role: "", message: "" })

      if (user.message == null) {
        navigate(-1);
      }

    }
    else {
      console.log("There is an error");
    }
  }



  // This following section will display the form that takes input from the user to update the data.
  return (
    <div>
      <h3>Add Account</h3>
      <form className="content" onSubmit={onSubmit}>
        <div className="form-group m-3">
          <label htmlFor="firstname">First Name: </label>
          <input
            type="text"
            className="form-control border bg-light"
            id="firstname"
            value={form.firstname}
            onChange={(e) => updateForm({ firstname: e.target.value })}
          />
        </div>
        <div className="form-group m-3">
          <label htmlFor="lastname">Last Name: </label>
          <input
            type="text"
            className="form-control border bg-light"
            id="lastname"
            value={form.lastname}
            onChange={(e) => updateForm({ lastname: e.target.value })}
          />
        </div>
        <div className="form-group m-3">
          <label htmlFor="email">Email: </label>
          <input
            type="text"
            className="form-control border bg-light"
            id="email"
            value={form.email}
            onChange={(e) => updateForm({ email: e.target.value })}
          />
        </div>
        <div className="form-group m-3">
          <label htmlFor="phone">Phone: </label>
          <input
            type="text"
            className="form-control border bg-light"
            id="lastname"
            value={form.phone}
            onChange={(e) => updateForm({ phone: e.target.value })}
          />
        </div>
        <div className="form-group m-3">
          <label htmlFor="password">Password: </label>
          <input
            type="text"
            className="form-control border bg-light"
            id="password"
            value={form.password}
            onChange={(e) => updateForm({ password: e.target.value })}
          />
        </div>
        <div className="form-group m-3">
          <label htmlFor="password">Confirm Password: </label>
          <input
            type="text"
            className="form-control border bg-light"
            id="confirm-password"
            value={form.confirmPassword}
            onChange={(e) => updateForm({ confirmPassword: e.target.value })}
          />
        </div>
        <div className="form-group m-3">
          <label htmlFor="role">Role: </label>
          <input
            type="text"
            className="form-control border bg-light"
            id="role"
            value={form.role}
            onChange={(e) => updateForm({ role: e.target.value })}
          />
        </div>
        <br />
        <div className="invalidMessage">{form.message}</div>
        <div className="invalidMessage">{invalidMessage}</div>
        <br />
        <br />
        <div>
          <input
            type="submit"
            value="Add Account"
            className="submit-button account-button"
          />
        </div>
      </form>
      {/* end of card */}
      <div className="footer">
        <p>Copyright &copy; 2024 Team Gecko</p>
      </div>
    </div>
  );
}
