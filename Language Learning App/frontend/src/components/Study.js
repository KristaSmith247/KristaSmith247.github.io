import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Footer from "./Footer";
import "./Study.css";

export default function Study() {
    const params = useParams();
    const [accountInfo, setAccountInfo] = useState({
        username: "",
        password: "",
        type: "",
    });
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    // get words from database
    const [click, setClick] = useState(0);
    const [url, setUrl] = useState("");
    const [isVerb, setIsVerb] = useState(false);
    const navigate = useNavigate();

    // set words from database
    const [spanishWord, setSpanishWord] = useState([]);
    const [englishWord, setEnglishWord] = useState([]);
    const [tense, setTense] = useState([]);

    // determine which translations are shown
    const [showSpanish, setShowSpanish] = useState(true);
    const [showEnglish, setShowEnglish] = useState(true);
    const [showVerb, setShowVerb] = useState(false);

    // get account information on load
    useEffect(() => {
        if (localStorage.getItem("username") === null || localStorage.getItem("username") === "") {
            setIsLoggedIn(false);
        } else {
            setIsLoggedIn(true);
        }

        async function fetchData() {
            // fetch account data based on id
            const id = params.id.toString();
            const response = await fetch(`http://localhost:4000/accounts/${id}`);

            if (!response.ok) {
                const message = `An error has occurred: ${response.statusText}`;
                window.alert(message);
                return;
            }
            const account = await response.json();
            if (!account) {
                window.alert(`Account with id ${id} not found`);
                navigate("/");
                return;
            }
            setAccountInfo(account);

            if (accountInfo.type == "" || accountInfo.type == null) {
                // ensure each account has a type
                accountInfo.type = "general";
            }

            // get vocab list based on account type
            if (accountInfo.type === "general") {
                setUrl("http://localhost:4000/get-word/general");
            } else if (accountInfo.type === "business") {
                setUrl("http://localhost:4000/get-word/business");
            }
            else if (accountInfo.type === "student") {
                setUrl("http://localhost:4000/get-word/student");
            }
            else if (accountInfo.type === "travel") {
                setUrl("http://localhost:4000/get-word/travel");
            }
        }

        fetchData();
        console.log("url: ");
        console.log(url);
    }, [params.id]);

    function changeToVerbs() {
        // get random verb on click
        setUrl("http://localhost:4000/get-word/verb");
        setClick(click + 1);
        setIsVerb(true);
    }

    function onClickHandler() {
        // get random word on click
        setClick(click + 1);
        if (isVerb) {
            setUrl(`http://localhost:4000/get-word/${accountInfo.type}`);
            setIsVerb(false);
        }
    }

    // the following 3 functions are used when clicking on buttons to hide/show words
    function changeSpanishDisplay() {
        if (showSpanish) {
            setShowSpanish(false);
        } else {
            setShowSpanish(true);
        }
    }

    function changeEnglishDisplay() {
        if (showEnglish) {
            setShowEnglish(false);
        } else {
            setShowEnglish(true);
        }
    }

    function changeVerbDisplay() {
        if (showVerb) {
            setShowVerb(false);
        } else {
            setShowVerb(true);
        }
    }

    useEffect(() => {
        // function to get random word based on account type
        async function getWord() {
            console.log(url);
            const response = await fetch(`${url}`);
            if (!response.ok) {
                const message = `An error occurred: ${response.statusText}`;
                window.alert(message);
                return;
            }

            const word = await response.json();
            setSpanishWord(word.spanish);
            setEnglishWord(word.english);
            if (isVerb) {
                setTense(word.tense);
            }
            console.log(word.english);
            console.log(word.spanish);
        }
        getWord();

    }, [click]);

    function capitalizeAccountType() {
        // capitalize words (to look nicer)
        return accountInfo.type.charAt(0).toUpperCase() + accountInfo.type.slice(1);
    }

    const logoutHandler = () => {
        // clear local storage on logout
        console.log("Logout called");
        localStorage.removeItem("username");
        localStorage.removeItem("password");
        localStorage.removeItem("type");
        navigate("/");
    }

    if (isLoggedIn) {
        return (
            <>
                <div className="container-heading">
                    <div className="account-info">
                        <button onClick={() => logoutHandler()} className="logout-button">Logout</button>
                        <h1 className="heading">{capitalizeAccountType(accountInfo.type)} Account</h1>
                    </div>
                    <p className="account-heading">Welcome, {accountInfo.username}!</p>
                    <p className="account-heading">
                        (DEMO ONLY) hashed password: <br />
                        {accountInfo.password} <br />
                    </p>
                </div>
                <div className="container-vocab">
                    <h1>Vocabulary Practice</h1>
                    <button onClick={() => changeToVerbs()}>Get Verb</button>
                    <button onClick={() => onClickHandler()}>Get Word</button>
                    <br />
                    <div className="row-words">
                        {showEnglish && <p className="word">{englishWord}</p>}
                        {showSpanish && <p className="word">{spanishWord}</p>}
                        {!showEnglish && !showSpanish && <p className="word">Click one of the languages!</p>}
                        {isVerb && showVerb && <p className="word">Tense: {tense}</p>}
                    </div>
                    <br />

                    <div className="row">
                        <div className="column-buttons">
                            <button onClick={() => changeSpanishDisplay()}>Spanish</button>
                            <button onClick={() => changeEnglishDisplay()}>English</button>
                            {isVerb && <button onClick={() => changeVerbDisplay()}>Verb Tense</button>}
                        </div>
                    </div>
                </div >
                <Footer />
            </>
        )
    } else {
        return (
            <>
                {/* show error page if user has not logged in */}
                <div className="container-heading">
                    <p className="account-heading">You are not logged in.</p>
                </div>
                <div className="error-buttons">
                    <button onClick={() => navigate("/")}>Login</button>
                    <button onClick={() => navigate("/create")}>Register</button>
                </div>
                <Footer />
            </>
        )
    }
}
