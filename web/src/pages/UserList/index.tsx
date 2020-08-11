import React, {FormEvent, useState} from "react";

import './styles.css'
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import UserItem, {User} from "../../components/UserItem";
import api from "../../services/api";

function UserList() {
    const [users, setUsers] = useState([]);
    const [id, setId] = useState('');
    const [fullName, setFullName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [createdAt, setCreatedAt] = useState(Date.now);

    async function listUsers(e: FormEvent) {
        e.preventDefault();

        const response = await api.get('user');
        setUsers(response.data);
    }

    async function createUser(e: FormEvent) {
        e.preventDefault();

        const response = await api.post('user', {
            params: {
                fullName,
                email,
                password,
                createdAt
            }
        })
        setUsers(response.data);
    }


    return (
        <div id="user-page">
            <PageHeader
                title="Usuários"
                description="Aqui é possível listar, cadastrar, alterar e remover usuários."
            >
                <main>
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
                            <div className="list-id">
                                <Input
                                    name="userId"
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
                    <form onSubmit={createUser} className="user-create">
                        <fieldset>
                            <legend>
                                Criação
                            </legend>
                            <Input name="fullName" label="Nome completo"
                                   value={fullName}
                                   onChange={(e) => {setFullName(e.target.value)}}
                            />
                            <Input name="email" label="E-mail"
                                   value={email}
                                   onChange={(e) => {setEmail(e.target.value)}}
                            />
                            <div className="user-create-action">
                                <Input name="password" label="Senha"
                                       value={password}
                                       onChange={(e) => {setPassword(e.target.value)}}
                                />
                                <button type="submit" className="create">
                                    Gravar
                                </button>
                            </div>
                        </fieldset>
                    </form>

                    <div id="list-users-response">
                        {users.map((user: User) => {
                            return <UserItem key={user.id} user={user}/>;
                        })}
                    </div>
                </main>
            </PageHeader>
        </div>
    )
}

export default UserList;