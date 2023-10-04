import React from "react";
import BarAuth from "./barAuth/BarAuth";
import BarNav from "./barNav/BarNav";
import "../../style/css/header.css";

const Header = () => {
    return (
        <header className="navBar">
            <BarNav/>
            <BarAuth/>
        </header>
    );
}

export default Header;