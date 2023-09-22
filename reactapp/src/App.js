import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Auth from "./pages/Authorization";
import Login from "./pages/Login";
import Users from "./pages/Users";


function App() {
  return (
    <Routes>
      <Route path="/" element={<Home/>}/>
      <Route path="/user/auth" element={<Auth/>}/>
      <Route path="/user/login" element={<Login/>} />
      <Route path="/user/listproject" element={<Users/>} />
    </Routes>
  );
}
export default App;
