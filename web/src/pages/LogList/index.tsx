import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';

import './styles.css'
import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import LogItem, {Log} from "../../components/LogItem";
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
    const [logs, setLogs] = useState([]);
    const [id, setId] = useState('');

    async function handleListAllLogs(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }
            const response = await api.get('log', {
                headers: {
                    authorization: token
                }
            });
            setLogs(response.data);
        } catch (e) {
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            } else if (e) {
                history.push('/user/login');
                alert('Ocorre um erro ao processar sua solicitação. Favor tentar novamente dentro de alguns minutos.');
            }
        }
    }

    async function handleListLogById(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }
            const response = await api.get(`/log/${id}`, {
                headers: {
                    authorization: token
                }

            });

            if (Object.prototype.toString.call( response.data ) !== '[object Array]') {
                let currLog = [].concat(response.data);
                setLogs(currLog);
            }
        } catch (e) {
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            } else {
                alert('Ocorreu um erro ao processar sua solicitação. Favor tentar novamente dentro de alguns minutos.');
            }
        }
    }

    return (
        <div id="log-page" className="container">
            <PageHeader
                title="Listar Logs"
                description="Aqui é possível listar os logs cadastrados."
                menu={'log'}
            />
            <div id="nav-bar" className="nav-bar-container">
                <form className="log-list">
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
                                onClick={handleListAllLogs}
                                startIcon={<ListOutlinedIcon />}
                            >
                                Listar Todos
                            </Button>
                        </div>
                    </fieldset>
                </form>
                <form className="log-list-id">
                    <fieldset>
                        <div className="grid-container-3">
                            <Input
                                name="id"
                                label="Informe o id do log"
                                value={id}
                                onChange={(e) => {setId(e.target.value)}}
                            />
                            <Button
                                type="submit"
                                className="list-by-id-button"
                                variant="contained"
                                color="primary"
                                onClick={handleListLogById}
                                startIcon={<BrandingWatermarkOutlinedIcon />}
                            >
                                Listar por Id
                            </Button>
                        </div>
                    </fieldset>
                </form>
            </div>
            <main>
                <div id="list-logs-response">
                    {
                        logs.map((log: Log) => {
                            return <LogItem key={log.id} log={log}/>;
                        })}
                </div>
            </main>
            <PageFooter />
        </div>
    )
}

export default LogList;
