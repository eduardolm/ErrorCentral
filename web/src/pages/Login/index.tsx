import React, {FormEvent, useState} from "react";

import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import api from "../../services/api";

import warningIcon from '../../assets/images/icons/alert-triangle.svg';

import './styles.css';

function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');


    function handleLoginUser(e: FormEvent) {
        e.preventDefault();

        api.post('/user/login', {
            email,
            password
        })
    }

    return (
        <div id="login-page">
            <PageHeader
                title="Login"
                description="Faça seu login utilizando o formulário abaixo."
            />
            <main>
                <form onSubmit={handleLoginUser}>
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
                        <button type="submit">
                            Login
                        </button>
                    </footer>
                </form>
            </main>
        </div>
    )
}

export default Login;