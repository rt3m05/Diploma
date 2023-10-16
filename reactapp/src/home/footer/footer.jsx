import React from "react";
import IconFacebook from "../../image/sothial_network/icon_facebook.png";
import IconTwitter from "../../image/sothial_network/icon_twitter.png";
import IconDiscord from "../../image/sothial_network/icon_discord.png";
import IconYoutube from "../../image/sothial_network/icon_youtube.png";
import IconLinkedin from "../../image/sothial_network/icon_linkedin.png";
import IconTiktok from "../../image/sothial_network/icon_tiktok.png";
import IconInstagram from "../../image/sothial_network/icon_instagram.png";
import Logo from "../../image/logo_white.png";
import "../../style/css/footer.css";


const Footer = () => {
    return (
        <div className="footer">
            <div className="footer_sothialNet">
                <img src={IconDiscord} alt="" />
                <img src={IconFacebook} alt="" />
                <img src={IconInstagram} alt="" />
                <img src={IconLinkedin} alt="" />
                <img src={IconTiktok} alt="" />
                <img src={IconTwitter} alt="" />
                <img src={IconYoutube} alt="" />
            </div>
            <div className="footer_company">
                <img src={Logo} alt="" />
                <p>Copyright © 2022-2023 Daily</p>
            </div>
        </div>
    );
}

export default Footer;