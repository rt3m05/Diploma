import React, { useState } from "react";
import UserPhoto from "./userPhoto";

import Avatar from "./../../image/avatar.png"

const UserModal = ({ isModalOpen, setIsModalOpen, userInfo }) => {
    const [imageUrl, setImageUrl] = useState('');
    const [email, setEmail] = useState("");
    const [nikname, setNikname] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [fileError, setFileError] = useState("");

    const handleFileChange = (e) => {
      const file = e.target.files[0];

      if (file) {
        const reader = new FileReader();

        reader.onload = () => {
          const image = new Image();
          image.src = reader.result;
          console.log("yes")  
          image.onload = () => {
            if (image.width <= 250 && image.height <= 200) {
                setImageUrl(reader.result);
                setFileError("");
                console.log("Not problem");
            } else {
              setFileError("Изображение должно быть размером 200x200 пикселей");
            }
          };
        };

        reader.readAsDataURL(file);
      }
    };

    const handleSubmit = () => { };
    
    if (!imageUrl) {
        setImageUrl(Avatar);
    }

  return (
    <div
      className={
        isModalOpen
          ? "userInfo_userModal userInfo_userModal_active"
          : "userInfo_userModal"
      }
      onClick={(e) => {
        setIsModalOpen(false);
      }}
    >
      <div
        className={
          isModalOpen
            ? "userInfo_userModal_content userInfo_userModal_content_active"
            : "userInfo_userModal_content"
        }
        onClick={(e) => e.stopPropagation()}
      >
        <UserPhoto imageUrl={imageUrl} /><br/>
        <label for="userPhoto">Загрузить фото</label>
          <input
              type="file"
              id="userPhoto"
              accept=".png, .jpg" 
              onChange={handleFileChange}
          />
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder="Email"
        />
        <input
          type="text"
          value={nikname}
          onChange={(e) => setNikname(e.target.value)}
          placeholder="Nikname"
        />
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder="Password"
        />
        <input
          type="password"
          value={confirmPassword}
          onChange={(e) => setConfirmPassword(e.target.value)}
          placeholder="Confirm Password"
        />
        <button onClick={handleSubmit}>Submit</button>
      </div>
    </div>
  );
};

export default UserModal;
