import React, {FormEvent, useState} from "react";
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import api from "../../services/api";
import {useCookies} from 'react-cookie';
import DeleteOutlineOutlinedIcon from '@material-ui/icons/DeleteOutlineOutlined';

import './styles.css';
import Button from "@material-ui/core/Button";

function UserDelete() {
    const [cookies] = useCookies();
    const token = `Bearer ${cookies['token'].access_token}`;
    const [id, setId] = useState('');

    async function handleDeleteUser(e: FormEvent) {
        e.preventDefault();

        await api.delete('user/' + id, {
            headers: {
                authorization: token
            }
        }).then(() => {
            alert('Usuário excluído com sucesso!');
        }).catch((e) => {
            alert('Erro ao excluir o usuário.');
            console.log(e);
        })
    }
    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Excluir usuário"
                description="Aqui é possível excluir usuários."
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
        </div>

    )
}

export default UserDelete;