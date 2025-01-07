import "./Create.css";
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";


export default function AddWord() {
    const [form, setForm] = useState({
        english: "",
        spanish: "",
        partOfSpeech: "", // tells if noun, adjective, etc
        tense: "",
        list: "", // tells what list to put word in
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

        console.log("In onSubmit - add word");
        console.log(form);

        let englishCheck, spanishCheck, partOfSpeechCheck, listCheck = 1;

        // english check
        if (!form.english) {
            englishCheck = 1;
            setInvalidMessage("Please enter an English word.");
        } else {
            englishCheck = 0;
        }

        if (!form.spanish) {
            spanishCheck = 1;
            setInvalidMessage("Please enter a Spanish word.");
        } else {
            spanishCheck = 0;
        }

        if (!form.partOfSpeech) {
            partOfSpeechCheck = 1;
            setInvalidMessage("Please enter if this is a noun, verb, adjective, etc.");
        } else {
            partOfSpeechCheck = 0;
        }

        if (!form.list) {
            listCheck = 1;
            setInvalidMessage("Please enter the list this should be a part of.");
        } else {
            listCheck = 0;
        }


        console.log(form);


        if (englishCheck === 0
            && spanishCheck === 0
            && partOfSpeechCheck === 0
            && listCheck === 0) {
            console.log("No errors in form");
            const newWord = { ...form };
            delete newWord.message;

            console.log("New word");
            console.log(newWord);
            async function createWord() {
                try {
                    const response = await fetch("http://localhost:4000/get-word/create", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(newWord),
                    }).catch((error) => {
                        window.alert(error);
                        return;
                    });

                    // console.log("Response");
                    // console.log(response);

                    // if (!response.ok) {
                    // 	const message = `An error has occurred: ${response.statusText}`;
                    // 	window.alert(message);
                    // 	return;
                    // }

                    const word = await response.json();

                    console.log("From create word: ");
                    // console.log(word);

                    setForm({ english: "", spanish: "", partOfSpeech: "", list: "", tense: "" });
                    if (word.message == null) {
                        //navigate(-1);
                        console.log("Word message null");
                    } else {
                        console.log("There is an error");
                    }
                }
                catch (error) {
                    window.alert(error);
                    return;
                }

            }
            createWord();

        }
    }

    return (
        <div className="container">
            <h3>New Word</h3>
            <form onSubmit={handleSubmit}>
                <label htmlFor="english" className="form-label">
                    English
                </label>
                <input
                    id="english"
                    type="text"
                    placeholder="Please enter an English translation."
                    name="english"
                    value={form.english}
                    onChange={(e) => updateForm({ english: e.target.value })}
                    required
                />

                <label htmlFor="spanish" className="form-label">
                    Spanish
                </label>
                <input
                    id="spanish"
                    type="text"
                    placeholder="Please enter a Spanish translation."
                    value={form.spanish}
                    onChange={(e) => updateForm({ spanish: e.target.value })}
                    required
                />

                <label htmlFor="partOfSpeech" className="form-label">Part Of Speech</label>
                <input
                    id="partOfSpeech"
                    type="text"
                    placeholder="Please enter a part of speech"
                    value={form.partOfSpeech}
                    onChange={(e) => updateForm({ partOfSpeech: e.target.value })} required />

                <label htmlFor="tense" className="form-label">Verb tense</label>
                <input
                    id="tense"
                    type="text"
                    placeholder="Please enter a tense"
                    value={form.tense}
                    onChange={(e) => updateForm({ tense: e.target.value })} />

                <label htmlFor="list" className="form-label">
                    List
                </label>
                <input
                    id="list"
                    type="text"
                    placeholder="Please enter the list type."
                    name="list"
                    value={form.list}
                    onChange={(e) => updateForm({ list: e.target.value })}
                    required
                />
                <br />
                <button type="submit" value="addWord" onClick={handleSubmit}>Add Word</button>
            </form>
            {invalidMessage}
        </div>
    );
}
