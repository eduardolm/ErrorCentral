import React from "react";
import {Link} from 'react-router-dom';
import logoImg from '../../assets/images/logo.png';
import loginIcon from '../../assets/images/icons/login.svg';
import registrationIcon from '../../assets/images/icons/registration.png';

import './styles.css';

function Landing(){
    return (
        <div id="landing-page">
            <div id="landing-page-content" className="container">
                <div className="logo-container">
                    <img src={logoImg} alt="Central de Erros" />
                </div>
                <div className="buttons-container">
                    <Link to="/login" className="login">
                        <img src={loginIcon} alt="Estudar" />
                        Login
                    </Link>

                    <Link to="/register" className="register">
                        <img src={registrationIcon} alt="Dar aulas" />
                        Cadastro
                    </Link>
                </div>
            </div>
        </div>
    )
}

export default Landing;