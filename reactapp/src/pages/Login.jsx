import React, { useState } from "react";
import MyInput from "../UI/input/MyInput";
import MyButton from "../UI/button/MyButton";
import AuthOther from "../UI/authOther/AuthOther";
import Logo_company from "../image/logo_company.png";
import Auth_back from "../image/auth_back.png";
import "../style/css/auth.css";

const Login = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [emailError, setEmailError] = useState("");
    const [passwordErrors, setPasswordErrors] = useState([]);
    const [formError, setFormError] = useState("");

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

    const handleSubmit = async (e) => {
        e.preventDefault();

        const response = await fetch("http://localhost/Users/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                email,
                password,
            }),
        });

        if (response.status >= 400 && response.status <= 499) {
            setFormError("Невірний логін або пароль. Спробуйте ще раз.");
        } else if (response.status >= 500 && response.status <= 599) {
            setFormError("Сервер не доступний. Спробуйте пізніше.");
        } else if (response.status === 200) {
        const data = await response.json();
        const token = data.token;
        const twoHours = 2 * 60 * 60 * 1000; 
        const expiryDate = new Date(Date.now() + twoHours);
        document.cookie = `token=${encodeURIComponent(token)}; expires=${expiryDate.toUTCString()}`;
        window.location.href = "/home";
        }
    }

    return (
        <div className="auth_wrap">
            <div className="auth_form">
                <div className="auth_form_back">
                    <img src={Auth_back} alt="" />
                </div>
                <div className="auth_logo_company">
                     <img src={Logo_company} alt="" />
                </div>
                <form onSubmit={handleSubmit}>
                    <div className="auth_title">Ласкаво просимо</div>
                    <p className="auth_text">Немає облікового запису? 
                        <a href="/user/auth"> Зареєструватися бескоштовно</a>
                    </p>
                     {formError && <div className="auth_error_message">{formError}</div>}
                    <MyInput
                        type="email"
                        placeholder="Електронна пошта"
                        value={email}
                        onChange={handleEmailChange}
                    />
                    {emailError && <div className="auth_error_message">{emailError}</div>}
                    <p className="auth_text"><a href="#">Забули пароль?</a></p>
                    <MyInput
                        type="password"
                        placeholder="Пароль"
                        value={password}
                        onChange={handlePasswordChange}
                    />
                    {passwordErrors.map((error, index) => (
                        <div key={index} className="auth_error_message">{error}</div>
                    ))}
                    <MyButton>Увйти</MyButton>
                </form>
                <AuthOther/>
            </div>            
        </div>    
    );
}

export default Login;