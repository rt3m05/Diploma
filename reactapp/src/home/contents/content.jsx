import React from "react";
import FirstSection from "./firstSection/firstSection";
import SecondSection from "./secondSection/secondSection";
import ThirdSection from "./thirdSection/thirdSection";

const Content = () => {
    return (
        <div className="content">
            <FirstSection />
            <SecondSection/>
            <ThirdSection/>
        </div>
    );
}
export default Content;