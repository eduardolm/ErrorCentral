import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';
import Button from '@material-ui/core/Button';

import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import api from "../../services/api";

import warningIcon from '../../assets/images/icons/alert-triangle.svg';

import './styles.css';

function Login() {
    const [cookies, setCookies] = useCookies();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    async function handleLoginUser(e: FormEvent) {
        e.preventDefault();

        const response = await api.post('/user/login', {
            email,
            password
        });
        setCookies('token', response.data);
        const token = `Bearer ${cookies['token'].access_token}`;

        if (token) {
            api.interceptors.request.use(req => {
                req.headers.authorization = token;
                return req;
            });
        }
    }

    return (
        <div id="login-page">
            <PageHeader
                title="Login"
                description="Faça seu login utilizando o formulário abaixo."
            />
            <main>
                <form >
                    <fieldset>
                        <legend>
                            Seus dados
                        </legend>

                        <Input
                            name="email"
                            label="E-mail"
                            value={email}
                            onChange={(e) => {setEmail(e.target.value)}}
                        />
                        <Input
                            name="password"
                            label="Senha"
                            value={password}
                            onChange={(e) => {setPassword(e.target.value)}}
                        />
                    </fieldset>

                    <footer>
                        <p>
                            <img src={warningIcon} alt="Aviso importante" />
                            Importante! <br />
                            Preencha todos os dados
                        </p>
                        <Button
                            type="submit"
                            variant="contained"
                            color="primary"
                            onClick={handleLoginUser}
                        >
                            Login
                        </Button>
                    </footer>
                </form>
            </main>
        </div>
    )
}

export default Login;