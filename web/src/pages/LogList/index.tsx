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
import Select from "../../components/Select";
import { orderBy } from '@progress/kendo-data-query';
import _ from 'lodash';

function LogList(this: any) {
    const history = useHistory();
    const [cookies] = useCookies();
    const token = (cookies['token']) ? `Bearer ${cookies['token'].access_token}` : '';
    const [logs, setLogs] = useState([]);
    const [id, setId] = useState('');
    const [environmentId, setEnvironmentId] = useState('');
    const [levelId, setLevelId] = useState('');
    const [layerId, setLayerId] = useState('');
    const [orderId, setOrderId] = useState('');
    const [searchId, setSearchId] = useState('');
    const [description, setDescription] = useState('');
    const [output, setOutput] = useState([]);
    const [outputPayload, setOutputPayload] = useState([]);
    const [countLevel, setCountLevel] = useState('');
    const [countDescr, setCountDesc] = useState('');
    const [countLayer, setCountLayer] = useState('');


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

            if (response.status === 204) {
                alert('Nenhum registro encontrado.')
                return [];
            }

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
            console.log(response.data);
            if (response.status === 204) {
                alert('Nenhum registro encontrado.')
                return [];
            }

            if (Object.prototype.toString.call( response.data ) !== '[object Array]') {
                let currLog = [].concat(response.data);
                setLogs(currLog);
            }

        } catch (e) {
            console.log(e.statusCode);
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            } else if( e.statusCode === 404) {
                alert('Nenhum registro encontrado.')
                return [];
            }else {
                alert('Ocorreu um erro ao processar sua solicitação. Favor tentar novamente dentro de alguns minutos.');
            }

        }
    }

    async function handleAdvancedSearch(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }

            if (searchId === '1') {
                const response = await api.get(`/log/environment/${environmentId}/level/${levelId}`, {
                    headers: {
                        authorization: token
                    }
                });
                if (response.status === 204) {
                    alert('Nenhum registro encontrado.')
                    return [];
                }

                console.log(response.data);
                if (Object.prototype.toString.call(response.data) !== '[object Array]') {
                    setOutputPayload(convertToArray(response.data));
                } else {
                    setOutputPayload((response.data));
                    setLogs(response.data);
                    console.log(outputPayload);
                    console.log(logs);
                }



            } else if (searchId === '2') {
                const response = await api.get(`/log/environment?environmentId=${environmentId}&description=${description}`, {
                    headers: {
                        authorization: token
                    }
                });

                if (response.status === 204) {
                    alert('Nenhum registro encontrado.')
                    return [];
                }

                // if (orderId === '1') {
                //     setOutputPayload(orderBy(output, [{field: "levelId", dir: "asc"}]));
                // } else if (orderId === '2') {
                //
                //     _.countBy(output);
                // }

                setOutput(convertToArray(response.data));

                const deixo = sortResults(output, description);
                console.log(deixo);

            } else if (searchId === '3') {

                const response = await api.get(`/log/environment/${environmentId}/layer/${layerId}`, {
                    headers: {
                        authorization: token
                    }
                });
                if (response.status === 204) {
                    alert('Nenhum registro encontrado.')
                    return [];
                }
                setOutput(convertToArray(response.data));
            }

            setLogs(outputPayload);

        } catch (e) {
            if (e.statusCode === 401) {
                history.push('/user/login');
                alert('Sessão expirada. Favor fazer o login para prosseguir.');
            } else {
                history.push('/log/list');
                alert('Ocorreu um erro ao processar sua solicitação. Favor tentar novamente dentro de alguns minutos.');
            }
        }
    }

    function sortResults(data: any, sorter: any) {

        if (orderId === '1') {
            return orderBy(data, [{field: sorter, dir:  "asc"}]);
        }

        if (orderId === '2') {
            let edu = orderBy(data, [{field: sorter, dir: "desc"}]);
            let teste = _.countBy(edu);
            console.log(teste);
        }
    }

    function convertToArray(data: any) {
        if (Object.prototype.toString.call(data) !== '[object Array]') {
            return [].concat(data);
        } else {
            return data;
        }
    }

    function handleSearchTypeSelect(e: string) {
        setSearchId(e);
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
                        <div className="grid-container-1-list">
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
                <form className="log-list-env-id-level-id">
                    <fieldset>
                        <legend>
                            Busca avançada
                        </legend>
                        <div className="grid-container-2-list">
                            <Select
                                name="environment"
                                label="Ambiente"
                                value={environmentId}
                                onChange={(e) => {setEnvironmentId(e.target.value)}}
                                options={[
                                    {value: 1, label: 'Desenvolvimento'},
                                    {value: 2, label: 'Homologação'},
                                    {value: 3, label: 'Produção'}
                                ]}
                            />
                            <Select
                                name="orderby"
                                label="Ordenar por"
                                value={orderId}
                                onChange={(e) => {setOrderId(e.target.value)}}
                                options={[
                                    {value: 1, label: 'Level'},
                                    {value: 2, label: 'Frequência'}
                                ]}
                            />
                            <Select
                                name="searchfor"
                                label="Buscar por"
                                value={searchId}
                                onChange={(e) => {handleSearchTypeSelect(e.target.value)}}
                                options={[
                                    {value: 1, label: 'Level'},
                                    {value: 2, label: 'Descrição'},
                                    {value: 3, label: 'Origem'}
                                ]}
                            />
                        </div>
                        <div className="grid-container-3-list">
                            <div>
                                {
                                    (searchId === '' || searchId === '1' || searchId === '3') ?
                                        <div>
                                            {(searchId === '' || searchId === '1') ?
                                        <Select
                                            name="level-select"
                                            label="Level"
                                            options={[
                                                {value: 1, label: 'Debug'},
                                                {value: 2, label: 'Warning'},
                                                {value: 3, label: 'Error'}
                                            ]}
                                            onChange={(e) => setLevelId(e.target.value)}
                                        />  : <Select
                                                    name="layer-select"
                                                    label="Origem"
                                                    options={[
                                                        {value: 1, label: 'Backend'},
                                                        {value: 2, label: 'Frontend'},
                                                        {value: 3, label: 'Mobile'},
                                                        {value: 4, label: 'Desktop'}
                                                    ]}
                                                    onChange={(e) => setLayerId(e.target.value)}
                                                />}
                                        </div>
                                : <Input
                                    name="description"
                                    label="Descrição"
                                    value={description}
                                    onChange={(e) => {setDescription(e.target.value)}}
                                />
                                }
                            </div>
                            <Button
                                type="submit"
                                className="list-advanced-button"
                                variant="contained"
                                color="primary"
                                onClick={handleAdvancedSearch}
                                startIcon={<ListOutlinedIcon />}
                            >
                                Buscar
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
