import React from "react";
import { Route, Routes } from "react-router-dom";
import HelloWorld from "./components/helloworld.js";

const App = () => {
  return (
    <div>
      <Routes>
        <Route path="/" element={<HelloWorld />} />
      </Routes>
    </div>
  )
}

export default App;
