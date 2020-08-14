import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';
import {useHistory} from 'react-router-dom';

import './styles.css';
import api from "../../services/api";
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import SaveAltOutlinedIcon from "@material-ui/icons/SaveAltOutlined";
import Button from "@material-ui/core/Button";

function UserUpdate() {
    const history = useHistory();
    const [cookies] = useCookies();
    const token = (cookies['token']) ? `Bearer ${cookies['token'].access_token}` : '';
    const [id, setId] = useState('');
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [createdAt, setCreatedAt] = useState(new Date());

    async function handleUpdateUser(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }
            const response = await api.get('user/' + id, {
                headers: {
                    authorization: token
                }
            });
            setCreatedAt(response.data.createdAt);

            await api.put('user', {id, fullName, email, password, createdAt}, {
                headers: {
                    authorization: token
                }
            })
        } catch (e) {
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            } else if (e.statusCode === 400) {
                alert('Erro ao atualizar o usuário. Confira os dados informados.');
            } else if (e.statusCode === 500) {
                alert('Erro do servidor. Tente novamente em alguns minutos. Se o erro se repetir, entre em contato com o administrador do sistema');
            } else {
                history.push('/main');
                alert('Erro ao realizar sua solicitação.')
            }
        }
    }

    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Alterar usuário"
                description="Aqui é possível alterar os cadastros dos usuários."
            />
            <div id="nav-bar" className="nav-bar-container">
                <form onSubmit={handleUpdateUser} className="user-update">
                    <fieldset>
                        <legend>
                            Alteração
                        </legend>
                        <Input className="update-id" name="id" label="Id do usuário a ser alterado"
                               value={id}
                               onChange={(e) => {setId(e.target.value)}}
                        />
                        <Input className="update-name" name="fullName" label="Nome completo"
                               value={fullName}
                               onChange={(e) => {setFullName(e.target.value)}}
                        />
                        <Input className="update-email" name="email" label="E-mail"
                               value={email}
                               onChange={(e) => {setEmail(e.target.value)}}
                        />
                        <div className="user-update-action">
                            <Input className="update-password" name="password" label="Senha"
                                   value={password}
                                   onChange={(e) => {setPassword(e.target.value)}}
                            />
                            <Button
                                type="submit"
                                className="update"
                                variant="contained"
                                color="primary"
                                onClick={handleUpdateUser}
                                startIcon={<SaveAltOutlinedIcon />}
                            >
                                Gravar
                            </Button>
                        </div>
                    </fieldset>
                </form>
            </div>
        </div>
    )
}

export default UserUpdate;