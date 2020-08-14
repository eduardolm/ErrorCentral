import React from "react";
import PageHeader from "../../components/PageHeader";
import {useCookies} from 'react-cookie';
import {useHistory} from 'react-router-dom';

function InternalLanding() {
    const history = useHistory();
    const [cookies] = useCookies();

    if (!cookies['token']) {
        history.push('/user/login');
        alert('Sessão expirada! Favor fazer o login para prosseguir.')
    }

    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Bem-vindos!"
                description="Esta é a página principal da aplicação. Nela é possível escolher qual o tipo de informação desejado."
            />
        </div>
    )
}

export default InternalLanding;