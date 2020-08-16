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
        await api.delete('log/' + id, {
            headers: {
                authorization: token
            }
        }).then(() => {
            alert('Log excluído com sucesso!');
        }).catch((e) => {
            alert('Erro ao excluir o log.');
            console.log(e);
        })
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