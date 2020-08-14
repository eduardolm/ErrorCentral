import React from "react";

import './styles.css';

export interface Log {
    id: number,
    name: string,
    description: string,
    user: string,
    environment: string,
    layer: string,
    level: string,
    status: string
    createdAt: Date
}

interface LogItemProps {
    log: Log;
}

const LogItem: React.FC<LogItemProps> = ({log}) => {
    return (
        <article className="log-item">
            <div>
                <ul className="log-item-list">
                    <li>
                        <strong>
                            Id: {'  '}{log.id}
                        </strong>
                    </li>
                    <li>
                        Título: {'  '}{log.name}
                    </li>
                    <li>
                        Descrição: {'  '}{log.description}
                    </li>
                    <li>
                        Usuário: {'  '}{log.user}
                    </li>
                    <li>
                        Ambiente: {'  '}{log.environment}
                    </li>
                    <li>
                        Camada: {'  '}{log.layer}
                    </li>
                    <li>
                        Criticidade: {'  '}{log.level}
                    </li>
                    <li>
                        Status: {'  '}{log.status}
                    </li>
                    <li>
                        Cadastro: {'  '}{log.createdAt}
                    </li>
                </ul>
            </div>
        </article>
    );
}

export default LogItem;