import React from "react";
import BarAuth from "./barAuth/BarAuth";
import BarNav from "./barNav/BarNav";

const Header = () => {
    return (
        <header>
            <BarNav/>
            <BarAuth/>
        </header>
    );
}

export default Header;