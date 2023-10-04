import React from "react";
import "../../../style/css/barAuth.css"


const BarAuth = () => {
    return (
        <div className="barAuth">
            <a className="barAuth_login" href="/user/login">Увійти</a>
            <a className="barAuth_auth" href="/user/auth">Зареєструватися</a>
        </div>
    );
}

export default BarAuth;