import { useState } from "react"; // allows us to create states to remember things


function Square({ value, onSquareClick }) {
    // create a square component
    // let user input a {value} to put in the square

    // IN PROGRESS: we wanted to set a state with this, but it doesn't update for 9 squares
    // const [value, setValue] = useState(null);
    // value is the thing that is read, setValue is the setter that updates value


    // IN PROGRESS: this local function allowed child components to alter their own data, we don't want that
    // function handleClick() {
    //     // handle clicking event - must build this
    //     setValue("X");

    //     // TESTING TO SEE IF EVENT IS HANDLED:
    //     // console.log("clicked!");
    //     // this will show up in the browser, not the terminal
    // }
    return (
        <button className="square"
            // onClick={handleClick} // this was used to handle the onClick event
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
    const [squares, setSquares] = useState(Array(9).fill(null));

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
        setSquares(nextSquares); // here is your new 9-element array to use
        setXIsNext(!xIsNext);
    }

    const winner = calculateWinner(squares);
    let status;
    if (winner) {
        // print winner
        status = "Winner: " + winner;
    } else {
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
            </div>
            <div className="board-row">
                <Square value={squares[3]} onSquareClick={() => handleClick(3)} />
                <Square value={squares[4]} onSquareClick={() => handleClick(4)} />
                <Square value={squares[5]} onSquareClick={() => handleClick(5)} />
            </div>
            <div className="board-row">
                <Square value={squares[6]} onSquareClick={() => handleClick(6)} />
                <Square value={squares[7]} onSquareClick={() => handleClick(7)} />
                <Square value={squares[8]} onSquareClick={() => handleClick(8)} />
            </div>
        </>
    );
}

function calculateWinner(squares) {
    const lines = [
        [0, 1, 2],
        [3, 4, 5],
        [6, 7, 8],
        [0, 3, 6],
        [1, 4, 7],
        [2, 5, 8],
        [0, 4, 8],
        [2, 4, 6]
    ];
    for (let i = 0; i < lines.length; i++) {
        const [a, b, c] = lines[i];
        if (squares[a] && squares[a] === squares[b] && squares[a] === squares[c]) {
            // if all values in a row, col, or diagonal match, return
            return squares[a];
        }
    }
    return null;
}

// states trigger when a value is changed
// value is like a class object
// C# example of const [value (Getter), setValue(setter)] = useState(initialValue):
// public class Square{
//     public string value {get; set;}
// }
// use setValue(updatedValue); NOT value = updatedValue;


