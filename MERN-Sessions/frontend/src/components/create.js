// create a frontend Create route (add new records)
import React, { useState } from "react";
import { useNavigate } from "react-router"; // send us to a new page

export default function Create() {
    const [form, setForm] = useState({
        name: "",
        position: "",
        level: "",
    });

    const navigate = useNavigate();

    function updateForm(jsonObj) {
        return setForm((prevJsonObj) => {
            // take whatever was in the form previously, pass it in as prev
            // unpack into key-value pairs
            // then add to it the jsonObj
            // 11:00 min in
            return { ...prevJsonObj, ...jsonObj };
        });
    }


    async function onSubmit(e) {
        // create this function to determine what to do on submittance
        e.preventDefault(); // don't reload the page
        const newPerson = { ...form }; // unpack form json obj, repack & put into new person (clone json obj)
        await fetch("http://localhost:4000/record/add", {
            method: "POST", // send a new item into db
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(newPerson),
        })
            .catch(error => {
                window.alert(error);
                return;
            });
        // clear form so you don't double submit
        setForm({ name: "", position: "", level: "" });
        navigate("/"); // navigate to homepage
    }

    return (
        <div>
            <h3>Create Record</h3>
            <form onSubmit={onSubmit}>
                <div>
                    <label>Name: </label>
                    <input
                        type="text"
                        id="name"
                        value={form.name}
                        onChange={(e) => updateForm({ name: e.target.value })}
                    />
                </div>
                <div>
                    <label>Position: </label>
                    <input
                        type="text"
                        id="position"
                        value={form.position}
                        onChange={(e) => updateForm({ position: e.target.value })}
                    />
                </div>
                <div>
                    <label>Level: </label>
                    <input
                        type="text"
                        id="level"
                        value={form.level}
                        onChange={(e) => updateForm({ level: e.target.value })}
                    />
                </div>
                <br />
                <div>
                    <input
                        type="submit"
                        value="Create Record"
                    />
                </div>
            </form>
        </div>
    );
}

