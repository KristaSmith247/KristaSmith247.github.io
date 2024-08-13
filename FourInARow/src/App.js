import { useState } from "react"; // allows us to create states to remember things


function Square({ value, onSquareClick }) {
    return (
        <button className="square"
            onClick={onSquareClick}
        >
            {value}
        </button>
    );

}

export default function Board() {
    // create a component named Board

    // these lines are rerun every time board is initialized
    const [xIsNext, setXIsNext] = useState(true);
    const [squares, setSquares] = useState(Array(42).fill(null));

    function handleClick(i) {
        if (squares[i] || calculateWinner(squares)) {
            // if square is already filled, return same value
            return;
        }
        const nextSquares = squares.slice(); // make a copy of the array
        if (xIsNext) {
            nextSquares[i] = "X"; // update 0th index
        } else {
            nextSquares[i] = "O";
        }
        setSquares(nextSquares); // here is your new 42-element array to use
        setXIsNext(!xIsNext);
    }

    const winner = calculateWinner(squares);
    let status;
    if (winner && winner !== "Tie!") {
        // print winner
        status = "Winner: " + winner;
    }
    else if (winner && winner === "Tie!") {
        status = "Tie!";
    }
    else {
        // if no winner, print next player
        status = "Next player: " + (xIsNext ? "X" : "O");
    }

    return (
        <>
            <div className="status">{status}</div>
            <div className="board-row">
                <Square value={squares[0]} onSquareClick={() => handleClick(0)} />
                <Square value={squares[1]} onSquareClick={() => handleClick(1)} />
                <Square value={squares[2]} onSquareClick={() => handleClick(2)} />
                <Square value={squares[3]} onSquareClick={() => handleClick(3)} />
                <Square value={squares[4]} onSquareClick={() => handleClick(4)} />
                <Square value={squares[5]} onSquareClick={() => handleClick(5)} />
                <Square value={squares[6]} onSquareClick={() => handleClick(6)} />
            </div>
            <div className="board-row">
                <Square value={squares[7]} onSquareClick={() => handleClick(7)} />
                <Square value={squares[8]} onSquareClick={() => handleClick(8)} />
                <Square value={squares[9]} onSquareClick={() => handleClick(9)} />
                <Square value={squares[10]} onSquareClick={() => handleClick(10)} />
                <Square value={squares[11]} onSquareClick={() => handleClick(11)} />
                <Square value={squares[12]} onSquareClick={() => handleClick(12)} />
                <Square value={squares[13]} onSquareClick={() => handleClick(13)} />
            </div>
            <div className="board-row">
                <Square value={squares[14]} onSquareClick={() => handleClick(14)} />
                <Square value={squares[15]} onSquareClick={() => handleClick(15)} />
                <Square value={squares[16]} onSquareClick={() => handleClick(16)} />
                <Square value={squares[17]} onSquareClick={() => handleClick(17)} />
                <Square value={squares[18]} onSquareClick={() => handleClick(18)} />
                <Square value={squares[19]} onSquareClick={() => handleClick(19)} />
                <Square value={squares[20]} onSquareClick={() => handleClick(20)} />
            </div>
            <div className="board-row">
                <Square value={squares[21]} onSquareClick={() => handleClick(21)} />
                <Square value={squares[22]} onSquareClick={() => handleClick(22)} />
                <Square value={squares[23]} onSquareClick={() => handleClick(23)} />
                <Square value={squares[24]} onSquareClick={() => handleClick(24)} />
                <Square value={squares[25]} onSquareClick={() => handleClick(25)} />
                <Square value={squares[26]} onSquareClick={() => handleClick(26)} />
                <Square value={squares[27]} onSquareClick={() => handleClick(27)} />
            </div>
            <div className="board-row">
                <Square value={squares[28]} onSquareClick={() => handleClick(28)} />
                <Square value={squares[29]} onSquareClick={() => handleClick(29)} />
                <Square value={squares[30]} onSquareClick={() => handleClick(30)} />
                <Square value={squares[31]} onSquareClick={() => handleClick(31)} />
                <Square value={squares[32]} onSquareClick={() => handleClick(32)} />
                <Square value={squares[33]} onSquareClick={() => handleClick(33)} />
                <Square value={squares[34]} onSquareClick={() => handleClick(34)} />
            </div>
            <div className="board-row">
                <Square value={squares[35]} onSquareClick={() => handleClick(35)} />
                <Square value={squares[36]} onSquareClick={() => handleClick(36)} />
                <Square value={squares[37]} onSquareClick={() => handleClick(37)} />
                <Square value={squares[38]} onSquareClick={() => handleClick(38)} />
                <Square value={squares[39]} onSquareClick={() => handleClick(39)} />
                <Square value={squares[40]} onSquareClick={() => handleClick(40)} />
                <Square value={squares[41]} onSquareClick={() => handleClick(41)} />
            </div>
        </>
    );
}

