import React from "react";
import Header from "./navBar/Header";
import Content from "./contents/content"
import Footer from "./footer/footer";
import "../style/css/home.css";

const HomePage = () => {
    return (
        <div className="wrapper">
            <Header/>
            <Content/>
            <Footer/>
        </div>
    );
}
export default HomePage;