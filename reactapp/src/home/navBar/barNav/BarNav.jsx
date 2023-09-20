import React from "react"; 
import MyButton from "../../../UI/button/MyButton";
import Logo from "../../../image/logo_company.png";

const BarNav = () => {
    return (
        <div >
                <img src={Logo}/>
                <MyButton>Продукт</MyButton>
                <MyButton>Галерея</MyButton>
                <MyButton>Команда</MyButton>
            </div>
    );
}

export default BarNav;