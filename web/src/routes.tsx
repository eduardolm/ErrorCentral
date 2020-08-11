import React from 'react';
import {BrowserRouter, Route} from 'react-router-dom';
import Landing from './pages/Landing';
import Login from './pages/Login';
import User from './pages/UserList';

function Routes() {
    return (
        <BrowserRouter>
            <Route path="/" exact component={Landing} />
            <Route path='/login' component={Login} />
            <Route path='/user' component={User} />
        </BrowserRouter>
    )
}

export default Routes;