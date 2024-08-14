// We import NavLink to utilize the react router.
import { NavLink } from "react-router-dom";

const WinStatus = ({ status, word, reset }) => {
    if (!status) {
        return;
    }

    return <div className="winStatus">
        <p>You {status}!</p>
        <p>The word was: {word}</p>
        <button onClick={reset}>Try again?</button>
        <button>
            <NavLink to="/top-scores">
              Check Out Top Scores
            </NavLink>
        </button>
    </div>

}

export default WinStatus;
