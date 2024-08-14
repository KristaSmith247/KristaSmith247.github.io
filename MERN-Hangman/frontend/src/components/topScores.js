// the homepage will ask the user to enter their name,
// then navigate them to the game

import React, { useEffect, useState } from "react";
import { NavLink, useNavigate, useParams } from "react-router-dom";
import { ReactSession } from "react-client-session";

const TopScore = (props) => (
    <tr>
        <td>{props.topScore.username}</td>
        <td>{props.topScore.guesses}</td>
        <td>{props.topScore.wordLength}</td>
    </tr>
);

export default function TopScores() {

    const navigate = useNavigate(); // to redirect pages

    async function onClick(e) {
        // navigate back to game
        e.preventDefault();
        navigate("/hangman");
    }

    const [topScore, setTopScores] = useState([]);

    useEffect(() => {
        // Session to get word length
        let wordLength = ReactSession.get("wordLength");

        async function getTopScores() {
            const response = await fetch(`http://localhost:4000/get-top-scores/${wordLength}`);
            if (!response.ok) {
                const message = `An error occurred: ${response.statusText}`;
                window.alert(message);
                return;
            }

            const topScores = await response.json();
            setTopScores(topScores);
        }

        getTopScores();

        return;

    }, [topScore.length]);

    // Top score list
    function topScoreList() {
        return topScore.map((topScore) => {
            return (
                <TopScore
                    topScore={topScore}
                    key={topScore._id}
                />
            );
        });
    }

    // Return Top Score List and options to play again
    return (

        <div>

            <h1>Top Scores</h1>
            <table>
                <thead>
                    <tr>
                        <th>Username</th>
                        <th>Guesses</th>
                        <th>Word Length</th>
                    </tr>
                </thead>
                <tbody>{topScoreList()}</tbody>
            </table>
            <br />
            <button onClick={onClick}>Return to Game</button>

        </div>

    );
};

