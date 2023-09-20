import React, { useState } from "react";
import MyInput from "../UI/input/MyInput";
import MyButton from "../UI/button/MyButton";
import Logo_google from "../image/logo_google.png";
import Logo_apple from "../image/logo_apple.png";
import Logo_company from "../image/logo_company.png";
import Auth_back from "../image/auth_back.png";
import "../style/css/auth.css";

const Auth = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [emailError, setEmailError] = useState("");
    const [passwordErrors, setPasswordErrors] = useState([]);
    const [confirmPasswordError, setConfirmPasswordError] = useState("");

    const handleEmailChange = (e) => {
        const newEmail = e.target.value;
        setEmail(newEmail);

        const error = validateEmail(newEmail);
        setEmailError(error);
    }

    const handlePasswordChange = (e) => {
        const newPassword = e.target.value;
        setPassword(newPassword);

        const errors = validatePassword(newPassword);
        setPasswordErrors(errors);
    }

    const handleConfirmPasswordChange = (e) => {
        const newConfirmPassword = e.target.value;
        setConfirmPassword(newConfirmPassword);

        const isMatching = newConfirmPassword === password;
        setConfirmPasswordError(isMatching ? "" : "Не збігається з полем пароль");
    }

    const validateEmail = (email) => {
        let error = '';
        if (email !== '') {
            if (!/\S+@\S+\.\S+/.test(email)) {
                error = "Невірно введений email";
            }
        }
        return error;
    }

    const validatePassword = (password) => {
        const errors = [];

        if (password !== '') {
            if (password.length < 4) {
                errors.push("Довжина має бути більше 4");
            }

            if (!/[A-Z]/.test(password)) {
                errors.push("Має бути хоча б одна велика літера");
            }

            if (!/\d/.test(password)) {
                errors.push("Має бути хоча б одна цифра");
            }
        }
        return errors;
    }

    const handleSubmit = (e) => {
        e.preventDefault();

        if (emailError || passwordErrors.length > 0 || password !== confirmPassword) {
            if (password !== confirmPassword) {
                setConfirmPasswordError("Не збігається з полем пароль");
            }
            return;
        }
    }

    return (
        <div className="auth_wrap">
            <div className="auth_form">
                <div className="auth_back">
                    <img src={Auth_back} alt="" />  
                </div>
                <form onSubmit={handleSubmit}>
                    <div className="auth_title">Створити аккаунт</div>
                    <MyInput 
                        type="email" 
                        placeholder="Email"
                        id="email"
                        value={email} 
                        onChange={handleEmailChange}  
                    />
                    {emailError && <div className="auth_error_message">{emailError}</div>}
                    <MyInput 
                        type="password" 
                        placeholder="Пароль"
                        id="password"
                        value={password}
                        onChange={handlePasswordChange}
                    />
                    {passwordErrors.map((error, index) => (
                        <div key={index} className="auth_error_message">{error}</div>
                    ))}
                    <MyInput 
                        id="confirm_password" 
                        type="password" 
                        placeholder="Повторити пароль"
                        value={confirmPassword}
                        onChange={handleConfirmPasswordChange}
                    />
                    {confirmPasswordError && <div className="auth_error_message">{confirmPasswordError}</div>}
                    <MyButton type="submit">Створити акаунт</MyButton>
                </form>
                <p>Або продовжити</p>
                <div className="auth_other">
                    <MyButton className="auth_other_google"><img src={Logo_google} alt="" /></MyButton>
                    <MyButton className="auth_other_apple"><img src={Logo_apple} alt="" /></MyButton>
                </div>
                <div className="auth_logo_company">
                     <img src={Logo_company} alt="" />
                </div>
                <p>Вже є акаунт? 
                    <a href="/user/login"> Вхід</a>
                </p>
            </div>            
        </div>    
    );
}

export default Auth;

