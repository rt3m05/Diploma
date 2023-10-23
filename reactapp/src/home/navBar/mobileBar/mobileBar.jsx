import React from "react";
import "../../../style/css/burgerMenu.css";

const MobileBar = () => {


    return (
        <div className="burgerMenu">
            <div className="burgerMenu_icon">
                &#9776;
            </div>
            <div className="burgerMenu_mobileMenu">
                <a href="/">Продукт</a>
                <a href="#">Галерея</a>
                <a href="#">Команда</a>
            </div>
        </div>
    );
}

export default MobileBar;