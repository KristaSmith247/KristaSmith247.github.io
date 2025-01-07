import "./Create.css";
import Footer from "./Footer";
import React, { useState } from "react";
import { useNavigate, Link } from "react-router-dom";

export default function Create() {
	const [form, setForm] = useState({
		username: "",
		password: "",
		confirmPassword: "",
		type: "",
		message: "", // return if sucessful or error
	});
	const [invalidMessage, setInvalidMessage] = useState("");
	const navigate = useNavigate();

	function updateForm(value) {
		return setForm((prev) => {
			return { ...prev, ...value };
		});
	}

	async function handleSubmit(e) {
		e.preventDefault();
		let usernameCheck, passwordCheck, typeCheck = 1;

		console.log("In onSubmit");
		console.log(form);

		// username check
		if (!form.username) {
			usernameCheck = 1;
			setInvalidMessage("Please enter a username");
		} else {
			console.log("username check cleared");
			usernameCheck = 0;
		}

		if (!form.type) {
			// if no form type, set account to general
			form.type = "general";
			console.log("Set form to general");
			typeCheck = 0;
		} else {
			// form already has an account type
			typeCheck = 0;
		}

		console.log(form);

		// check to make sure passwords match
		if (JSON.stringify(form.password.trim()) === JSON.stringify(form.confirmPassword.trim()) && form.password != "") {
			passwordCheck = 0;
			console.log("password check cleared");
		} else {
			console.log("Password error");
			setInvalidMessage("Please enter two matching passwords.");
			passwordCheck = 1;
		}

		console.log("About to hit fetch");
		if (usernameCheck === 0 && passwordCheck === 0 && typeCheck === 0) {
			console.log("No errors in form");
			const newAccount = { ...form };
			delete newAccount.message;

			console.log("New account");
			console.log(newAccount);

			const response = await fetch("http://localhost:4000/accounts/create", {
				method: "POST",
				headers: {
					"Content-Type": "application/json",
				},
				body: JSON.stringify(newAccount),
			}).catch((error) => {
				window.alert(error);
				return;
			});

			const account = await response.json();

			console.log("From create: ");

			setForm({ username: "", password: "", confirmPassword: "", type: "" });

			if (account.message == null) {
				console.log("Account message null");
			} else {
				console.log("There is an error");
				setInvalidMessage(account.message);
			}
		}
	}


	return (
		<>
			<div className="container-heading">
				<h1>Create Account</h1>
				<button className="heading-button"><Link to={"/"}>Home</Link></button>
			</div>
			<div className="container">
				<h3>New Account</h3>
				<form onSubmit={handleSubmit}>
					<label htmlFor="username" className="form-label">
						Username
					</label>
					<input
						id="username"
						type="text"
						placeholder="Please enter username"
						name="username"
						value={form.username}
						onChange={(e) => updateForm({ username: e.target.value })}
						required
					/>

					<label htmlFor="password" className="form-label">
						Password
					</label>
					<input
						id="password"
						type="text"
						placeholder="Please enter password"
						value={form.password}
						onChange={(e) => updateForm({ password: e.target.value })}
						required
					/>

					<label htmlFor="confirmPassword" className="form-label">Confirm Password</label>
					<input
						id="confirmPassword"
						type="text"
						placeholder="Please confirm password"
						value={form.confirmPassword}
						onChange={(e) => updateForm({ confirmPassword: e.target.value })} required />
					<label htmlFor="type" className="form-label">
						Account Type
					</label>
					<select
						name="type"
						id="type"
						onChange={(e) => updateForm({ type: e.target.value })}
						selected="selected"
					>
						<option value="general">General</option>
						<option value="business">Business</option>
						<option value="student">Student</option>
						<option value="travel">Travel</option>
					</select>
					<br />
					<button type="submit" value="Create Account" onClick={handleSubmit}>Create Account</button>
				</form>
				<p>{invalidMessage}</p>
			</div>

			<br />

			<Footer />
		</>

	);
}
