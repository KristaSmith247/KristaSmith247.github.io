import { Route, Routes } from "react-router-dom";
import Login from "./components/Login";
import Create from "./components/Create";
import Study from "./components/Study";
import AddWord from "./components/AddWord";

const App = () => {
  return (
    <div className="App">
      <Routes>
        <Route exact path="/" element={<Login />} />
        <Route path="/create" element={<Create />} />
        <Route path="/account/:id" element={<Study />} />
        <Route path="/addword" element={<AddWord />} />
      </Routes>
    </div>
  );
}

export default App;
