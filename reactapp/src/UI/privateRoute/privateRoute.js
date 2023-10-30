import { React } from "react";
import { Navigate } from "react-router-dom";

const PrivateRoute = ({ Component }) => {
  const token = document.cookie
                .split("; ")
                .find((row) => row.startsWith("token="))
                ?.split("=")[1];
  return  token ? <Component/> : <Navigate to="/login"  />;
};

export default PrivateRoute;