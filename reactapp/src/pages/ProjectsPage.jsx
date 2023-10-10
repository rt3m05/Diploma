import React from 'react';
import AddIcon from "../images2/add.png";
import ProjIcon from "../images2/proj.png";
import PlusIcon from "../images2/plus.png";
import TrashIcon from "../images2/trash.png";
import CompanyIcon from "../images2/companyLogo.png";
import '../styles2/ProjectsPage/mainPage.css';

function App() {
  return (
    <div className="App">
      <div className='App_left'>
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
