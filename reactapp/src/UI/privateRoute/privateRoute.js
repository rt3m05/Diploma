import { React, useEffect } from "react";
import {Route, useNavigate } from "react-router-dom";

const PrivateRoute = ({path, element}) => {
  const navigate = useNavigate();

  useEffect(() => {
    const token = document.cookie
      .split("; ")
      .find((row) => row.startsWith("token="))
      ?.split("=")[1];
    console.log(token);
    if (!token) {
      navigate('/login');
    } 
  }, [navigate]);

  return <Route path={path} element={element} />;
};

export default PrivateRoute;