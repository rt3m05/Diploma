import { Routes, Route } from "react-router-dom";
import Home from "./contents/pages/Home";
import Auth from "./contents/pages/Authorization";
import Login from "./contents/pages/Login";


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
