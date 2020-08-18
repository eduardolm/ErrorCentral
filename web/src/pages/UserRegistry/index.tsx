import React, {FormEvent, useState} from "react";
import {useHistory} from 'react-router-dom';
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import api from "../../services/api";
import Button from '@material-ui/core/Button';
import SaveAltOutlinedIcon from '@material-ui/icons/SaveAltOutlined';


function UserRegistry() {
    const history = useHistory();
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    async function handleCreateUser(e: FormEvent) {
        e.preventDefault();

        await api.post('user/registry', {fullName, email, password}
        ).then(() => {
            alert('Cadastro realizado com sucesso!');
            history.push('/user/login')
        }).catch(() => {
            alert('Erro no cadastro.');
        });
    }

    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Novo cadastro"
                description="Bem-vindo! Faça seu cadsatro para começar a utilizar o sistema."
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
        </div>

    )
}

export default UserRegistry;