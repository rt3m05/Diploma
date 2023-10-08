import React from 'react';
import CompanyIcon from '../images2/companyLogo.png';
import HomeIcon from '../images2/home.png';
import MenuIcon from '../images2/menu.png';
import Menu2Icon from '../images2/menu2.png';
import ArrowIcon from '../images2/arrow.png';
import ProjIcon from "../images2/proj.png";
import AddIcon from "../images2/add.png";
import Plus2Icon from "../images2/plus2.png";
import StarIcon from "../images2/star.png";
import Star2Icon from "../images2/star2.png";
import UploadIcon from "../images2/upload.png";
import Upload2Icon from "../images2/upload2.png";
import BellIcon from "../images2/bell.png";
import Bell2Icon from "../images2/bell2.png";
import AcceptedIcon from "../images2/accepted.png";
import Accepted2Icon from "../images2/accepted2.png";
import QuestionIcon from "../images2/question.png";
import Question2Icon from "../images2/question2.png";
import Search2Icon from "../images2/search2.png";
import SearchIcon from "../images2/search.png";
import PersonIcon from "../images2/person.png";
import GroupIcon from "../images2/group.png";
import TemplatesIcon from "../images2/templates.png";
import RecentIcon from "../images2/recent.png";
import TrashIcon from "../images2/trash.png";
import '../styles2/Workspace/workspace.css';



function Workspace() {
    const menuAdd = () => {
        let leftMenu = document.querySelector(".leftMenu");
        leftMenu.classList.toggle("disable");
        leftMenu.classList.toggle("active");
    };
    const menuRemove = () => {
        let leftMenu = document.querySelector(".leftMenu");
        leftMenu.classList.toggle("active");
        leftMenu.classList.toggle("disable");
    };
  return (
    <div className="Workspace">
        <div className='leftMenu disable'>

            <div className='lLogo'>
                <img src={CompanyIcon} alt="CompanyIcon" className='logo'/>
                <p>Daily</p>
                <img src={ArrowIcon} alt="ArrowIcon" className='back' onClick={menuRemove}/>
            </div>

            <div className='leftNavBar'>
                <div className='leftNavBarDivActive'>
                    <img src={PersonIcon} alt='PersonIcon'/>
                    <p>Основна</p>
                </div>
                <div>
                    <img src={GroupIcon} alt='GroupIcon'/>
                    <p>Спільний доступ</p>
                </div>
                <div>
                    <img src={Star2Icon} alt='Star2Icon'/>
                    <p>Обрані</p>
                </div>
                <div>
                    <img src={RecentIcon} alt='RecentIcon'/>
                    <p>Нещодавні</p>
                </div>
                <div>
                    <img src={TemplatesIcon} alt='TemplatesIcon'/>
                    <p>Галерея шаблонів</p>
                </div>
                <div>
                    <img src={Plus2Icon} alt='Plus2Icon' className='addWorkplace'/>
                    <p>Робочі області</p>
                </div>
            </div>

            <div className='trash'>
                <img src={TrashIcon} alt='Plus2Icon'/>
                <h3>Кошик</h3>
            </div>

        </div>

        <div className='navBar'>

            <img src={CompanyIcon} alt="CompanyIcon" className='companyIcon' />

            <a href='' className='home'>
                <img src={HomeIcon} alt="HomeIcon"  className='homeIcon' />
            </a>

            <div className='menu'>
                <img src={MenuIcon} alt="MenuIcon" className='menuIcon' onClick={menuAdd}/>
            </div>

            <div className='block'></div>

        </div>

        <div className='main'>

            <div className='navBar2'>
                <img src={Menu2Icon} alt="menu" className='menuIcon2' onClick={menuAdd}/>
                <div>
                    <img src={Accepted2Icon} alt="accepted" />
                    <img src={Bell2Icon} alt="bell" />
                    <img src={SearchIcon} alt="SearchIcon" />
                    <img src={Question2Icon} alt="QuestionIcon" />  
                    <img src={CompanyIcon} alt="CompanyIcon" style={{width:'50px', height:'50px'}}/>  
                </div>     
            </div>

            <div className='tabNavBar'>
                <div className='left'>
                    <div className='arrowsDiv'>
                        <h1>Test 1</h1>
                        <div className='arrows'>
                            <img src={ArrowIcon} alt="arrow1" />
                            <img src={ArrowIcon} alt="arrow2" />
                            <img src={Upload2Icon} alt="upload" />
                            <img src={Star2Icon} alt="star" />
                        </div>
                    </div>

                    <div className='share'>
                        <img src={AddIcon} alt="add" />
                        <p>Поділитися</p>
                    </div>

                </div>
                <div className='right'>

                    <div className='tabs'>
                        <a href='' className='currentTab'>tab 1</a>
                        <a href='' className='tab'>tab 2</a>
                        <a href='' className='tab'>tab 3</a>
                        <a href='' className='tab'>tab 4</a>
                        <a href='' className='plusTab'><img src={AddIcon} alt="add" /></a>
                    </div>

                </div>
            </div>

            <div className='field'>

            </div>

        </div>

        <div className='rightMenu'>
                <img src={AcceptedIcon} alt="accepted" />
                <img src={BellIcon} alt="bell" />
                <img src={Search2Icon} alt="SearchIcon" />
                <img src={QuestionIcon} alt="QuestionIcon" />
                <img src={StarIcon} alt="StarIcon" />
                <img src={UploadIcon} alt="UploadIcon" />
        </div>

    </div>
  );
}

export default Workspace;
