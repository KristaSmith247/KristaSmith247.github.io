import React, { useState } from "react";
import { useNavigate } from "react-router";
import { Link } from "react-router-dom";
import "./Login.css";

export default function Login() {
	const [form, setForm] = useState({
		username: "",
		password: "",
		message: "",
	});
	const [invalidMessage, setInvalidMessage] = useState("");
	const [isLoggedIn, setIsLoggedIn] = useState(false);

	const navigate = useNavigate();

	function updateForm(value) {
		return setForm((prev) => {
			return { ...prev, ...value };
		});
	}

	async function onSubmit(e) {
		e.preventDefault();

		const existingAccount = { ...form };
		console.log(form);
		const response = await fetch("http://localhost:4000/accounts/login", {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
			},
			body: JSON.stringify(existingAccount),
		}).catch((error) => {
			console.log("ERROR: Unable to fetch");
			window.alert(error);
			return;
		});

		const account = await response.json();

		if (account.message == null) {
			localStorage.setItem("username", account._id);
			localStorage.setItem("type", account.type);

			console.log("Logged in successfully");
			navigate("/account/" + account._id);
		}; // end of account

		setInvalidMessage(
			"Invalid username or password. Please enter and try again."
		);
		setForm({ username: "", password: "", message: account.message });

	}; // end of onSubmit function

	return (
		<>
			<div className="container-heading">
				<h1 className="heading-title">Spanish-English Learning App</h1>
			</div>
			<div className="container">
				<h3>Login</h3>
				<form onSubmit={onSubmit}>
					<label htmlFor="username" className="form-label">
						Username
					</label>
					<input type="text" placeholder="Please enter username"
						value={form.username} onChange={(e) => updateForm({ username: e.target.value })} />

					<label htmlFor="password" className="form-label">
						Password
					</label>
					<input type="text" placeholder="Please enter password"
						value={form.password} onChange={(e) => updateForm({ password: e.target.value })} />

					<br />

					<button type="submit">Submit</button>
				</form>
				<label>{invalidMessage}</label>
				<br />
				<label>Don't have an account?</label>
				<button>
					<a href="/create">Sign Up</a>
				</button>
				<div className="footer">
					<p>Copyright &copy; 2024 Language App</p>
				</div>

				{/* <label>TESTING PURPOSES</label>
				<button>
					<a href="/addword">Add Word</a>
				</button> */}
			</div>
		</>
	); // end of return
} // end of function
