import React, { Component } from "react";
import { Link, withRouter } from "react-router-dom";

import "../styles.css";
import authClient from "../../methods/auth";

class NavBar extends Component {
  constructor(props) {
    super(props);
    this.state = {};
  }

  async componentDidUpdate() {
    if (!this.props.checkingSession && !this.state.user) {
      try {
        await this.setState({ user: await authClient.getUser() });
      } catch (err) {}
    }
  }

  signOut = () => {
    authClient.signOut();
    this.props.history.replace("/");
  };

  render() {
    let userPart = <label className="mr-2 text-white">Načítání..</label>;
    if (!this.props.checkingSession) {
      if (authClient.isAuthenticated()) {
        const { user } = this.state;
        userPart = (
          <div>
            <Link to={user ? `/user/${user.userId}` : "/"}>
              <label className="mr-2 text-white pointer">
                {user ? user.firstName : "Načítám uživatele.."}
              </label>
            </Link>
            <button
              className="btn btn-dark"
              onClick={() => {
                this.signOut();
                this.props.history.push("/");
              }}
            >
              Odhlásit se
            </button>
          </div>
        );
      } else {
        userPart = (
          <button
            className="btn btn-dark"
            onClick={() => {
              this.props.history.push("/login");
            }}
          >
            Přihlásit se
          </button>
        );
      }
    }

    return (
      <nav className="navbar navbar-dark bg-primary fixed-top">
        <Link className="navbar-brand font-weight-bold" to="/">
          Jazz Metrics
        </Link>
        {userPart}
      </nav>
    );
  }
}

export default withRouter(NavBar);
