import React from 'react';
import {BrowserRouter, Route} from 'react-router-dom';
import Landing from './pages/Landing';
import Login from './pages/Login';

function Routes() {
    return (
        <BrowserRouter>
            <Route path="/" exact component={Landing} />
            <Route path='/login' component={Login} />
        </BrowserRouter>
    )
}

export default Routes;