function calculateWinner(squares) {
    const lines = [
        // horizontal winning answers
        // 1st col
        [0, 1, 2, 3],
        [7, 8, 9, 10],
        [14, 15, 16, 17],
        [21, 22, 23, 24],
        [28, 29, 30, 31],
        [35, 36, 37, 38],
        // 2nd col
        [1, 2, 3, 4],
        [8, 9, 10, 11],
        [15, 16, 17, 18],
        [22, 23, 24, 25],
        [29, 30, 31, 32],
        [36, 37, 38, 39],
        // 3rd col
        [2, 3, 4, 5],
        [9, 10, 11, 12],
        [16, 17, 18, 19],
        [23, 24, 25, 26],
        [30, 31, 32, 33],
        [37, 38, 39, 40],
        // 4th col
        [3, 4, 5, 6],
        [10, 11, 12, 13],
        [17, 18, 19, 20],
        [24, 25, 26, 27],
        [31, 32, 33, 34],
        [38, 39, 40, 41],
        // vertical winning answers
        // 1st row
        [0, 7, 14, 21],
        [1, 8, 15, 22],
        [2, 9, 16, 23],
        [3, 10, 17, 24],
        [4, 11, 18, 25],
        [5, 12, 19, 26],
        [6, 13, 20, 27],
        // 2nd row
        [7, 14, 21, 28],
        [8, 15, 22, 29],
        [9, 16, 23, 30],
        [10, 17, 24, 31],
        [11, 18, 25, 32],
        [12, 19, 26, 33],
        [13, 20, 27, 34],
        // 3rd row
        [14, 21, 28, 35],
        [15, 22, 29, 36],
        [16, 23, 30, 37],
        [17, 24, 31, 38],
        [18, 25, 32, 39],
        [19, 26, 33, 40],
        [20, 27, 34, 41],
        // diagonal winning answers
        // 1st col
        [0, 8, 16, 24],
        [7, 15, 23, 31],
        [14, 22, 30, 38],
        [21, 15, 9, 3],
        [28, 22, 16, 10],
        [35, 29, 23, 17],
        // 2nd col
        [1, 9, 17, 25],
        [8, 16, 24, 32],
        [15, 23, 31, 39],
        [22, 16, 10, 4],
        [29, 23, 17, 11],
        [36, 30, 24, 18],
        // 3rd col
        [2, 10, 18, 26],
        [9, 17, 25, 33],
        [16, 24, 32, 40],
        [23, 17, 11, 5],
        [30, 24, 18, 12],
        [37, 31, 25, 19],
        // 4th col
        [3, 11, 19, 27],
        [10, 18, 26, 34],
        [17, 25, 33, 41],
        [24, 18, 12, 6],
        [31, 25, 19, 13],
        [38, 32, 26, 20]
    ];
    for (let i = 0; i < lines.length; i++) {
        const [a, b, c, d] = lines[i];
        if (squares[a] && squares[a] === squares[b] && squares[a] === squares[c] && squares[a] === squares[d]) {
            // if all values in a row, col, or diagonal match, return
            return squares[a];
        }
    }
    const isFull = squares.every((square) => square !== null);
    if (isFull) {
        // check if every square is filled in without a winner
        return "Tie!";
    }
    return null;
}
