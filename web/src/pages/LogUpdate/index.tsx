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


function LogUpdate() {
    const history = useHistory();
    const [cookies] = useCookies();
    const token = (cookies['token']) ? `Bearer ${cookies['token'].access_token}` : '';
    const [id, setId] = useState('');
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [userId, setUserId] = useState('');
    const [environmentId, setEnvironmentId] = useState('');
    const [layerId, setLayerId] = useState('');
    const [levelId, setLevelId] = useState('');
    const [statusId, setStatusId] = useState('');


    async function handleUpdateLog(e: FormEvent) {
        e.preventDefault();

        if (!cookies['token']) {
            history.push('/user/login');
            alert('Sessão expirada! Favor fazer o login para prosseguir.')
        }
        await api.put('log/update', {id, name, description, userId, environmentId, layerId, levelId, statusId}, {
            headers: {
                authorization: token
            }
        }).then(() => {
            alert('Cadastro alterado com sucesso!');
        }).catch(() => {
            alert('Erro ao alterar o cadastro.');
        });
    }

    return (
        <div id="log-page" className="container">
            <PageHeader
                title="Alterar log"
                description="Aqui é possível alterar os logs de erros."
                menu={'log'}
            />
            <div id="nav-bar" className="nav-bar-container">
                <form onSubmit={handleUpdateLog} className="log-update">
                    <fieldset>
                        <legend>
                            Alteração
                        </legend>
                        <Input className="update-id" name="id" label="Id"
                               value={id}
                               onChange={(e) => {setId(e.target.value)}}
                        />
                        <Input className="update-name" name="name" label="Título"
                               value={name}
                               onChange={(e) => {setName(e.target.value)}}
                        />
                        <Textarea
                            name="description"
                            label="Descrição"
                            value={description}
                            onChange={(e) => {setDescription(e.target.value)}}
                        />
                        <div className="grid-container-1-update">
                            <Input className="update-user-id" name="userId" label="User Id"
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
                        <div className="grid-container-2-update">
                            <Select
                                name="level"
                                label="Criticidade"
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
                                className="update-log-button"
                                variant="contained"
                                color="primary"
                                onClick={handleUpdateLog}
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

export default LogUpdate;