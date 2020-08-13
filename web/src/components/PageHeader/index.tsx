import React from "react";
import {Link} from 'react-router-dom';

import logoImg from '../../assets/images/logo-small.png';
import backIcon from '../../assets/images/icons/back.svg';

import './styles.css';
import {IconButton} from "@material-ui/core";
import TemporaryDrawer from "../NavBar";

interface PageHeaderProps {
    title: string;
    description?: string;
}

const PageHeader: React.FC<PageHeaderProps> = (props) => {
    return (
        <header className="page-header">
            <div className="top-bar-container">
                <Link to="/">
                    <img src={backIcon} alt="Voltar" />
                </Link>
                <img src={logoImg} alt="Central de Erros" />
            </div>
            <div className="header-content">
                <strong>
                    {props.title}
                </strong>
                {props.description && <p>{props.description}</p>}
                {props.children}
            </div>
            <div className="header-menu-container">
                <IconButton
                    className="header-button-nav"
                    // color="action"
                >
                    <TemporaryDrawer />
                </IconButton>
            </div>
        </header>
    )
}

export default PageHeader;