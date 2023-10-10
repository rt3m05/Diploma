import React from 'react';
import AddIcon from "../images2/add.png";
import ProjIcon from "../images2/proj.png";
import PlusIcon from "../images2/plus.png";
import TrashIcon from "../images2/trash.png";
import CompanyIcon from "../images2/companyLogo.png";

import Bell2Icon from "../images2/bell2.png";
import Accepted2Icon from "../images2/accepted2.png";
import Question2Icon from "../images2/question2.png";
import Menu2Icon from "../images2/menu2.png";
import SearchIcon from "../images2/search2.png";
import Search2Icon from "../images2/search.png";

import ArrowIcon from '../images2/arrow.png';
import Plus2Icon from "../images2/plus2.png";
import Star2Icon from "../images2/star2.png";
import PersonIcon from "../images2/person.png";
import GroupIcon from "../images2/group.png";
import TemplatesIcon from "../images2/templates.png";
import RecentIcon from "../images2/recent.png";

import '../styles2/ProjectsPage/mainPage.css';

function App() {
  const menuAdd = () => {
    let leftMenu = document.querySelector(".Workspace_leftMenu");
    leftMenu.classList.toggle("disable");
    leftMenu.classList.toggle("active");
};
const menuRemove = () => {
    let leftMenu = document.querySelector(".Workspace_leftMenu");
    leftMenu.classList.toggle("active");
    leftMenu.classList.toggle("disable");
};
  return (
    <div className="App">

    <div className='Workspace_leftMenu disable'>

      <div className='Workspace_lLogo'>
          <img src={CompanyIcon} alt="CompanyIcon" className='Workspace_logo'/>
          <p>Daily</p>
          <img src={ArrowIcon} alt="ArrowIcon" className='Workspace_back' onClick={menuRemove}/>
      </div>

      <div className='Workspace_leftNavBar'>
          <div className='Workspace_leftNavBarDivActive'>
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
              <img src={Plus2Icon} alt='Plus2Icon' className='Workspace_addWorkplace'/>
              <p>Робочі області</p>
          </div>
      </div>

      <div className='Workspace_trash'>
          <img src={TrashIcon} alt='Plus2Icon'/>
          <h3>Кошик</h3>
      </div>

    </div>
      
    
      <div className='App_left'>
            <div className='App_navBar2'>
                <img src={Menu2Icon} alt="menu" className='App_menuIcon2' onClick={menuAdd}/>
                <div>
                    <img src={Accepted2Icon} alt="accepted" />
                    <img src={Bell2Icon} alt="bell" />
                    <img src={Search2Icon} alt="Search2Icon" />
                    <img src={Question2Icon} alt="QuestionIcon" />  
                    <img src={CompanyIcon} alt="CompanyIcon" style={{width:'50px', height:'50px'}}/>  
                </div>     
            </div>
        <div className='App_Lhead'>

          <div className='App_group1'>

            <div className='App_rectangle'>
              <h1>Основна</h1>
            </div>

            <div className='App_addProj'>
              <a href="">
                <img src={AddIcon} alt="add" className='App_addIcon'/>
              <p>Новий проєкт</p>
              </a>
            </div>

          </div>
          
          <form action="" className='App_searchForm' method="get">
              <input type="text" placeholder="Пошук" className="App_searchText" name=""/>
              <input type="submit" value=" " className="App_searchSubmit"/>
          </form>
        </div>
        <div className='App_projects'>
          <a href='' className='App_project'>
            
            <div>
              <img src={ProjIcon} alt="ProjIcon" />
              <h2>Test 1</h2>
            </div>
            
            <h3>25.09.2023</h3>
          </a>
          <a href='' className='App_project'>

            <div>
              <img src={ProjIcon} alt="ProjIcon" />
              <h2>Test 2</h2>
            </div>
            
            <h3>25.09.2023</h3>
          </a>
          <a href='' className='App_project'>

            <div>
              <img src={ProjIcon} alt="ProjIcon" />
              <h2>Test 3</h2>
            </div>

            <h3>25.09.2023</h3>
          </a>
        </div>
      </div>
      <div className='App_right'>

        <div className='App_logo'>
          <img src={CompanyIcon} alt="CompanyIcon" />
          <p>Daily</p>
        </div>

        <div className='App_userData'>
          <p className='App_email'>example@gmail.com</p>
        </div>

        <div className='App_pages'>

          <div className='App_page'>
            <div className='App_roundActive'></div>
            <p>Основна</p>
          </div>

          <div className='App_page'>
            <div className='App_round'></div>
            <p>Спільний доступ</p>
          </div>

          <div className='App_page'>
            <div className='App_round'></div>
            <p>Обрані</p>
          </div>
          
          <div className='App_page'>
            <div className='App_round'></div>
            <p>Нещодавні</p>
          </div>

          <div className='App_page'>
            <div className='App_round'></div>
            <p>Галерея шаблонів</p>
          </div>

        </div>

        <div className='App_workspace'>

          <div>
            <a href="">
              <img src={PlusIcon} alt="add" />
            </a>
            <h3>Створити робочу область</h3>
          </div>
          
        </div>

        <div className='App_memoryUsage'>

          <div className='App_kindOfUsage'>
            <p style={{fontWeight:'bold'}}>Блоки</p>
            <p>126/1000</p>
          </div>

          <div className='App_progressBar'>

          </div>

          <div className='App_kindOfUsage'>
            <p style={{fontWeight:'bold'}}>Файли</p>
            <p>126/1000</p>
          </div>

          <div className='App_progressBar'>

          </div>

        </div>

        <a href='' className='App_trash'>
          <p>Кошик</p>
          <img src={TrashIcon} alt="trash" />
        </a>

      </div>
    </div>
  );
}

export default App;
