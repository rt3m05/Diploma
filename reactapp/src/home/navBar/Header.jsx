import React from "react";
import BarAuth from "./barAuth/BarAuth";
import BarNav from "./barNav/BarNav";
import "../../style/css/header.css";
import MobileBar from "./mobileBar/mobileBar";

const Header = () => {
    return (
        <header className="header">
            <BarNav id="top"/>
            <BarAuth />
            <MobileBar/>
        </header>
    );
}

export default Header;