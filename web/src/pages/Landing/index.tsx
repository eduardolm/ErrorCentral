import React from "react";
import logoImg from '../../assets/images/logo.png';
import ExitToAppOutlinedIcon from '@material-ui/icons/ExitToAppOutlined';
import CreateOutlinedIcon from '@material-ui/icons/CreateOutlined';
import Button from '@material-ui/core/Button';
import {useHistory} from 'react-router-dom';

import './styles.css';

function Landing() {
    const history = useHistory();

    return (
        <div id="landing-page">
            <div id="landing-page-content" className="container">
                <div className="logo-container">
                    <img src={logoImg} alt="Central de Erros" />
                </div>
                <div className="buttons-container">
                    <Button
                        size="large"
                        variant="contained"
                        color="primary"
                        onClick={() => {history.push('/user/login')}}
                        startIcon={<ExitToAppOutlinedIcon />}
                    >
                        Login
                    </Button>
                    <Button
                        size="large"
                        variant="contained"
                        onClick={() => {history.push('/user/registry')}}
                        startIcon={<CreateOutlinedIcon />}
                    >
                        Cadastrar
                    </Button>
                </div>
            </div>
        </div>
    )
}

export default Landing;