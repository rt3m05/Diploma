import React from "react";
import MyButton from "../../../UI/button/MyButton";
import ImageOne from "../../../image/second_section_image1.png";
import ImageTwo from "../../../image/second_section_image2.png";
import "../../../style/css/content/secondSection.css";

const SecondSection = () => {
    return (
        <div className="secondSection">
            <div className="secondSection_button">
                <MyButton>Планування</MyButton>
                <MyButton>Збір інформації</MyButton>
                <MyButton> Мозковий штурм</MyButton>
                <MyButton>Візуальні проєкти</MyButton>
                <MyButton>Мобільна версія</MyButton>
            </div>
            <div className="secondSection_image">
                <img className="secondSection_image_one" src={ImageOne} alt="" />
                <img className="secondSection_image_two" src={ImageTwo} alt="" />
            </div>
        </div>
    );
}

export default SecondSection;