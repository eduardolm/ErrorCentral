import React from 'react';
import {BrowserRouter, Route, Switch} from 'react-router-dom';
import Landing from './pages/Landing';
import Login from './pages/Login';
import UserDelete from "./pages/UserDelete";
import UserCreate from "./pages/UserCreate";
import UserUpdate from "./pages/UserUpdate";
import UserList from "./pages/UserList";
import UserRegistry from "./pages/UserRegistry";
import InnerMain from "./pages/InnerMain";

const Routes = () => {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/" exact component={Landing} />
                <Route path='/main' component={InnerMain} />
                <Route path='/user/login' component={Login} />
                <Route path='/user/registry' component={UserRegistry} />
                <Route path='/user/list' component={UserList} />
                <Route path='/user/create' component={UserCreate} />
                <Route path='/user/update' component={UserUpdate} />
                <Route path='/user/delete' component={UserDelete} />
            </Switch>
        </BrowserRouter>
    )
}

export default Routes;