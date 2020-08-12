import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';

import './styles.css'
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import UserItem, {User} from "../../components/UserItem";
import api from "../../services/api";


function UserList() {
    const [cookies] = useCookies();
    const token = `Bearer ${cookies['token'].access_token}`;
    const [users, setUsers] = useState([]);
    const [id, setId] = useState('');
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [createdAt, setCreatedAt] = useState(new Date());

    async function listUsers(e: FormEvent) {
        e.preventDefault();

        const response = await api.get('user', {
            headers: {
                authorization: token
            }
        });
        setUsers(response.data);
    }


    async function handleListUserById(e: FormEvent) {
        e.preventDefault();

        const response = await api.get('user/' + id, {
            headers: {
                authorization: token
            }
        });
        console.log(response.data);

    }

    async function handleCreateUser(e: FormEvent) {
        e.preventDefault();

        await api.post('user/create', {fullName, email, password}, {
            headers: {
                authorization: token
            }
        }).then(() => {
            alert('Cadastro realizado com sucesso!');
        }).catch(() => {
            alert('Erro no cadastro.');
        });
    }

    async function handleUpdateUser(e: FormEvent) {
        e.preventDefault();

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
        }).then(() => {
            alert('Usuário alterado com sucesso!');
        }).catch(() => {
            alert('Erro ao alterar o cadastro.');
        })
    }

    async function handleDeleteUser(e: FormEvent) {
        e.preventDefault();

        await api.delete('user/' + id, {
            headers: {
                authorization: token
            }
        }).then(() => {
            alert('Usuário excluído com sucesso!');
        }).catch(() => {
            alert('Erro ao excluir o usuário.');
        })
    }

    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Usuários"
                description="Aqui é possível listar, cadastrar, alterar e remover usuários."
            />
            <div id="nav-bar" className="nav-bar-container">
                <form onSubmit={listUsers} className="user-list">
                    <fieldset>
                        <legend>
                            Listagem
                        </legend>
                        <div className="list-all">
                            <button type="submit">
                                Listar todos
                            </button>
                        </div>
                    </fieldset>
                </form>
                <form onSubmit={handleListUserById} className="user-list-id">
                    <fieldset>
                        <div className="list-id">
                            <Input
                                name="id"
                                label="Informe o id do usuário"
                                value={id}
                                onChange={(e) => {setId(e.target.value)}}
                            />
                            <button type="submit" className="list-by-id">
                                Listar por id
                            </button>
                        </div>
                    </fieldset>
                </form>
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
                            <button type="submit" className="create">
                                Gravar
                            </button>
                        </div>
                    </fieldset>
                </form>
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
                            <button type="submit" className="update">
                                Gravar
                            </button>
                        </div>
                    </fieldset>
                </form>
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
                            <button type="submit" className="delete">
                                Excluir
                            </button>
                        </div>

                    </fieldset>
                </form>
            </div>
            <main>
                <div id="list-users-response">
                    {users.map((user: User) => {
                        return <UserItem key={user.id} user={user}/>;
                    })}
                </div>
            </main>
        </div>
    )
}

export default UserList;