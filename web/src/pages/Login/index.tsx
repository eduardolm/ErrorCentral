import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';
import Button from '@material-ui/core/Button';
import {useHistory} from 'react-router-dom';
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import api from "../../services/api";
import ReportProblemOutlinedIcon from '@material-ui/icons/ReportProblemOutlined';

import warningIcon from '../../assets/images/icons/alert-triangle.svg';

import './styles.css';

function Login() {
    const history = useHistory();
    const [cookies, setCookies] = useCookies();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    async function handleLoginUser(e: FormEvent) {
        e.preventDefault();

        const response = await api.post('/user/login', {
            email,
            password
        });

        if (response.data.access_token) {
            setCookies('token', response.data, {
                path: '/',
                maxAge: 3600
            });

            history.push('/main');
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
                            <ReportProblemOutlinedIcon style={{fontSize: 30, color: '#D71414', margin: 10}}/>
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