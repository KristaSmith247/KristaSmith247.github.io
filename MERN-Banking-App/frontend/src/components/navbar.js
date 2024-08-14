import React from "react";
// We import NavLink to utilize the react router.
import { NavLink } from "react-router-dom";
import { useNavigate } from "react-router";


// Here, we display our Navbar
export default function Navbar() {

  const navigate = useNavigate();

  // If user logged in, show navbar
  function showNav() {
    let sessionName = localStorage.getItem("username")
    if (sessionName !== "" && sessionName !== null) {
      return (
        <div className="nav-div">
          <div className="nav-button-div">
            <button className="submit-button account-button nav-button"
                onClick={() => {
                navigate(-1);
              }}
            >
              <p className="nav-p">&#8656;Back</p>
            </button>
            <button className="submit-button account-button nav-button"
                onClick={() => {
                navigate("/account/" + sessionName);
              }}
            >
              <p className="nav-p">My Account</p>
            </button>
            <button onClick={logout} className="submit-button account-button nav-button" type="button">
              <p className="nav-p">Logout</p>
            </button>
          </div>
        </div>
      )
    }
  }

  // Remove session name
  function logout() {
    localStorage.removeItem("username");
    localStorage.removeItem("role");
    navigate("/");
  }

  return (
    <div>
      {showNav()}
    </div>
  );
}
