import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router";
import { sha256 } from "js-sha256";

export default function Edit() {
  const [form, setForm] = useState({
    firstname: "",
    lastname: "",
    email: "",
    phone: "",
    password: "",
    role: "",
  });

  // set error message to blank as default
  const [invalidMessage, setInvalidMessage] = useState('');

  const params = useParams();
  // Setup Nav
  const navigate = useNavigate();

  useEffect(() => {
    async function fetchData() {
      const id = params.id.toString();
      const response = await fetch(`http://localhost:4000/accounts/${params.id}`);

      if (!response.ok) {
        const message = `An error has occurred: ${response.statusText}`;
        window.alert(message);
        return;
      }

      const account = await response.json();
      if (!account) {
        window.alert(`Account with id ${id} not found`);
        return;
      }

      // fetch retrieves the hashed password -
      // user will not know what hashed password is,
      // so show a blank instead
      account.password = "";

      setForm(account);
    }

    fetchData();

    return;
  }, [params.id, navigate]);

  // These methods will update the state properties.
  function updateForm(value) {
    return setForm((prev) => {
      return { ...prev, ...value };
    });
  }

  function showRole() {
    if (localStorage.getItem("role") === "Administrator") {
      return (
        <div className="form-group m-3">
          <label htmlFor="roles">Role: </label>
          <input
            type="text"
            className="form-control border bg-light"
            id="role"
            value={form.role}
            onChange={(e) => updateForm({ role: e.target.value })}
          />
        </div>
      )
    }
  }

  async function onSubmit(e) {
    e.preventDefault();
    if ((form.confirmPassword == form.password)) {

      if ((form.confirmPassword != '' && form.password != '')) {
        // if the user changed their password:

        //console.log("TESTING: in account update");
        const sha256 = require('js-sha256'); // used for hashing
        const toBeHashed = form.password;
        // console.log("To be hashed: " + toBeHashed);
        const hash = sha256(toBeHashed);
        //console.log("Here's my hash: " + hash);

        // change the form password to what the hashed version is
        form.password = hash;

        const editedPerson = {
          firstname: form.firstname,
          lastname: form.lastname,
          email: form.email,
          phone: form.phone,
          password: form.password,
          role: form.role,
        };
        // This will send a post request to update the data in the database.
        await fetch(`http://localhost:4000/update/${params.id}`, {
          method: "PUT",
          body: JSON.stringify(editedPerson),
          headers: {
            'Content-Type': 'application/json'
          },
        });

        navigate(-1);
      } // end of if the user changed their password

    }

    setInvalidMessage("Please update or confirm password - ensure both entries match");
  }

  // This following section will display the form that takes input from the user to update the data.
  return (
    <div>
      <h3>Update Account</h3>
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
            id="phone"
            value={form.phone}
            onChange={(e) => updateForm({ phone: e.target.value })}
          />
        </div>
        <div className="form-group m-3">
          <label htmlFor="password">Password: </label>
          <input
            type="text"
            id="password"
            placeholder="Update password here"
            className="form-control border bg-light"
            value={form.password}
            onChange={(e) => updateForm({ password: e.target.value })}
          />
        </div>
        <div className="form-group m-3">
          <label htmlFor="confirm-password">Confirm Password: </label>
          <input
            type="text"
            id="confirm-password"
            placeholder="Confirm new password"
            className="form-control border bg-light"
            value={form.confirmPassword}
            onChange={(e) => updateForm({ confirmPassword: e.target.value })}
          />
        </div>
        {showRole()}
        <div className="invalidMessage">{invalidMessage}</div>
        
        <br />

        <div className="form-group m-3">
          <input
            type="submit"
            value="Update Account"
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
