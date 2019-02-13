import React, { Component, Fragment } from "react";
import { withRouter } from "react-router-dom";

import authClient from "../../methods/auth";
import "./Login.css";

class Login extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  componentWillMount() {
    if (authClient.isAuthenticated()) {
      this.props.history.goBack();
    }
  }

  doLogin = () => {
    this.setState({ isSubmitting: true, error: undefined });

    const { username, password } = this.state;
    if (username && password) {
      authClient
        .signIn({
          username: username,
          password: password
        })
        .then(message => {
          if (message) {
            this.setState({ error: message });
          } else {
            const path = localStorage.getItem("path");
            localStorage.removeItem("path");
            console.log(path);
            this.props.history.push(
              path.toLowerCase() === "/login" ? "/" : path || "/"
            );
          }
        });
    } else {
      if (!username) {
        this.setState({ errorUsername: "zadejte uživatelské jméno" });
      }
      if (!password) {
        this.setState({ errorPassword: "zadejte heslo" });
      }
    }

    this.setState({ isSubmitting: false });
  };

  handleUsername = event => {
    this.setState({ username: event.target.value, errorUsername: "" });
  };

  handlePassword = event => {
    this.setState({ password: event.target.value, errorPassword: "" });
  };

  render() {
    return (
      <Fragment>
        <h3 className="text-white">Přihlášení</h3>
        <div className="form-horizontal">
          <div className="col-md-3 mx-auto">
            <hr />
            <div className="form-group">
              <input
                type="text"
                className="form-control"
                value={this.state.value}
                onChange={this.handleUsername}
                placeholder="uživatelské jméno"
              />
              <div className="input-feedback text-danger">
                {this.state.errorUsername}
              </div>
            </div>
            <div className="form-group">
              <input
                type="password"
                className="form-control"
                value={this.state.value}
                placeholder="heslo"
                onChange={this.handlePassword}
              />
              <div className="input-feedback text-danger">
                {this.state.errorPassword}
              </div>
            </div>
            <div className="form-group">
              <div className="input-feedback text-danger mb-3">
                {this.state.error}
              </div>
              <input
                type="submit"
                value="Přihlášení"
                onClick={this.doLogin}
                disabled={this.state.isSubmitting}
                className="btn btn-dark"
              />
            </div>
          </div>
        </div>
      </Fragment>
    );
  }
}

export default withRouter(Login);
