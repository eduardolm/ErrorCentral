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

function LogDelete() {
    const history = useHistory();
    const [cookies] = useCookies();
    const token = (cookies['token']) ? `Bearer ${cookies['token'].access_token}` : '';
    const [id, setId] = useState('');

    async function handleDeleteLog(e: FormEvent) {
        e.preventDefault();

        if (!cookies['token']) {
            history.push('/user/login');
            alert('Sessão expirada! Favor fazer o login para prosseguir.')
        }

        try {
            const response = await api.delete('log/' + id, {
                headers: {
                    authorization: token
                }
            });

            if (response.status === 200) {
                alert('Registro excluído com sucesso!');
            }
            if (response.status === 204) {
                alert('Registro não encontrado.')
                return [];
            }

        } catch(e) {
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            } else if( e.statusCode === 404) {
                alert('Nenhum registro encontrado.')
                return [];
            }else {
                alert('Ocorreu um erro ao processar sua solicitação. Favor tentar novamente dentro de alguns minutos.');
            }
        }
    }
    return (
        <div id="log-page" className="container">
            <PageHeader
                title="Excluir log"
                description="Aqui é possível excluir logs."
                menu={'log'}
            />
            <div id="nav-bar" className="nav-bar-container">
                <form onSubmit={handleDeleteLog} className="log-delete">
                    <fieldset>
                        <legend>
                            Exclusão
                        </legend>
                        <div className="log-delete-action">
                            <Input className="log-delete-input" name="id" label="Id do log a ser excluído"
                                   value={id}
                                   onChange={(e) => {setId(e.target.value)}}
                            />
                            <Button
                                type="submit"
                                className="delete"
                                variant="contained"
                                color="primary"
                                onClick={handleDeleteLog}
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

export default LogDelete;