import React from "react"; 
import MyButton from "../../../UI/button/MyButton";
import Logo from "../../../image/logo_company_home.png";
import "../../../style/css/barNav.css";

const BarNav = () => {
    return (
        <div className="barNav">
            <div className="barNav_logo">
                <img src={Logo} alt="" />
                <p>Daily</p>
            </div>
            <div className="barNav_nav">
                <MyButton>Продукт</MyButton>
                <MyButton>Галерея</MyButton>
                <MyButton>Команда</MyButton>
                </div>
            </div>
    );
}

export default BarNav;