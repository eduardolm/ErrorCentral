import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';
import Button from '@material-ui/core/Button';

import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import api from "../../services/api";

import './styles.css';
import SaveAltOutlinedIcon from "@material-ui/icons/SaveAltOutlined";
import {useHistory} from "react-router-dom";
import PageFooter from "../../components/PageFooter";

function UserCreate() {
    const history = useHistory();
    const [cookies] = useCookies();
    const token = (cookies['token']) ? `Bearer ${cookies['token'].access_token}` : '';
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    async function handleCreateUser(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }
            const response = await api.post('user/create', {fullName, email, password}, {
                headers: {
                    authorization: token
                }
            });

            if (response.status === 200) {
                alert('Cadastro realizado com sucesso!');
            }

        } catch(e) {
            if (e.status === 400) {
                alert('Verifique os dados digitados. Caso estejam corretos, já existe este registro em nosso banco de dados.');
            }
            if (e.status === 401) {
                history.push(('/user/login'));
                alert('Sessão expirada! Favor fazer o login para prosseguir.');
            }
            if (e.status >= 500) {
                alert('Ocorreu um erro interno. Favor tentar novamente dentro de alguns minutos.');
            }
        }
    }

    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Cadastrar usuáro"
                description="Aqui é possível cadastrar novos usuários."
                menu={'user'}
            />
            <div id="nav-bar" className="nav-bar-container">
                <form onSubmit={handleCreateUser} className="user-create">
                    <fieldset>
                        <legend>
                            Cadastro
                        </legend>
                        <Input className="create-name" name="fullName" label="Nome completo"
                               value={fullName}
                               onChange={(e) => {setFullName(e.target.value)}}
                        />
                        <Input className="create-email" name="email" label="E-mail"
                               value={email}
                               onChange={(e) => {setEmail(e.target.value)}}
                        />
                        <div className="user-create-action">
                            <Input className="create-password" name="password" label="Senha"
                                   value={password}
                                   onChange={(e) => {setPassword(e.target.value)}}
                            />
                            <Button
                                type="submit"
                                className="create"
                                variant="contained"
                                color="primary"
                                onClick={handleCreateUser}
                                startIcon={<SaveAltOutlinedIcon />}
                            >
                                Gravar
                            </Button>
                        </div>
                    </fieldset>
                </form>
            </div>
            <PageFooter />
        </div>

    )
}

export default UserCreate;