import React, { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import HomePage from "../home/homePage";

const Home = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const token = document.cookie
      .split("; ")
      .find((row) => row.startsWith("token="))
      ?.split("=")[1];
      console.log(token);
    if (token) {
      navigate("/user/listproject");
    } 
  }, [navigate]);

  return <HomePage/>;
};

export default Home;