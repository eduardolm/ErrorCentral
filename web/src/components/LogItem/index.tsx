import React from "react";

import './styles.css';

export interface Log {
    id: number,
    name: string,
    description: string,
    user: {
        id: number
        fullName: string,
        email: string,
        createdAt: string,
    },
    environment: {
        id: number,
        name: string
    },
    layer: {
        id: number,
        name: string
    },
    level: {
        id: number,
        name: string
    },
    status: {
        id: number,
        name: string
    }
    createdAt: Date
}

interface LogItemProps {
    log: Log;
}

const LogItem: React.FC<LogItemProps> = (props) => {
    return (
        <article className="log-item">
            <header className="log-item-header">
                <h2>
                    Título:
                </h2>
                <h3>
                    {props.log.name}
                </h3>
            </header>
            <div>
                <ul className="log-item-list">
                    <li>
                        <strong>
                            Id: {'  '}{props.log.id}
                        </strong>
                    </li>
                    <li>
                        Descrição: {'  '}{props.log.description}
                    </li>
                    <li>
                        Usuário: {'  '}{props.log.user.fullName}
                    </li>
                    <li>
                        Ambiente: {'  '}{props.log.environment.name}
                    </li>
                    <li>
                        Camada: {'  '}{props.log.layer.name}
                    </li>
                    <li>
                        Criticidade: {'  '}{props.log.level.name}
                    </li>
                    <li>
                        Status: {'  '}{props.log.status.name }
                    </li>
                    <li>
                        Cadastro: {'  '}{props.log.createdAt}
                    </li>
                </ul>
            </div>
        </article>
    );
}

export default LogItem;