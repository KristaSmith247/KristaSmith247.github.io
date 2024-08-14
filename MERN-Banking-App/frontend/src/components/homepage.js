// This file was created to make the frontend folder
// the Login function will allow users to login

import React, { useState } from "react";
//import { NavLink } from "react-router-dom";
import { useNavigate } from "react-router";
//import { sha256 } from "js-sha256";

import "./homepage.css";

export default function Login() {
    const [form, setForm] = useState({
        // set form to blanks as default
        email: "",
        password: "",
        message: ""
    });

    const [invalidMessage, setInvalidMessage] = useState(''); // message to show incorrect username/password

    const navigate = useNavigate(); // allow user to navigate to other pages

    function updateForm(value) {
        // update variables
        return setForm((prev) => {
            return { ...prev, ...value };
        });
    }

    async function onSubmit(e) {
        // when form is submitted, send info to db
        e.preventDefault();

        // hash the password entered in the form
        console.log("TESTING: where am i");
        const sha256 = require('js-sha256');
        const toBeHashed = form.password;
        console.log("To be hashed: " + toBeHashed);
        const hash = sha256(toBeHashed);
        console.log("Here's my hash: " + hash);

        // change the form password to what the hashed version is
        form.password = hash;

        const existingPerson = { ...form };

        const response = await fetch("http://localhost:4000/accounts/login",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(existingPerson),
            })
            .catch(error => {
                console.log("ERROR - UNABLE TO FETCH");
                // display error message if unable to fetch info
                window.alert(error);
                return;
            });

        const user = await response.json();

        console.log("User: " + user);
        console.log("email: " + user.email);
        console.log("password: " + user.password);
        console.log("hashed: " + hash);
        console.log("message: " + user.message);

        // reset form to blank, set message to username
        setForm({ email: "", password: "", message: user.message });

        if (user.message == null) {
            // Set session data
            localStorage.setItem("username", user._id);
            localStorage.setItem("role", user.role);


            // navigate to user account
            console.log("successful login");
            //console.log("About to navigate to account");
            navigate("/account/" + user._id);


        }
        setInvalidMessage("Invalid email or password. Please correct and try again.");

    }

    return (
        <div>
            <h3>Welcome to Gecko Banking!</h3>
            <div className="flex-container content">
                <img className="" src="https://images.unsplash.com/photo-1634207138281-3321b35e9662?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D" class="card-img-top" alt="A gecko"
                />
                <div className="">
                    <form onSubmit={onSubmit}>

                        <label htmlFor="email" className="form-label">Email</label>
                        <input type="text" id="email"
                            placeholder="Please enter your email"
                            value={form.email}
                            onChange={(e) => updateForm({ email: e.target.value })}
                        />

                        <div>
                            <label htmlFor="password" className="form-label">Password</label>
                            <input type="text" id="password" placeholder="Please enter your password"
                                value={form.password}
                                onChange={(e) => updateForm({ password: e.target.value })}
                            />
                        </div>
                        <div className="invalidMessage">{invalidMessage}</div>
                        <div>
                            <input type="submit" value="Login" className="submit-button account-button" />
                        </div>
                    </form>
                </div>
            </div>
            {/* end of card */}
            <div className="footer">
                <p>Copyright &copy; 2024 Team Gecko</p>
                <p>Photo by David Clode</p>
            </div>
        </div>
    );
}

