import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { TvShowOverview } from './components/TvShowOverview';
import { MovieOverview } from './components/MovieOverview'
import { PersonOverview } from './components/PersonOverview'
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/tvshows' component={TvShowOverview} />
        <Route path='/movies' component={MovieOverview} />
        <Route path='/persons' component={PersonOverview} />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}