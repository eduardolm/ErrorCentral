import React from "react";

import logoImg from '../../assets/images/logo-small.png';

import './styles.css';
import {IconButton} from "@material-ui/core";
import TemporaryDrawer from "../NavBar";
import ArrowBackIosOutlinedIcon from '@material-ui/icons/ArrowBackIosOutlined';
import {useHistory} from 'react-router-dom';


interface PageHeaderProps {
    title: string;
    description?: string;
    menu: string;
}

const PageHeader: React.FC<PageHeaderProps> = (props) => {
    const history = useHistory();
    return (
        <header className="page-header">
            <div className="top-bar-container">
                <IconButton
                    aria-label="Back"
                    size="medium"
                    style={{color: '#9C98A6'}}
                    onClick={() => history.push('/main')}
                >
                    <ArrowBackIosOutlinedIcon />
                </IconButton>
                <img src={logoImg} alt="Central de Erros" className="logout-icon"/>
            </div>
            <div className="header-content">
                <strong>
                    {props.title}
                </strong>
                {props.description && <p>{props.description}</p>}
                {props.children}
            </div>
            <div className="header-menu-container">
                <div
                    className="header-button-nav"
                >
                    <TemporaryDrawer
                        menuType={props.menu}
                    />
                </div>
            </div>
        </header>
    )
}

export default PageHeader;