import React from 'react';
import './App.css';
import { Route, Switch, Redirect } from 'react-router';
import Layout from './pages/Layout/Layout';
import NotFound from './pages/NotFound/NotFound';
import Orders from './pages/Orders/Orders';
import { BrowserRouter } from 'react-router-dom';

const App: React.FC = () => {
  return (
    <div className="App">
      <Layout>
        <BrowserRouter>
          <Switch>
            <Route path="/" exact component={Orders} />
            <Route path="/order" exact component={Orders} />
            <Route path="/404" component={NotFound} />
            <Redirect from='*' to='/404' />
          </Switch>
        </BrowserRouter>
      </Layout>
    </div>
  );
}

export default App;
