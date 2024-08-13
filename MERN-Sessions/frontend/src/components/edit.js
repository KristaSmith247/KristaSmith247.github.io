// create frontend way to edit records

import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router"; // send us to a new page

export default function Edit() {
    const [form, setForm] = useState({
        name: "",
        position: "",
        level: "",
    });

    const params = useParams(); // to use id obj
    const navigate = useNavigate();

    useEffect(() => {
        async function fetchData() {
            // get id for particular object in URL, convert into string
            const id = params.id.toString();
            const response = await fetch(`http://localhost:4000/record/${id}`); // connect to backend here
            if (!response.ok) {
                // get backend's json response
                const message = `An error occurred: ${response.statusText}`;
                window.alert(message);
                return;
            }
            const responseRecord = await response.json(); // wait for results to come back
            if (!responseRecord) {
                // check to make sure record exists
                window.alert(`Record with id ${id} not found`);
                navigate("/");
            }
            // put json list of records into records state
            setForm(responseRecord); // set state
            return;
        }
        fetchData();
        return;
    }, [params.id, navigate]
    );


    function updateForm(jsonObj) {
        return setForm((prevJsonObj) => {
            // take whatever was in the form previously, pass it in as prev
            // unpack into key-value pairs
            // then add to it the jsonObj
            return { ...prevJsonObj, ...jsonObj };
        });
    }


    async function onSubmit(e) {
        // create this function to determine what to do on submittance
        e.preventDefault(); // don't reload the page
        const editedPerson = {
            name: form.name,
            position: form.position,
            level: form.level
        };
        await fetch(`http://localhost:4000/update/${params.id}`, {
            method: "PUT", // send a new item into db
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(editedPerson),
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
            <h3>Update Record</h3>
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
                        value="Update Record"
                    />
                </div>
            </form>
        </div>
    );
}
