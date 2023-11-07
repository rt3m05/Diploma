import React, { useEffect, useState } from 'react';
import UserPhoto from './userPhoto';

import UserModal from './userModal';
import Avatar from './../../image/avatar.png';
import './../../style/css/userInfo.css';
import UserEmail from './usermail';

const UserInfo = () => {
    const [email, setEmail] = useState(''); 
    const [imageUrl, setImageUrl] = useState(); 
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [userData, setUserData] = useState('');
    
    const handleClick = () => {
        setIsModalOpen(true);
    }
    useEffect(() => {
        const request = async () => {
            const token = document.cookie
                .split("; ")
                .find((row) => row.startsWith("token="))
                ?.split("=")[1];
            try {
                const response = await fetch("https://localhost:7023/api/Users", {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (!response.ok) {
                    throw new Error(`Ошибка запроса: ${response.status}`);
                }

                setUserData(await response.json());
            } catch (error) {
                console.error("Ошибка:", error.message);
            }
        };
        request();
    }, []);
    console.dir(userData);
    
    useEffect(() => {
      if (userData) {
        setEmail(userData.email);
        setImageUrl(userData.image);
      }
    }, [userData]);

    console.log("email =" + email);
    console.log("image =" + imageUrl);
    if (!imageUrl) {
        setImageUrl(Avatar);
    }

    if (email == "") {
        setEmail("John Doe or Jane Doe");
    }

  return (
    <div className="userInfo">
          <UserPhoto
              imageUrl={imageUrl}
              handleClick={handleClick}
          />
          <UserEmail
              email={email}
              handleChange={handleClick}
          />
        <UserModal
            isModalOpen={isModalOpen}
            setIsModalOpen={setIsModalOpen}
        />
    </div>
  );
}

export default UserInfo;