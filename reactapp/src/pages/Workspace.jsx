import React from 'react';
import { useParams } from 'react-router-dom';

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
    const { id } = useParams();
    const baseUrl = window.location.origin;
    let leftId=null, rightId=null;

    const fillInfo = async() => {
        let token = document.cookie;
        token = token.substring(6);

        try{
            const response = await fetch(`https://localhost:7023/api/Projects/${id}`, {
                method: "GET",
                headers: {
                "Authorization": `Bearer ${token}`,
                }
            });
            
            if (!response.ok) {
                throw new Error(`Ошибка запроса: ${response.status}`);
            }
            
            let result = await response.json();
            
            document.querySelector(".Workspace_arrowsDiv h1").textContent = result["name"];
        }
      catch(error){
        console.error('Ошибка:', error.message);
      }

        try{
            const response = await fetch("https://localhost:7023/api/Projects", {
                method: "GET",
                headers: {
                "Authorization": `Bearer ${token}`
                },
            });
            

            if (!response.ok) {
                throw new Error(`Ошибка запроса: ${response.status}`);
            }
            
            let result = await response.json();

            result.sort((a, b) => new Date(a.timeStamp) - new Date(b.timeStamp));

            const index = result.findIndex(item => item.id === id);

            if (index !== -1) {
                
                const leftItem = index > 0 ? result[index - 1] : null;
                const rightItem = index < result.length - 1 ? result[index + 1] : null;
                

                if(leftItem!==null){
                    leftId = leftItem["id"];
                }
                if(rightItem!==null){
                    rightId = rightItem["id"];
                }

            } 
            else {
                console.log("Элемент с id не найден в массиве result.");
            }
            
            }
        catch(error){
             console.error('Ошибка:', error.message);
        }

        try{
            
            const response = await fetch(`https://localhost:7023/api/Tabs?projectId=${id}`, {
                method: "GET",
                headers: {
                "Authorization": `Bearer ${token}`
                }
            });

            let result = await response.json();

            let Workspace_tabs = document.querySelector(".Workspace_tabs");
            result.sort((a, b) => new Date(b.timeStamp) - new Date(a.timeStamp));

            for(let i=0; i<result.length-1; ++i){
                Workspace_tabs.insertAdjacentHTML("afterbegin", `
                    <p class='Workspace_tab' id="${result[i].id}">${result[i].name}</p>
                `);
            }
            Workspace_tabs.insertAdjacentHTML("afterbegin", `
                <p class='Workspace_tab Workspace_current' id="${result[result.length-1].id}">${result[result.length-1].name}</p>
            `);

            
            let list = document.querySelectorAll(".Workspace_tab");

            for(let i=0; i<list.length; ++i){
                list[i].addEventListener('click', () => currentTab(list[i]));
            }
        }
        catch(error){
            console.error('Ошибка:', error.message);
       }
    }
    const currentTab = (e)=>{
        if(!e.classList.contains("Workspace_current")){
            let tab = document.querySelector(".Workspace_current");
            tab.classList.toggle("Workspace_current");
            e.classList.toggle("Workspace_current");
        }
    }
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
    const prevProject = () => {
        if(leftId!=null)
        window.location.href = `/user/workspace/${leftId}`;
    };
    const nextProject = () => {
        if(rightId!=null)
        window.location.href = `/user/workspace/${rightId}`;
    };
    const deleteProjectChoice = () => {
        let Workspace_deleteField = document.querySelector(".Workspace_deleteField");
        Workspace_deleteField.classList.toggle("Workspace_deleteField_disable");
        Workspace_deleteField.classList.toggle("Workspace_deleteField_active");
    }

    const deleteProject = async() => {
        let token = document.cookie;
        token = token.substring(6);
        try{
            const response = await fetch(`https://localhost:7023/api/Projects/${id}`, {
                method: "DELETE",
                headers: {
                "Authorization": `Bearer ${token}`
                },
            });
            if (!response.ok) {
                throw new Error(`Ошибка запроса: ${response.status}`);
            }
            window.location.href = `/user/listproject`;
        }
        catch(error){
            console.error('Ошибка:', error.message);
        }
    }

    const closeDeleteField = () => {
        let Workspace_deleteField = document.querySelector(".Workspace_deleteField");
        Workspace_deleteField.classList.toggle("Workspace_deleteField_disable");
        Workspace_deleteField.classList.toggle("Workspace_deleteField_active");
    }

    const addTabField = () => {
        let div = document.querySelector(".Workspace_addTabField");
        div.classList.toggle("Workspace_addTabField_disable");
        div.classList.toggle("Workspace_addTabField_active");
    }
    const closeAddTabField = () => {
        let div = document.querySelector(".Workspace_addTabField");
        div.classList.toggle("Workspace_addTabField_disable");
        div.classList.toggle("Workspace_addTabField_active");
    }
    const handleAddTabSubmit = async(e) => {
        e.preventDefault();
        let input = document.querySelector(`.Workspace_addTabField form [type="text"]`);
        const name = input.value;
        let token = document.cookie;
        token = token.substring(6);
        if(name==""){
            input.placeholder = "Поле не може бути пустим!";
          }
        try{
            const response = await fetch(`https://localhost:7023/api/Tabs`, {
                method: "POST",
                headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    "projectId": `${id}`,
                    "name": name
                })
            });
            if (!response.ok) {
                throw new Error(`Ошибка запроса: ${response.status}`);
            }
            input.value = "";
            closeAddTabField();
            window.location.reload();
        }
        catch(error){
            console.error('Ошибка:', error.message);
        }
    }
    const addTileItemField = ()=>{
        let div = document.querySelector(".Workspace_addTileItemField");
        div.classList.toggle("Workspace_addTabField_disable");
        div.classList.toggle("Workspace_addTabField_active");
    }
    const addTile = async()=>{
        let token = document.cookie;
        token = token.substring(6);
        let tabId = document.querySelector(".Workspace_current").id;
        let lastTilePosition;
        try{
            const response = await fetch(`https://localhost:7023/api/Tiles?tabId=${tabId}`, {
                method: "GET",
                headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"                
                },
            });
            const responseData = await response.json();
            if(responseData.length!=0){
                responseData.sort((a, b) => b.position - a.position);
                lastTilePosition = responseData[0]["position"] + 1;
                console.log("Success1");
            }
            else{
                lastTilePosition=0;
                console.log("Success2");
            }
            if (!response.ok) {
                throw new Error(`Ошибка запроса: ${response.status}`);
            }
        }
        catch(error){
            console.error('Ошибка:', error.message);
        }


        try{
            const response = await fetch(`https://localhost:7023/api/Tiles`, {
                method: "POST",
                headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    "tabId": tabId,
                    "name": "string",
                    "position": lastTilePosition
                })
            });
            if (!response.ok) {
                throw new Error(`Ошибка запроса: ${response.status}`);
            }
        }
        catch(error){
            console.error('Ошибка:', error.message);
        }
    }
    const addTileItem = async(e)=>{
        e.preventDefault();
    }
    const inputTileText = async(e)=>{
        e.preventDefault();
        console.dir(e.target.textContent);
    }

    const checkboxChange = async(e)=>{
        console.dir(e.target);
    }

    fillInfo();
  return (
    <div className="Workspace">
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

            <div className='Workspace_trash' onClick={deleteProjectChoice}>
                <img src={TrashIcon} alt='Plus2Icon'/>
                <h3>Видалити</h3>
            </div>

        </div>

        <div className='Workspace_deleteField Workspace_deleteField_disable'>
            <div>
                <h2>Ви дійсно хочете видалити проєкт?</h2>
                <div>
                    <button className='Workspace_No' onClick={closeDeleteField}>Ні</button>
                    <button className='Workspace_Yes' onClick={deleteProject}>Так</button>
                </div>

            </div>
        </div>

        <div className="Workspace_addTabField Workspace_addTabField_disable">
          <form onSubmit={handleAddTabSubmit}>
            <p onClick={closeAddTabField}>Закрити</p>
            <input type="text" name="name" placeholder="Введіть ім'я вкладки: "/>
            <div>
              <input type="submit" value="Відправити"/> 
            </div>
          </form> 
        </div>

        <div className="Workspace_addTileItemField Workspace_addTabField_disable">
          <form onSubmit={addTileItem}>
            <p onClick={addTileItemField}>Закрити</p>
            <label>Виберіть тип:</label>
            <section>
                <label htmlFor="text">Text</label>
                <input type="radio" name="tileitem" value={"text"}/>
                <br/>
                <label htmlFor="task">Task</label>
                <input type="radio" name="tileitem" value={"task"}/>
            </section>
            <div>
              <input type="submit" value="Відправити"/> 
            </div>
          </form> 
        </div>

        <div className='Workspace_navBar'>

            <img src={CompanyIcon} alt="CompanyIcon" className='Workspace_companyIcon' />

            <a href={baseUrl + "/user/listproject"} className='Workspace_home'>
                <img src={HomeIcon} alt="HomeIcon"  className='Workspace_homeIcon' />
            </a>

            <div className='Workspace_menu'>
                <img src={MenuIcon} alt="MenuIcon" className='Workspace_menuIcon' onClick={menuAdd}/>
            </div>

            <div className='Workspace_block'></div>

        </div>

        <div className='Workspace_main'>

            <div className='Workspace_navBar2'>
                <img src={Menu2Icon} alt="menu" className='Workspace_menuIcon2' onClick={menuAdd}/>
                <div>
                    <img src={Accepted2Icon} alt="accepted" />
                    <img src={Bell2Icon} alt="bell" />
                    <img src={SearchIcon} alt="SearchIcon" />
                    <img src={Question2Icon} alt="QuestionIcon" />  
                    <img src={CompanyIcon} alt="CompanyIcon" style={{width:'50px', height:'50px'}}/>  
                </div>     
            </div>

            <div className='Workspace_tabNavBar'>
                <div className='Workspace_left'>
                    <div className='Workspace_arrowsDiv'>
                        <h1></h1>
                        <div className='Workspace_arrows'>
                            <img src={ArrowIcon} alt="arrow1" onClick={prevProject}/>
                            <img src={ArrowIcon} alt="arrow2" onClick={nextProject}/>
                            <img src={Upload2Icon} alt="upload" />
                            <img src={Star2Icon} alt="star" />
                        </div>
                    </div>

                    <div className='Workspace_share'>
                        <img src={AddIcon} alt="add" />
                        <p onClick={addTile}>Додати тайл</p>
                    </div>

                </div>
                <div className='Workspace_right'>

                    <div className='Workspace_tabs'>
                        <p href='' className='Workspace_plusTab' onClick={addTabField}><img src={AddIcon} alt="add" /></p>
                    </div>

                </div>
            </div>

            <div className='Workspace_field'>
                <div className="Workspace_tile">
                    <h1 className="Workspace_TileTitle">Title</h1>
                    <div className="Workspace_text">
                        <input type="text" onChange={inputTileText} placeholder='Input some text'/>
                        <div className='Workspace_addTileItem' onClick={addTileItemField}>+</div>
                    </div>
                    <div className="Workspace_task">
                        <input type="checkbox" onChange={checkboxChange} />
                        <input type="text" onChange={inputTileText} placeholder='Input some text'/>
                        <div className='Workspace_addTileItem'>+</div>
                    </div>
                    <div className="Workspace_image">
                        <img src={ProjIcon} alt="" />
                        <div className='Workspace_addTileItem'>+</div>
                    </div>
                </div>
  
            </div>

        </div>

        <div className='Workspace_rightMenu'>
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