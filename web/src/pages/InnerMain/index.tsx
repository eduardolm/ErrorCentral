import React from "react";
import PageHeader from "../../components/PageHeader";
import {useCookies} from 'react-cookie';
import {useHistory} from 'react-router-dom';
import {Button} from "@material-ui/core";
import PersonOutlineOutlinedIcon from '@material-ui/icons/PersonOutlineOutlined';
import ListAltOutlinedIcon from '@material-ui/icons/ListAltOutlined';

import './styles.css';

function InnerMain() {
    const history = useHistory();
    const [cookies] = useCookies();

    if (!cookies['token']) {
        history.push('/user/login');
        alert('Sessão expirada! Favor fazer o login para prosseguir.')
    }

    function handleGoToUser() {
        history.push('/user/list')
    }

    function handleGoToLog() {
        history.push('/log/list')
    }

    return (
        <div id="main-page" className="container">
            <PageHeader
                title="Bem-vindos!"
                description="Esta é a página principal da aplicação. Nela é possível escolher qual o tipo de informação desejado."
                menu={'user'}
            />
            <main>
                <div className="main-work-area-container"> {/*Formatado css */}
                    <div className="text-container">
                        <p>
                            Clique em "usuários" para acessar o conteúdo relacionado aos usuários, ou "logs" para ter acesso aos logs de erros.
                        </p>
                    </div>
                    <div className="main-page-buttons-container">
                        <Button
                            className="user-button"
                            size="large"
                            variant="contained"
                            style={{width: 120}}
                            color="primary"
                            startIcon={<PersonOutlineOutlinedIcon/>}
                            onClick={handleGoToUser}
                        >
                            Usuários
                        </Button>
                        <Button
                            className="log-button"
                            size="large"
                            variant="contained"
                            style={{width: 120}}
                            startIcon={<ListAltOutlinedIcon/>}
                            onClick={handleGoToLog}
                        >
                            Logs
                        </Button>
                    </div>
                </div>
            </main>
            <footer className="main-footer">
            </footer>
        </div>
    );
}

export default InnerMain;