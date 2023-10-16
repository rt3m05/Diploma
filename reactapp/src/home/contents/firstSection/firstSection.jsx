import React from "react";
import FirstBack from "../../../image/first_section_back.png"
import "../../../style/css/content/firstSection.css";
import MyButton from "../../../UI/button/MyButton";

const FirstSection = () => {

    const toAuth = () => {
        window.location.href = '/user/login';
    }

    return (
        <div className="firstSection">
            <img src={FirstBack} alt="" />
            <p>Універсальний робочий простір для нотаток, завдань та проєктів</p>
            <div className="firstSection_button">
                <MyButton onClick={toAuth}>Почніть - це безкоштовно</MyButton>
                <MyButton>Продукт №1</MyButton>
            </div>
        </div>
    );
}

export default FirstSection;