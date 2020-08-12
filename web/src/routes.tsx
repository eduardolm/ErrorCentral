import React from 'react';
import {BrowserRouter, Route, Switch} from 'react-router-dom';
import Landing from './pages/Landing';
import Login from './pages/Login';
import User from './pages/UserList';

const Routes = () => {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" exact component={Landing} />
                <Route path='/login' component={Login} />
                <Route path='/user' component={User} />
                <Route path='/user/:id' component={User} />
                <Route exact path='/user/create' component={User} />
            </Switch>
        </BrowserRouter>
    )
}


export default Routes;