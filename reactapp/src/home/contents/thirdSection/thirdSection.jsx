import React from "react";
import "../../../style/css/content/thirdSection.css";
import CarouselBack1 from "../../../image/carousel/carousel_back_1.png";
import CarouselBack2 from "../../../image/carousel/carousel_back_2.png";
import CarouselBack3 from "../../../image/carousel/carousel_back_3.png";
import CarouselBack4 from "../../../image/carousel/carousel_back_4.png";
import CarouselImg1 from "../../../image/carousel/carousel_img_1.png";
import CarouselImg2 from "../../../image/carousel/carousel_img_2.png";
import CarouselImg3 from "../../../image/carousel/carousel_img_3.png";
import CarouselImg4 from "../../../image/carousel/carousel_img_4.png";
import ThirdSectionImage from "../../../image/third_section_image.png";


const ThirdSection = () => {
    return (
        <div className="thirdSection">
            <div className="thirdSection_block">
                <div className="thirdSection_block_item">
                    <div className="thirdSection_block_item_text">
                        <p>Оберіть свій ідеальний планувальник</p>
                        <p>Відкрийте для себе переваги Daily швидше і надихайтеся нашими шаблонами</p>
                    </div>
                    <div className="thirdSection_block_item_slider">
                        <div className="thirdSection_block_item_slider_img">
                            <img src={CarouselBack1} alt="" />
                            <img src={CarouselImg1} alt="" />
                        </div>
                        <div className="thirdSection_block_item_slider_img">
                            <img src={CarouselBack2} alt="" />
                            <img src={CarouselImg2} alt="" />
                        </div>
                        <div className="thirdSection_block_item_slider_img">
                            <img src={CarouselBack3} alt="" />
                            <img src={CarouselImg3} alt="" />
                        </div>
                        <div className="thirdSection_block_item_slider_img">
                            <img src={CarouselBack4} alt="" />
                            <img src={CarouselImg4} alt="" />
                        </div>
                    </div>
                </div>
            </div>
            <div className="thirdSection_imageText">
                    <div className="thirdSection_imageText_img">
                        <img src={ThirdSectionImage} alt="" />
                    </div>
                    <div className="thirdSection_imageText_text">
                        <p>Організовуйте та налаштовуйте на свій розсуд</p>
                        <p>Гнучкий інтерфейс переміщення плиток дозволяє організувати стуктуру так, як це зручно саме для вас.</p>
                    </div>
            </div>
        </div>
    );
}

export default ThirdSection;