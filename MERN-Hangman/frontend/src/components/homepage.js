// the homepage will ask the user to enter their name,
// then navigate them to the game

import React, { useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { ReactSession } from "react-client-session";


export default function Homepage() {
    const [form, setForm] = useState({
        name: ""
    });

    const navigate = useNavigate(); // to redirect pages

    // update state values
    function updateForm(value) {
        return setForm((prev) => {
            return { ...prev, ...value };
        });
    }

    async function onSubmit(e) {
        // when the user submits the form:
        // store user name

        e.preventDefault();

        ReactSession.setStoreType("sessionStorage");
        ReactSession.set("username", form.name);
        console.log("User: " + ReactSession.get("username"));
        
        navigate("/hangman");
    }


    // form to ask user for name
    return (
        <form onSubmit={onSubmit}>
            <div>Please enter your name: </div>
            <br />
            <label for="name">Name:  </label>
            <input type="text" id="name" value={form.name}
                onChange={(e) => updateForm({ name: e.target.value })}
            />
            <br />
            <input type="submit" value="Submit Name" />
        </form >

    );
};

