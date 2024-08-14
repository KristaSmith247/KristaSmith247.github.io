// We use Route in order to define the different routes of our application
import { Route, Routes } from "react-router-dom";
 
// We import all the components we need in our app
import Navbar from "./components/navbar";
import Login from "./components/homepage";
import Account from "./components/account";
import AccountList from "./components/accountList";
import CustomerList from "./components/customerList";
import Edit from "./components/accountEdit";
import Add from "./components/accountAdd";
import Customer from "./components/customer";
import Transactions from "./components/transactions";

const App = () => {

  return (
    <div>
      <Navbar />
      <Routes>
        <Route exact path="/" element={<Login />} />
        <Route path="/account/:id" element={<Account />} />
        <Route path="/account-list" element={<AccountList />} />
        <Route path="/customer-list" element={<CustomerList />} />
        <Route path="/edit/:id" element={<Edit />} />
        <Route path="/add" element={<Add />} />
        <Route path="/customer/:id" element={<Customer />} />
        <Route path="/transactions/:id" element={<Transactions />} />
      </Routes>
    </div>
  );
};
 
export default App;