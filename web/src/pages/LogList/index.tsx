import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';

import './styles.css'
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import UserItem, {User} from "../../components/UserItem";
import {useHistory} from 'react-router-dom';
import api from "../../services/api";
import Button from '@material-ui/core/Button';
import ListOutlinedIcon from '@material-ui/icons/ListOutlined';
import BrandingWatermarkOutlinedIcon from '@material-ui/icons/BrandingWatermarkOutlined';
import PageFooter from "../../components/PageFooter";

function LogList(this: any) {
    const history = useHistory();
    const [cookies] = useCookies();
    const token = (cookies['token']) ? `Bearer ${cookies['token'].access_token}` : '';
    const [users, setUsers] = useState([]);
    const [id, setId] = useState('');

    async function handleListAllUsers(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }
            const response = await api.get('user', {
                headers: {
                    authorization: token
                }
            });
            setUsers(response.data);
        } catch (e) {
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            } else if (e) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            }
        }
    }

    async function handleListUserById(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }
            const response = await api.get(`/user/${id}`, {
                headers: {
                    authorization: token
                }
            });
            console.log(response.data);

            if (Object.prototype.toString.call( response.data ) !== '[object Array]') {
                let currUser = [].concat(response.data);
                setUsers(currUser);
            }

        } catch (e) {
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            }
        }
    }

    return (
        <div id="user-page" className="container">
            <PageHeader
                title="Listar usuários"
                description="Aqui é possível listar os usuários cadastrados."
                menu={'user'}
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
                                startIcon={<BrandingWatermarkOutlinedIcon />}
                            >
                                Listar por Id
                            </Button>
                        </div>
                    </fieldset>
                </form>
            </div>
            <main>
                <div id="list-users-response">
                    {
                        users.map((user: User) => {
                            return <UserItem key={user.id} user={user}/>;
                        })}
                </div>
            </main>
            <PageFooter />
        </div>
    )
}

export default LogList;
