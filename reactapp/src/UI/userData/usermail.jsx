import React from "react";

const UserEmail = ({ email, handleChange }) => {
  return <p onChange={handleChange}>{email}</p>;
};

export default UserEmail;
