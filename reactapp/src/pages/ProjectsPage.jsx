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
      <div className='left'>
        <div className='Lhead'>

          <div className='group1'>

            <div className='rectangle'>
              <h1>Основна</h1>
            </div>

            <div className='addProj'>
              <a href="">
                <img src={AddIcon} alt="add" className='addIcon'/>
              <p>Новий проєкт</p>
              </a>
            </div>

          </div>
          
          <form action="" className='searchForm' method="get">
              <input type="text" placeholder="Пошук" className="searchText" name=""/>
              <input type="submit" value=" " className="searchSubmit"/>
          </form>
        </div>
        <div className='projects'>
          <a href='' className='project'>
            
            <div>
              <img src={ProjIcon} alt="ProjIcon" />
              <h2>Test 1</h2>
            </div>
            
            <h3>25.09.2023</h3>
          </a>
          <a href='' className='project'>

            <div>
              <img src={ProjIcon} alt="ProjIcon" />
              <h2>Test 2</h2>
            </div>
            
            <h3>25.09.2023</h3>
          </a>
          <a href='' className='project'>

            <div>
              <img src={ProjIcon} alt="ProjIcon" />
              <h2>Test 3</h2>
            </div>

            <h3>25.09.2023</h3>
          </a>
        </div>
      </div>
      <div className='right'>

        <div className='logo'>
          <img src={CompanyIcon} alt="CompanyIcon" />
          <p>Daily</p>
        </div>

        <div className='userData'>
          <p className='email'>example@gmail.com</p>
        </div>

        <div className='pages'>

          <div className='page'>
            <div className='roundActive'></div>
            <p>Основна</p>
          </div>

          <div className='page'>
            <div className='round'></div>
            <p>Спільний доступ</p>
          </div>

          <div className='page'>
            <div className='round'></div>
            <p>Обрані</p>
          </div>
          
          <div className='page'>
            <div className='round'></div>
            <p>Нещодавні</p>
          </div>

          <div className='page'>
            <div className='round'></div>
            <p>Галерея шаблонів</p>
          </div>

        </div>

        <div className='workspace'>

          <div>
            <a href="">
              <img src={PlusIcon} alt="add" />
            </a>
            <h3>Створити робочу область</h3>
          </div>
          
        </div>

        <div className='memoryUsage'>

          <div className='kindOfUsage'>
            <p style={{fontWeight:'bold'}}>Блоки</p>
            <p>126/1000</p>
          </div>

          <div className='progressBar'>

          </div>

          <div className='kindOfUsage'>
            <p style={{fontWeight:'bold'}}>Файли</p>
            <p>126/1000</p>
          </div>

          <div className='progressBar'>

          </div>

        </div>

        <a href='' className='trash'>
          <p>Кошик</p>
          <img src={TrashIcon} alt="trash" />
        </a>

      </div>
    </div>
  );
}

export default App;
