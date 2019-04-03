import React, { Component } from "react";
import { Route, withRouter } from "react-router-dom";

import authClient from "../../methods/auth";

class SecuredRoute extends Component {
  componentWillMount() {
    if (!authClient.isAuthenticated()) {
      localStorage.setItem("path", this.props.location.pathname);
      this.props.history.push("/login");
    }
  }

  render() {
    const { component: Component, path, checkingSession } = this.props;
    return (
      <Route
        path={path}
        render={() => {
          if (checkingSession) {
            return <h3 className="text-center">Validating session...</h3>;
          } else {
            return <Component />;
          }
        }}
      />
    );
  }
}

export default withRouter(SecuredRoute);
