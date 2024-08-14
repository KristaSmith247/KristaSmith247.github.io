import React from "react";
import { Route, Routes } from "react-router-dom";

import Homepage from "./components/homepage";
import Hangman from "./components/hangman";
import WinStatus from "./components/winStatus";
import TopScores from "./components/topScores";

const App = () => {
  return (
    <Routes>
      <Route exact path="/" element={<Homepage />} />
      <Route path="/hangman" element={<Hangman />} />
      <Route path="/top-scores/:id" element={<TopScores />} />
    </Routes>
  );
};

export default App;
