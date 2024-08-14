// this file will contain the hangman components
import React, { useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { ReactSession } from "react-client-session";
import WinStatus from "./winStatus";
import HangmanDrawing from "./hangmanDrawing";
import "./hangmanDrawing.css";


export default function Hangman() {
    const alphabet = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];

    const [word, setWord] = useState(''); // word to guess
    const [correctLetter, setCorrectLetter] = useState([]); // list of correct letters
    const [wrongLetter, setWrongLetter] = useState([]); // list of wrong letters
    const [status, setStatus] = useState(''); // win status
    const [usedList, setUsedList] = useState([]); // list of guessed letters
    const [repeatMessage, setMessage] = useState(''); // message for repeating letters

    // Guess Count state
    const [guessCount, setGuessCount] = useState(1);

    const navigate = useNavigate(); // to redirect pages

    // function to retrieve a random word from wordChoices
    const getWord = () => {
        async function getRandomWord() {
            const response = await fetch(`http://localhost:4000/get-word/`);

            if (!response.ok) {
                const message = `An error occurred: ${response.statusText}`;
                window.alert(message);
                return;
            }

            const word = await response.json();
            setWord(word.word.toUpperCase());
        }
        getRandomWord();
        console.log(word.word);
    }

    const resetGame = () => {
        getWord();
        setStatus('');
        setCorrectLetter([]);
        setWrongLetter([]);
        setGuessCount(0);
    }

    const makeGuess = letter => {
        // handle event of user choosing a letter
        console.log("We have selected a letter");

        setMessage("");

        if (!usedList.includes(letter)) {
            // Increment guess count
            setGuessCount(guessCount + 1);
            console.log(guessCount);

            if (word.includes(letter)) {
                // when the user chooses a correct letter:
                // set state of component to correct letter
                console.log("It was a good letter");
                setCorrectLetter([...correctLetter, letter]);
                setUsedList([letter]);
            } else {
                // when user chooses a wrong letter:
                console.log("It was a bad letter");
                setWrongLetter([...wrongLetter, letter]);
                setUsedList([letter]);
            }
        } else {
            // if wrong letter is in list:
            console.log("You have already chosen this");
            setMessage("You have chosen this letter already");
        }
    }

    const delay = ms => new Promise(res => setTimeout(res, ms));

    useEffect(() => {
        // handle win scenario
        if (correctLetter.length && word.split('').every(letter => correctLetter.includes(letter))) {
            console.log("Winner");
            setStatus('win');

            async function fetchData() {
                // Add winning user
                const user = {
                    "username": ReactSession.get("username"),
                    "guesses": guessCount,
                    "wordLength": word.length,
                }
                // Post user
                const response = await fetch("http://localhost:4000/add-user", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(user),
                })
                    .catch(error => {
                        window.alert(error);
                        return;
                    });
                const userResponse = await response.json();

                // Session to capture word length
                ReactSession.set("wordLength", word.length);

                // Delay before navigation
                await delay(3000);

                navigate(`/top-scores/${word.length}`);
            }

            fetchData();
            return;

        }
    }, [correctLetter]);

    useEffect(() => {
        // handle fail scenario (runs out of 6 guesses)
        if (wrongLetter.length === 6) {
            console.log("Out of guesses");
            setStatus('lost');

            async function fetchData() {
                // Add winning user
                const user = {
                    "username": ReactSession.get("username"),
                    "guesses": guessCount,
                    "wordLength": word.length,
                }
                // Post user
                const response = await fetch("http://localhost:4000/add-user", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(user),
                })
                    .catch(error => {
                        window.alert(error);
                        return;
                    });
                const userResponse = await response.json();

                // Session to capture word length
                ReactSession.set("wordLength", word.length);

                // Delay before navigation
                await delay(3000);

                navigate(`/top-scores/${word.length}`);
            }

            fetchData();
            return;

        }

    }, [wrongLetter]);

    useEffect(resetGame, []); // reset the game board to blank

    // if user has guessed a correct letter, show the letter; else show the "_"
    const hideWord = word.split('').map(letter => correctLetter.includes(letter) ? letter : ' _ ').join('');

    // return the hidden word, the alphabet, wrong letters, win status
    return (
        <div>
            <p>{hideWord}</p>
            {alphabet.map((letter, index) =>
                <button
                    onClick={() => makeGuess(letter)}
                    key={index}>{letter}
                </button>)}
            <p>Previous incorrect guesses: </p>
            <p>{wrongLetter}</p>
            <p>{repeatMessage}</p>
            <WinStatus status={status} word={word} reset={resetGame} />
            <br />
            <br />
            <br />
            <br />
            <br />
            <HangmanDrawing wrongLetter={wrongLetter.length} />
        </div>
    );
}
