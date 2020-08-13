import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';

import './styles.css'
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import UserItem, {User} from "../../components/UserItem";
import api from "../../services/api";
import Button from '@material-ui/core/Button';
import ListOutlinedIcon from '@material-ui/icons/ListOutlined';


function UserList(this: any) {
    const [cookies] = useCookies();
    const token = `Bearer ${cookies['token'].access_token}`;
    const [users, setUsers] = useState([]);
    const [id, setId] = useState('');

    async function handleListAllUsers(e: FormEvent) {
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

    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Listar usuários"
                description="Aqui é possível listar os usuários cadastrados."
            />
            <div id="nav-bar" className="nav-bar-container">
                <form onSubmit={handleListAllUsers} className="user-list">
                    <fieldset>
                        <legend>
                            Listagem
                        </legend>
                        <div className="list-all">
                            <Button
                                type="submit"
                                className="list-all"
                                variant="contained"
                                color="primary"
                                onClick={handleListAllUsers}
                                startIcon={<ListOutlinedIcon />}
                            >
                                Listar Todos
                            </Button>
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
                            <Button
                                type="submit"
                                className="list-by-id"
                                variant="contained"
                                color="primary"
                                onClick={handleListUserById}
                                startIcon={<ListOutlinedIcon />}
                            >
                                Listar por Id
                            </Button>
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
