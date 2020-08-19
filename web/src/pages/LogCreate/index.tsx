import React, {FormEvent, useState} from "react";
import {useCookies} from 'react-cookie';
import Button from '@material-ui/core/Button';

import PageHeader from "../../components/PageHeader";
import Input from "../../components/Input";
import api from "../../services/api";

import './styles.css';
import SaveAltOutlinedIcon from "@material-ui/icons/SaveAltOutlined";
import {useHistory} from "react-router-dom";
import PageFooter from "../../components/PageFooter";
import Textarea from "../../components/TextArea";
import Select from '../../components/Select';


function LogCreate() {
    const history = useHistory();
    const [cookies] = useCookies();
    const token = (cookies['token']) ? `Bearer ${cookies['token'].access_token}` : '';
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [userId, setUserId] = useState('');
    const [environmentId, setEnvironmentId] = useState('');
    const [layerId, setLayerId] = useState('');
    const [levelId, setLevelId] = useState('');
    const [statusId, setStatusId] = useState('');


    async function handleCreateLog(e: FormEvent) {
        e.preventDefault();

        try {
            if (!cookies['token']) {
                history.push('/user/login');
                alert('Sessão expirada! Favor fazer o login para prosseguir.')
            }
            const response = await api.post('log/create', {name, description, userId, environmentId, layerId, levelId, statusId}, {
                headers: {
                    authorization: token
                }
            });

            if (response.status === 200) {
                alert('Cadastro realizado com sucesso!');
            }

        } catch(e) {
            if (e.status === 400) {
                alert('Verifique os dados digitados. Caso estejam corretos, já existe este registro em nosso banco de dados.');
            }
            if (e.status === 401) {
                history.push(('/user/login'));
                alert('Sessão expirada! Favor fazer o login para prosseguir.');
            }
            if (e.status >= 500) {
                alert('Ocorreu um erro interno. Favor tentar novamente dentro de alguns minutos.');
            }
        }
    }

    return (
        <div id="log-page" className="container">
            <PageHeader
                title="Cadastrar log"
                description="Aqui é possível cadastrar novos logs de erros."
                menu={'log'}
            />
            <div id="nav-bar" className="nav-bar-container">
                <form onSubmit={handleCreateLog} className="log-create">
                    <fieldset>
                        <legend>
                            Cadastro
                        </legend>
                        <Input className="create-name" name="name" label="Título"
                               value={name}
                               onChange={(e) => {setName(e.target.value)}}
                        />
                        <Textarea
                            name="description"
                            label="Descrição"
                            value={description}
                            onChange={(e) => {setDescription(e.target.value)}}
                        />
                        <div className="grid-container-1-create">
                            <Input className="create-user-id" name="userId" label="User Id"
                                   value={userId}
                                   onChange={(e) => {setUserId(e.target.value)}}
                            />
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
                                name="layer"
                                label="Camada"
                                value={layerId}
                                onChange={(e) => {setLayerId(e.target.value)}}
                                options={[
                                    {value: 1, label: 'Backend'},
                                    {value: 2, label: 'Frontend'},
                                    {value: 3, label: 'Mobile'},
                                    {value: 4, label: 'Desktop'}
                                ]}
                            />
                        </div>
                        <div className="grid-container-2-create">
                            <Select
                                name="level"
                                label="Level"
                                value={levelId}
                                onChange={(e) => {setLevelId(e.target.value)}}
                                options={[
                                    {value: 1, label: 'Debug'},
                                    {value: 2, label: 'Warning'},
                                    {value: 3, label: 'Error'}
                                ]}
                            />
                            <Select
                                name="status"
                                label="Status"
                                value={statusId}
                                onChange={(e) => {setStatusId(e.target.value)}}
                                options={[
                                    {value: 1, label: 'Arquivado'},
                                    {value: 2, label: 'Pendente'},
                                    {value: 3, label: 'Ignorado'}
                                ]}
                            />
                            <Button
                                type="submit"
                                className="create-log-button"
                                variant="contained"
                                color="primary"
                                onClick={handleCreateLog}
                                startIcon={<SaveAltOutlinedIcon />}
                            >
                                Gravar
                            </Button>
                        </div>
                    </fieldset>
                </form>
            </div>
            <PageFooter />
        </div>

    )
}

export default LogCreate;