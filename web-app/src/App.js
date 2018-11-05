import React, { Component } from "react";
import { Route, withRouter } from "react-router-dom";

import "./App.css";
import SecuredRoute from "./components/SecuredRoute/SecuredRoute";
import ErrorBoundary from "./components/ErrorBoundary/ErrorBoundary";
import NavBar from "./components/NavBar/NavBar";
import Main from "./components/Main/Main";
import User from "./components/User/User";
import Login from "./components/Login/Login";
import authClient from "./methods/auth";

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      checkingSession: true
    };
  }

  async componentDidMount() {
    try {
      await authClient.silentAuth();
      this.forceUpdate();
    } catch (err) {}
    this.setState({ checkingSession: false });
  }

  render() {
    return (
      <div className="App">
        <NavBar checkingSession={this.state.checkingSession} />
        <ErrorBoundary>
          <div className="col-md-12">
            <Route exact path="/" component={Main} />
            <Route exact path="/Login" component={Login} />
            <SecuredRoute
              exact
              path="/User/:userId"
              component={User}
              checkingSession={this.state.checkingSession}
            />
          </div>
        </ErrorBoundary>
      </div>
    );
  }
}

export default withRouter(App);
