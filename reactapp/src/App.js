import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Auth from "./pages/Authorization";
import Login from "./pages/Login";


function App() {
  return (
    <Routes>
      <Route path="/" element={<Home/>}/>
      <Route path="/user/auth" element={<Auth/>}/>
      <Route path="/user/login" element={<Login/>}/>
    </Routes>
  );
}
export default App;
