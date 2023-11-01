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
    }

    const handleSubmit = async (e) => {
    e.preventDefault();

    try {
        const response = await fetch("https://localhost:7023/api/Users/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                email,
                password,
            }),
        });

        if (!response.ok) {
            throw new Error(`Ошибка запроса: ${response.status}`);
        }

        const token = await response.text();

        if (!token) {
            throw new Error("Отсутствует токен в ответе сервера.");
        }

        const twoHours = 2 * 60 * 60 * 1000; 
        const expiryDate = new Date(Date.now() + twoHours);
        document.cookie = `token=${encodeURIComponent(token)}; expires=${expiryDate.toUTCString()}; path=/; domain=.localhost;`;
        window.location.href = "/user/listproject";

    } catch (error) {
        console.error('Ошибка:', error.message);

        if (error.message.includes("400") || error.message.includes("401")) {
            setFormError("Невірний логін або пароль. Спробуйте ще раз.");
        } else if (error.message.includes("500") || error.message.includes("501")) {
            setFormError("Сервер не доступний. Спробуйте пізніше.");
        } else {
            setFormError("Непередбачена помилка. Будь ласка, спробуйте пізніше.");
        }
    }
}


    return (
        <div className="auth_wrap">
            <div className="auth_form">
                <div className="auth_form_back">
                    <img src={Auth_back} alt="" />
                </div>
                <div className="auth_form_logo_company">
                     <a href="/"><img src={Logo_company} alt="" /></a>
                </div>
                <form onSubmit={handleSubmit}>
                    <div className="auth_form_title">Ласкаво просимо</div>
                    <p className="auth_form_text">Немає облікового запису? 
                        <a href="/auth"> Зареєструватися</a>
                    </p>
                     {formError && <div className="auth_form_error_message">{formError}</div>}
                    <MyInput
                        className="auth_form_input"
                        type="email"
                        placeholder="Електронна пошта"
                        value={email}
                        onChange={handleEmailChange}
                    />
                    {emailError && <div className="auth_form_error_message">{emailError}</div>}
                    <p className="auth_form_text"><a href="/#">Забули пароль?</a></p>
                    <MyInput
                        className="auth_form_input"
                        type="password"
                        placeholder="Пароль"
                        value={password}
                        onChange={handlePasswordChange}
                    />
                    {passwordErrors.map((error, index) => (
                        <div key={index} className="auth_form_error_message">{error}</div>
                    ))}
                    <MyButton className="auth_form_button" >Увійти</MyButton>
                </form>
                <AuthOther/>
            </div>            
        </div>    
    );
}

export default Login;