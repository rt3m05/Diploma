import React from "react";
import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Auth from "./pages/Authorization";
import Login from "./pages/Login";
import Users from "./pages/Users";
import PrivateRoute from "./UI/privateRoute/privateRoute";

function App() {
  return (
      <Routes>
        <Route path="/" element={<Home/>}/>
        <Route path="/auth" element={<Auth/>}/>
        <Route path="/login" element={<Login />} />
        <PrivateRoute path="/user/listProject" element={<Users />}/>
        <PrivateRoute path="/user/tyb" element={<Users />}/>
      </Routes>
  );
}

export default App;
