import React, {FormEvent, useState} from "react";
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import api from "../../services/api";
import {useCookies} from 'react-cookie';
import DeleteOutlineOutlinedIcon from '@material-ui/icons/DeleteOutlineOutlined';
import {useHistory} from 'react-router-dom';

import './styles.css';
import Button from "@material-ui/core/Button";
import PageFooter from "../../components/PageFooter";

function UserDelete() {
    const history = useHistory();
    const [cookies] = useCookies();
    const token = (cookies['token']) ? `Bearer ${cookies['token'].access_token}` : '';
    const [id, setId] = useState('');

    async function handleDeleteUser(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }
            const response = await api.delete('user/' + id, {
                headers: {
                    authorization: token
                }
            }
            );

            if (response.status === 204) {
                alert('Registro não encontrado.');
                return [];
            }

        } catch (e) {
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            } else if (e.statusCode === 500) {
                alert('Erro do servidor. Tente novamente em alguns minutos. Se o erro se repetir, entre em contato com o administrador do sistema');
            } else {
                history.push('/user/delete');
                alert('Erro ao realizar sua solicitação.')
            }
        }

    }
    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Excluir usuário"
                description="Aqui é possível excluir usuários."
                menu={'user'}
            />
            <div id="nav-bar" className="nav-bar-container">
                <form onSubmit={handleDeleteUser} className="user-delete">
                    <fieldset>
                        <legend>
                            Exclusão
                        </legend>
                        <div className="user-delete-action">
                            <Input className="user-delete-input" name="id" label="Id do usuário a ser excluído"
                                   value={id}
                                   onChange={(e) => {setId(e.target.value)}}
                            />
                            <Button
                                type="submit"
                                className="delete"
                                variant="contained"
                                color="primary"
                                onClick={handleDeleteUser}
                                startIcon={<DeleteOutlineOutlinedIcon />}
                            >
                                Excluir
                            </Button>
                        </div>
                    </fieldset>
                </form>
            </div>
            <PageFooter />
        </div>

    )
}

export default UserDelete;