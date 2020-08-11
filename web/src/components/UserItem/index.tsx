import React from "react";

import './styles.css';

export interface User {
    id: number,
    fullName: string,
    email: string,
    createdAt: Date
}

interface UserItemProps {
    user: User;
}

const UserItem: React.FC<UserItemProps> = ({user}) => {
    return (
        <article className="user-item">
            <div>
                <ul className="user-item-list">
                    <li>
                        Id: {'  '}{user.id}
                    </li>
                    <li>
                        Nome: {'  '}{user.fullName}
                    </li>
                    <li>
                        E-mail: {'  '}{user.email}
                    </li>
                    <li>
                        Cadastro: {'  '}{user.createdAt}
                    </li>
                </ul>
            </div>
        </article>
    );
}

export default UserItem;