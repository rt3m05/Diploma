import React from "react";
import MyButton from "../button/MyButton";
import Logo_google from "../../image/logo_google.png";
import Logo_apple from "../../image/logo_apple.png";
import Auth_other_back from "../../image/auth_other_back.png";

const AuthOther = () => {
    return (
        <div className="auth_other">
            <div className="auth_other_title">
                <div className="auth_other_title_line"></div>
                <div className="auth_other_title_text">Або продовжити</div>
                <div className="auth_other_title_line"></div>
            </div>
            <div className="auth_other_back">
                    <img src={Auth_other_back} alt="" />
                </div>
            <div className="auth_other_google">
                <img src={Logo_google} alt="" />
                <MyButton>Увійти з Google</MyButton>
            </div>
            <div className="auth_other_apple">
                <img src={Logo_apple} alt="" />
                <MyButton>Увійти з Apple</MyButton>
            </div>
            <p>Реєструючись,ви погоджуєтесь з xTiles's Publick Offer, Privat Policy and Cookies.</p>
        </div>
    );
}
export default AuthOther;
