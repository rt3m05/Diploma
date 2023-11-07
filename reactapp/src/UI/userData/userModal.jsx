import React, { useState } from "react";
import { Navigate } from "react-router-dom"
import UserPhoto from "./userPhoto";

import Avatar from "./../../image/avatar.png"
import MyInput from "../input/MyInput";
import MyButton from "../button/MyButton";

const UserModal = ({ isModalOpen, setIsModalOpen, id }) => {
    const [imageUrl, setImageUrl] = useState('');
    const [email, setEmail] = useState("");
    const [nikname, setNikname] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [fileError, setFileError] = useState("");
    const [emailError, setEmailError] = useState("");
    const [passwordErrors, setPasswordErrors] = useState([]);
    const [confirmPasswordError, setConfirmPasswordError] = useState("");
    const [modalError, setModalError] = useState("");

  const handleEmailChange = (e) => {
    const newEmail = e.target.value;
    setEmail(newEmail);

    const error = validateEmail(newEmail);
    setEmailError(error);
  };
     const handlePasswordChange = (e) => {
       const newPassword = e.target.value;
       setPassword(newPassword);

       const errors = validatePassword(newPassword);
       setPasswordErrors(errors);
     };

     const handleConfirmPasswordChange = (e) => {
       const newConfirmPassword = e.target.value;
       setConfirmPassword(newConfirmPassword);

       const isMatching = newConfirmPassword === password;
       setConfirmPasswordError(
         isMatching ? "" : "Не збігається з полем пароль"
       );
  };

  const validateEmail = (email) => {
    let error = "";
    if (email !== "") {
      if (!/\S+@\S+\.\S+/.test(email)) {
        error = "Невірно введений email";
      }
    }
    return error;
  };

  const validatePassword = (password) => {
    const errors = [];

    if (password !== "") {
      if (password.length < 6) {
        errors.push("Довжина має бути більше 6");
      }

      if (!/[A-Z]/.test(password)) {
        errors.push("Має бути хоча б одна велика літера");
      }

      if (!/\d/.test(password)) {
        errors.push("Має бути хоча б одна цифра");
      }
    }
    return errors;
  };

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
            } else {
              setFileError("Произошла ошибка.");
            }
          };
        };

        reader.readAsDataURL(file);
      }
    };

  const handleSubmit = async () => { 
    const token = document.cookie
      .split("; ")
      .find((row) => row.startsWith("token="))
      ?.split("=")[1];
      if (emailError || passwordErrors.length > 0 || password !== confirmPassword) {
            if (password !== confirmPassword) {
                setConfirmPasswordError("Не збігається з полем пароль");
            }
            return;
        }
    console.log("email = " + email);
        try{
            const response = await fetch(`https://localhost:7023/api/Users`, {
              method: "PUT",
              headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json",
              },
              body: JSON.stringify({
                nickname: nikname,
                email: email,
                password: password,
                confirmPassword: confirmPassword,
              }),
            });

            if (!response.ok) {
                throw new Error(`Ошибка запроса: ${response.status}`);
            }        

        } catch (error) {
            console.error('Ошибка:', error.message);

            if (error.message.includes("400") || error.message.includes("401")) {
                setModalError("Непередбачувана помилка, спробуйте пізніше.");
            } 
        }
    };
    if (!imageUrl) {
        setImageUrl(Avatar);
  }
  const handleLogout = () => {
    console.log("yes");
      document.cookie =
        "token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    <Navigate to="/login"/>
  };

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
        {modalError && (
          <div className="auth_form_error_message">{modalError}</div>
        )}
        <MyButton onClick={handleLogout} className="userInfo_userModal_logout">
          Вийти
        </MyButton>
        <UserPhoto imageUrl={imageUrl} />
        <br />
        <label for="userPhoto">Загрузить фото</label>
        <input
          type="file"
          id="userPhoto"
          accept=".png, .jpg"
          onChange={handleFileChange}
        />
        {fileError && (
          <div className="auth_form_error_message">{fileError}</div>
        )}
        <MyInput
          type="email"
          value={email}
          // onChange={(e) => setEmail(e.target.value)}
          placeholder="Електронна пошта"
          onChange={handleEmailChange}
        />
        {emailError && (
          <div className="auth_form_error_message">{emailError}</div>
        )}
        <MyInput
          type="text"
          value={nikname}
          onChange={(e) => setNikname(e.target.value)}
          placeholder="Нік"
        />
        <MyInput
          type="password"
          value={password}
          // onChange={(e) => setPassword(e.target.value)}
          placeholder="Пароль"
          onChange={handlePasswordChange}
        />
        {passwordErrors.map((error, index) => (
          <div key={index} className="auth_form_error_message">
            {error}
          </div>
        ))}
        <MyInput
          type="password"
          value={confirmPassword}
          // onChange={(e) => setConfirmPassword(e.target.value)}
          placeholder="Підтвердіть пароль"
          onChange={handleConfirmPasswordChange}
        />
        {confirmPasswordError && (
          <div className="auth_form_error_message">{confirmPasswordError}</div>
        )}
        <button onClick={handleSubmit}>Submit</button>
      </div>
    </div>
  );
};

export default UserModal;
