import React from 'react';

const UserPhoto = ({ imageUrl, handleClick }) => {
  return <img src={imageUrl} alt="Profile" onClick={handleClick} />;
}

export default UserPhoto;